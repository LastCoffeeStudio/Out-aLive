using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClapEvent : GenericEvent
{
    public List<string> tvs;
    public GameObject tv;
    private Animator animator;
    private TVShowmanManager tVShowmanManager;

    private void Start()
    {
        tVShowmanManager = GameObject.FindGameObjectWithTag("TVShowmanManager").GetComponent<TVShowmanManager>();
        animator = tv.GetComponent<Animator>();
    }

    public override void onEnableEvent()
    {
        tVShowmanManager.playMessageAllTVs(tvs, type);
        animator.Play("Claps");
    }
}
