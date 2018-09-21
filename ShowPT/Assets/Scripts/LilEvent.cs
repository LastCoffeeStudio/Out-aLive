using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LilEvent : GenericEvent
{
    public List<string> tvs;

    private TVShowmanManager tVShowmanManager;

    private int nextPoint = 0;
    private bool eventActive = false;

    private void Start()
    {
        tVShowmanManager = GameObject.FindGameObjectWithTag("TVShowmanManager").GetComponent<TVShowmanManager>(); 
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
       // tVShowmanManager.playMessageAllTVs(tvs);
    }
}
