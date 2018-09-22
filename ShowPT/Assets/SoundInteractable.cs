using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundInteractable : InteractableObject {

    private CtrlAudio ctrlAudio;
    public AudioClip audioClip;

    protected override void Start()
    {
        ctrlAudio = GameObject.FindGameObjectWithTag("CtrlAudio").GetComponent<CtrlAudio>();
        base.Start();
    }

    protected override void executeAction()
    {
        ctrlAudio.playOneSound("UI", audioClip, transform.position, 1.0f, 0f, 128);
    }
}
