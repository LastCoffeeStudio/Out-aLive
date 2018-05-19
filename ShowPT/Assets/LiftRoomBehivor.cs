using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiftRoomBehivor : MonoBehaviour
{
    public int timeForOpen;
    public int timeForClose;
    public int timeForClimb;
    public float speedDoors;
    public float timeClimbingSec;
    public Vector3 positionLiftInDesert;

    private StateLift actualState;
    private GameObject player;
    public GameObject doorPos;
    public GameObject doorNeg;

    enum StateLift
    {
        Close,
        OpeningBelow,
        OpenedBelow,
        ClosingBelow,
        Climbing,
        Avobe,
        OpeningAvobe
    }

	// Use this for initialization
	void Start ()
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
            }
	    }

	    actualState = StateLift.Close;
	    StartCoroutine(delayForOpen());
    }

    IEnumerator delayForOpen()
    {
        yield return new WaitForSeconds(timeForOpen);
        actualState = StateLift.OpeningBelow;
    }


    // Update is called once per frame
    void Update () {
	    switch (actualState)
	    {
	        case StateLift.OpeningBelow:
	            openDoors();
                break;
	        case StateLift.ClosingBelow:
	            closeDoors();
	            break;
	        case StateLift.Climbing:
	            climbing();
	            break;
        }
    }
    void openDoors()
    {
        if (doorPos.transform.rotation.z < 0.20f)
        {
            doorNeg.transform.Rotate(0f, 0f, -speedDoors * Time.deltaTime);
            doorPos.transform.Rotate(0f, 0f, speedDoors * Time.deltaTime);
        }
        else
        {
            actualState = StateLift.OpenedBelow;
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        if ((actualState == StateLift.OpeningBelow || actualState == StateLift.OpenedBelow)
            && collider.gameObject.tag == "Player")
        {
            StartCoroutine(delayForClose());
            player = collider.gameObject;
            player.transform.parent = transform;
        }
    }

    IEnumerator delayForClose()
    {
        yield return new WaitForSeconds(timeForClose);
        actualState = StateLift.ClosingBelow;
    }

    void closeDoors()
    {
        if (doorPos.transform.rotation.z > 0f)
        {
            doorNeg.transform.Rotate(0f, 0f, speedDoors * Time.deltaTime);
            doorPos.transform.Rotate(0f, 0f, -speedDoors * Time.deltaTime);
        }
        else
        {
            StartCoroutine(delayForClimb());
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

            timeClimbingSec -= Time.deltaTime;
        }
        else
        {
            transform.position = positionLiftInDesert;
            player.transform.parent = null;
            actualState = StateLift.OpeningBelow;
        }
    }


    


}
