using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiftRoomBehivor : MonoBehaviour
{
    public int timeForOpen;
    public int timeForClose;
    public int timeForClimb;
    public float timeOpening;
    public AnimationCurve doorCurvetimeOpening;
    public float timeClosing;
    public AnimationCurve doorCurvetimeClosing;
    public float timeClimbingSec;
    public Vector3 positionLiftInDesert;
    public AnimationCurve speedCurvelights;
    public float speedLeave;

    private StateLift actualState;
    private GameObject player;
    private GameObject doorPos;
    private GameObject doorNeg;
    private GameObject lightSound;
    private float initialReflectionLight;
    private Vector3 initialPositionLightSound;

    //Values for restart
    private Vector3 initPosition;
    private float initTimeClimibingSec;

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

        initialReflectionLight = RenderSettings.reflectionIntensity;
        initPosition = transform.position;
        initTimeClimibingSec = timeClimbingSec;
        actualState = StateLift.Closed;
        StartCoroutine(delayForOpen());
    }

    IEnumerator delayForOpen()
    {
        yield return new WaitForSeconds(timeForOpen);
        actualState = StateLift.OpeningBelow;
        StartCoroutine(openDoorsSmooth());
    }

    private bool varPROVISIONAL = true;
    // Update is called once per frame
    void Update()
    {
        if (actualState == StateLift.Climbing)
        {
            if (varPROVISIONAL)
            {
                varPROVISIONAL = false;
                lightSound.active = true;
                initialPositionLightSound = lightSound.transform.localPosition;
            }
            climbing();
        }
    }

    IEnumerator openDoorsSmooth()
    {
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
        if ((actualState == StateLift.OpeningBelow || actualState == StateLift.OpenedBelow)
            && collider.gameObject.tag == "Player")
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

    
    IEnumerator closeDoorsSmooth()
    {
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
            RenderSettings.reflectionIntensity = 0.2f;
            actualState = StateLift.OpenedBelow;
            StartCoroutine(delayForClimb());
        }
        else if (actualState == StateLift.ClosingAvobe)
        {
            actualState = StateLift.Leaving;
            StartCoroutine(leaving());
        }
    }

    IEnumerator delayForClimb()
    {
        yield return new WaitForSeconds(timeForClimb);
        actualState = StateLift.Climbing;
    }


    void climbing()
    {
        if (timeClimbingSec > 0)
        {
            CtrlVibration.playVibration(0f, 5f);
            timeClimbingSec -= Time.deltaTime;
            moveLightLiftSound();
            vibratePlayer();
        }
        else
        {
            CtrlVibration.stopVibration();
            lightSound.active = false;
            transform.position = positionLiftInDesert;
            player.transform.parent = null;
            actualState = StateLift.OpeningAvobe;
            StartCoroutine(openDoorsSmooth());
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
        if ((actualState == StateLift.OpeningAvobe || actualState == StateLift.Avobe)
            && collider.gameObject.tag == "Player")
        {
            RenderSettings.reflectionIntensity = initialReflectionLight;
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

    }
}
