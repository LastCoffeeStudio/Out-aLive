using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CtrlAudioTest : MonoBehaviour
{
    // Use this for initialization
    void Start()
    {
        CtrlAudio ctrlAudio = GameObject.FindGameObjectWithTag("CtrlAudio").GetComponent<CtrlAudio>();
        ctrlAudio.SetTrackVolume("Enemies", 10, 5);
    }
}
