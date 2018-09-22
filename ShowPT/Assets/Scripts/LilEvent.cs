using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LilEvent : GenericEvent
{
    public List<string> tvs;
    private AudiosShowmanType audiosShowmanType;
    private TVShowmanManager tVShowmanManager;

    private void Start()
    {
        tVShowmanManager = GameObject.FindGameObjectWithTag("TVShowmanManager").GetComponent<TVShowmanManager>();
        audiosShowmanType = AudiosShowmanType.DRONES;
    }

    public override void onEnableEvent()
    {
        //tVShowmanManager.playMessageAllTVs(tvs, audiosShowmanType);
    }
}
