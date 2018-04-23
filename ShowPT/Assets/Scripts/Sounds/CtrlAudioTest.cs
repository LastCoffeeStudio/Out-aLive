using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CtrlAudioTest : MonoBehaviour
{
    public AudioClip clip;
    private CtrlAudio ctrlAudio;
    // Use this for initialization
    void Start()
    {
        ctrlAudio = GameObject.FindGameObjectWithTag("CtrlAudio").GetComponent<CtrlAudio>();
        ctrlAudio.setTrackVolume("Enemies", 10, 5);
        InvokeRepeating("playTest",0.5f, 0.1f);
    }

    void playTest()
    {
        ctrlAudio.playOneSound("Player", clip, transform.position, 0.5f, 0.0f, 128);
    }
}
