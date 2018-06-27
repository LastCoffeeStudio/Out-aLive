using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiftRoomBehivor : MonoBehaviour
{
    public int timeForOpen;
    public int timeForClose;
    public int timeForClimb;
    [Header("Modify the time and the curve")]
    public float timeOpening;
    public AnimationCurve doorCurvetimeOpening;
    [Header("Modify the time and the curve")]
    public float timeClosing;
    public AnimationCurve doorCurvetimeClosing;
    [Header("Modify the time and the curve")]
    public float timeClimbingSec;
    public AnimationCurve speedCurvelights;
    [Header("Position on appears the lift")]
	public Transform liftPlatform;
    public float speedLeave;
    public float reflectionLift;

    [Header("Audio Clips")]
    public AudioClip openDoorAudioClip;
    public AudioClip closeDoorAudioClip;
    public AudioClip soundBuzzAudioClip;
    public AudioClip climbingAudioClip;

    private StateLift actualState;
    private GameObject player;
    private GameObject doorPos;
    private GameObject doorNeg;
    private GameObject lightSound;
	private Vector3 positionLiftInDesert;
	private float altitudeDifference = 0.5f;
    private CtrlAudio ctrlAudio;

    //For know who collider is touching
    private int contCollider = 0;

    //Values for restart
    private Vector3 initPosition;
    private float initTimeClimibingSec;
    private float initialReflectionLight;
    private Vector3 initialPositionLightSound;

	[SerializeField]
	AudioClip backgroundMusic;
    private ulong idbackgroundMusic;
    [SerializeField]
	AudioClip levelMusic;

    enum StateLift
    {
        Closed,
        OpeningBelow,
        OpenedBelow,
        ClosingBelow,
        Climbing,
        Avobe,
        OpeningAvobe,
        ClosingAvobe,
        Leaving
    }

    // Use this for initialization
    void Start()
    {
        enabled = true;
        for (int i = 0; i < transform.childCount; i++)
        {
            switch (transform.GetChild(i).name)
            {
                case "DoorPos":
                    doorPos = transform.GetChild(i).gameObject;
                    break;
                case "DoorNeg":
                    doorNeg = transform.GetChild(i).gameObject;
                    break;
                case "lightSound":
                    lightSound = transform.GetChild(i).gameObject;
                    break;
            }
        }
       
        //initialReflectionLight = RenderSettings.reflectionIntensity;
        initPosition = transform.position;
        initTimeClimibingSec = timeClimbingSec;
        actualState = StateLift.Closed;
		positionLiftInDesert = new Vector3 (liftPlatform.position.x, liftPlatform.position.y + altitudeDifference, liftPlatform.position.z);
        ctrlAudio = GameObject.FindGameObjectWithTag("CtrlAudio").GetComponent<CtrlAudio>();
        idbackgroundMusic = ctrlAudio.playOneSound("Music", backgroundMusic, Vector3.zero, 0.50f, 1f, 150);
    }
    

    public void openLift()
    {
        if (actualState == StateLift.Closed)
        {
            actualState = StateLift.OpeningBelow;
            StartCoroutine(openDoorsSmooth());
        }
    }

    private bool varPROVISIONAL = true;
    private ulong varidSoundBuzz = 0;
    // Update is called once per frame
    void Update()
    {
        if (actualState == StateLift.Climbing)
        {
            if (varPROVISIONAL)
            {
                ctrlAudio.playOneSound("Scene", climbingAudioClip, transform.position, 0.7f, 0f, 100);
                varPROVISIONAL = false;
                lightSound.SetActive(true);
                initialPositionLightSound = lightSound.transform.localPosition;
                varidSoundBuzz = ctrlAudio.playOneSound("Scene", soundBuzzAudioClip, lightSound.transform.position, 0.6f, 1f, 100, lightSound);
            }
            climbing();
        }
    }

    IEnumerator openDoorsSmooth()
    {
        ctrlAudio.playOneSound("Scene", openDoorAudioClip, transform.position, 1f, 1f, 100);
        float time = 0f;
        Vector3 startRotation = new Vector3(0f, 0f, 0f);
        Vector3 endRotation = new Vector3(0f, 0f, 30f);
        while (time <= timeOpening)
        {
            time += Time.deltaTime;
            Quaternion newRotationPos = Quaternion.Euler(Vector3.Lerp(startRotation, endRotation, doorCurvetimeOpening.Evaluate(time)));
            Quaternion newRotationNeg = Quaternion.Euler(Vector3.Lerp(startRotation, -endRotation, doorCurvetimeOpening.Evaluate(time)));
            doorNeg.transform.localRotation = newRotationNeg;
            doorPos.transform.localRotation = newRotationPos;
            yield return null;
        }
        doorNeg.transform.localRotation = Quaternion.Euler(-endRotation);
        doorPos.transform.localRotation = Quaternion.Euler(endRotation);
        if (actualState == StateLift.OpeningBelow)
        {
            actualState = StateLift.OpenedBelow;
        }
        else if (actualState == StateLift.OpeningAvobe)
        {
            actualState = StateLift.Avobe;
        }


    }

    private void OnTriggerEnter(Collider collider)
    {
        ++contCollider;
        if ((actualState == StateLift.OpeningBelow || actualState == StateLift.OpenedBelow)
            && collider.gameObject.tag == "Player" && contCollider == 2)
        {
            actualState = StateLift.ClosingBelow;
            StartCoroutine(delayForClose());
            player = collider.gameObject;
            player.transform.parent = transform;
        }
    }

    IEnumerator delayForClose()
    {
        yield return new WaitForSeconds(timeForClose);
        StartCoroutine(closeDoorsSmooth());
    }

    IEnumerator delayForOpen()
    {
        yield return new WaitForSeconds(timeForOpen);
        StartCoroutine(openDoorsSmooth());
    }

    IEnumerator closeDoorsSmooth()
    {
        ctrlAudio.playOneSound("Scene", closeDoorAudioClip, transform.position, 1f, 1f, 100);
        float time = 0f;
        Vector3 startRotation = new Vector3(0f, 0f, 30f);
        Vector3 endRotation = new Vector3(0f, 0f, 0f);
        while (time <= timeClosing)
        {
            time += Time.deltaTime;
            Quaternion newRotationPos = Quaternion.Euler(Vector3.Lerp(startRotation, endRotation, doorCurvetimeClosing.Evaluate(time)));
            Quaternion newRotationNeg = Quaternion.Euler(Vector3.Lerp(-startRotation, endRotation, doorCurvetimeClosing.Evaluate(time)));
            doorNeg.transform.localRotation = newRotationNeg;
            doorPos.transform.localRotation = newRotationPos;
            yield return null;
        }
        doorNeg.transform.localRotation = Quaternion.Euler(-endRotation);
        doorPos.transform.localRotation = Quaternion.Euler(endRotation);

        

        if (actualState == StateLift.ClosingBelow)
        {
            //RenderSettings.reflectionIntensity = reflectionLift;
            if (contCollider == 2)
            {
                actualState = StateLift.OpenedBelow;
                StartCoroutine(delayForClimb());
            }
            else
            {
                actualState = StateLift.OpeningBelow;
                StartCoroutine(openDoorsSmooth());
            }
        }
        else if (actualState == StateLift.ClosingAvobe)
        {
            if (contCollider == 0)
            {
                gameObject.GetComponent<MeshCollider>().isTrigger = false;
                actualState = StateLift.Leaving;
                StartCoroutine(leaving());
            }
            else
            {
                actualState = StateLift.OpeningAvobe;
                StartCoroutine(openDoorsSmooth());
            }
        }
    }

    IEnumerator delayForClimb()
    {
        yield return new WaitForSeconds(timeForClimb);
        actualState = StateLift.Climbing;
    }


    void climbing()
    {
        ctrlAudio.stopSound(idbackgroundMusic);
        if (timeClimbingSec > 0)
        {
            CtrlVibration.playVibration(0f, 5f);
            timeClimbingSec -= Time.deltaTime;
            moveLightLiftSound();
            vibratePlayer();
        }
        else
        {
            ctrlAudio.stopSound(varidSoundBuzz);
            CtrlVibration.stopVibration();
            lightSound.SetActive(false);
            transform.position = positionLiftInDesert;
            player.transform.parent = null;
            actualState = StateLift.OpeningAvobe;
            gameObject.GetComponent<MeshCollider>().isTrigger = true;
            StartCoroutine(delayForOpen());
        }
    }

    void moveLightLiftSound()
    {
        float speed = speedCurvelights.Evaluate(timeClimbingSec) * Time.deltaTime;
        lightSound.transform.Translate(0, 0, -speed);
        if (lightSound.transform.localPosition.z < -2f)
        {
            lightSound.transform.localPosition = initialPositionLightSound;

        }
    }

    void vibratePlayer()
    {
        if (timeClimbingSec > 1 && timeClimbingSec < (initTimeClimibingSec - 1))
        {
            CtrlVibration.playVibration(10f, 10f);
            player.transform.Translate(0, Time.deltaTime * 0.5f, 0);
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        --contCollider;
        if ((actualState == StateLift.OpeningAvobe || actualState == StateLift.Avobe)
            && collider.gameObject.tag == "Player")
        {
            //RenderSettings.reflectionIntensity = initialReflectionLight;
            actualState = StateLift.ClosingAvobe;
            StartCoroutine(delayForClose());
        }
    }

    IEnumerator leaving()
    {
        while (transform.position.y > -5)
        {
            transform.Translate(0f, 0f, -speedLeave * Time.deltaTime);
            yield return null;
        }

        //Setup initial variables
        timeClimbingSec = initTimeClimibingSec;
        transform.position = initPosition;
        actualState = StateLift.Closed;
        enabled = false;
        
        ctrlAudio.playOneSound("Music", levelMusic, Vector3.zero, 0.7f, 0f, 150);
    }
}
