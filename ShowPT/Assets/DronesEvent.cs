using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DronesEvent : GenericEvent
{
    public List<string> tvs;
    private TVShowmanManager tVShowmanManager;
    private CtrlShieldDrones ctrlShieldDrones;

    private void Start()
    {
        tVShowmanManager = GameObject.FindGameObjectWithTag("TVShowmanManager").GetComponent<TVShowmanManager>();
        ctrlShieldDrones = GameObject.FindGameObjectWithTag("Drone").transform.parent.GetComponent<CtrlShieldDrones>();
    }

    public override void onEnableEvent()
    {
        ctrlShieldDrones.startEventDrones();
        //tVShowmanManager.playMessageAllTVs(tvs);
    }
}