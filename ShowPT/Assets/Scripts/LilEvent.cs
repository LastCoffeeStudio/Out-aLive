using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LilEvent : GenericEvent
{
    public GameObject Lil;
    public GameObject[] path;

    private LilRobot lilController;
    private int nextPoint = 0;
    private bool eventActive = false;

    private void Start()
    {
        lilController = Lil.GetComponent<LilRobot>();
    }

    private void FixedUpdate()
    {
        if (eventActive)
        {
            if (nextPoint >= path.Length)
            {
                eventActive = false;
                lilController.setState(LilRobot.LilRobotState.IDLE);
            }
            else
            {
                Debug.Log("mves lil");
                lilController.moveLil(path[nextPoint].transform.position);
                if (Vector3.Distance(Lil.transform.position, path[nextPoint].transform.position) <= 0.5f)
                {
                    ++nextPoint;
                }
            }
        }
    }

    public override void onEnableEvent()
    {
        eventActive = true;
        lilController.setState(LilRobot.LilRobotState.NONE);
    }
}
