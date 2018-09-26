using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WelcomeEvent : GenericEvent
{
    public List<string> tvs;
    private TVShowmanManager tVShowmanManager;

    private void Start()
    {
        tVShowmanManager = GameObject.FindGameObjectWithTag("TVShowmanManager").GetComponent<TVShowmanManager>();
    }

    public override void onEnableEvent()
    {
        tVShowmanManager.playMessageAllTVs(tvs, type);
    }
}
