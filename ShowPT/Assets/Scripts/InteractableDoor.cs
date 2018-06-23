using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableDoor : InteractableObject
{
    private CtrlAudio ctrlAudio;
    public AudioCollection lockedDoor;

    private ulong idClip = 0;

    protected override void Start()
    {
        ctrlAudio = GameObject.FindGameObjectWithTag("CtrlAudio").GetComponent<CtrlAudio>();
        base.Start();
    }

    protected override void executeAction()
    {
        ctrlAudio.stopSound(idClip);
        idClip = ctrlAudio.playOneSound(lockedDoor.audioGroup, lockedDoor[0], transform.position, lockedDoor.volume, lockedDoor.spatialBlend, lockedDoor.priority);
    }
}