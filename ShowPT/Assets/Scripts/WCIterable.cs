using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WCIterable : InteractableObject {

    public AudioClip clip;
    private CtrlAudio ctrlAudio;

    private ulong idClip = 0;

    protected override void init()
    {
        ctrlAudio = GameObject.FindGameObjectWithTag("CtrlAudio").GetComponent<CtrlAudio>();
    }

    protected override void executeAction()
    {
        ctrlAudio.stopSound(idClip);
        idClip = ctrlAudio.playOneSound("Scene", clip, transform.position, 0.5f, 1f, 128);
    }
}
