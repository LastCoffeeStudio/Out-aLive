using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LilEvent : GenericEvent
{
    public GameObject Lil;
    public GameObject[] path;

    private LilRobot lilController;

    public GameObject tVShowmanManager;

    private int nextPoint = 0;
    private bool eventActive = false;

    private void Start()
    {
        lilController = Lil.GetComponent<LilRobot>();
        tVShowmanManager = GameObject.FindGameObjectWithTag("TVShowmanManager"); 
    }

    private void FixedUpdate()
    {
        if (eventActive)
        {
           
        }
    }

    public override void onEnableEvent()
    {
        eventActive = true;
        //tVShowmanManager.
    }
}
