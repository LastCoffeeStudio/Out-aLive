using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CtrlAudioButtonsUI : MonoBehaviour
{
    public AudioClip highlightedAudioClip;
    public AudioClip selectedAudioClip;
    public AudioClip canceledAudioClip;
    private CtrlAudio ctrlAudio;

	// Use this for initialization
	void Start ()
	{
	    ctrlAudio = GameObject.FindGameObjectWithTag("CtrlAudio").GetComponent<CtrlAudio>();

	}

    public void playHiglight()
    {
        ctrlAudio.playOneSound("UI", highlightedAudioClip, Vector3.back, 0.6f, 0f, 2);
    }
    public void playSelected()
    {
        ctrlAudio.playOneSound("UI", selectedAudioClip, Vector3.back, 0.6f, 0f, 2);
    }

    public void playCancel()
    {
        ctrlAudio.playOneSound("UI", canceledAudioClip, Vector3.back, 0.6f, 0f, 2);
    }
}
