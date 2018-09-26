using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DronesEvent : GenericEvent
{
    public List<string> tvs;
    private TVShowmanManager tVShowmanManager;
    public CtrlShieldDrones ctrlShieldDrones;

    private void Start()
    {
        tVShowmanManager = GameObject.FindGameObjectWithTag("TVShowmanManager").GetComponent<TVShowmanManager>();
    }

    public override void onEnableEvent()
    {
        ctrlShieldDrones.startEventDrones();
        tVShowmanManager.playMessageAllTVs(tvs, type);
    }
}