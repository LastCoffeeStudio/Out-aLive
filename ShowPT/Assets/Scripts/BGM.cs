using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGM : MonoBehaviour {

	//This class serves mostly as an interface between CtrlAudio.cs and other classes in order to modify BGM stuff.

	CtrlAudio ctrlAudio;
	ulong idbackgroundMusic;
	[SerializeField]
	float normalVolume = 0.6f;

	float trackVolume;

	[SerializeField]
	AudioClip[] musicList;

	bool isFading = false;
	[SerializeField]
	float fadeSpeed = 1f;

	// Use this for initialization
	void Start () 
	{
		ctrlAudio = GameObject.FindGameObjectWithTag("CtrlAudio").GetComponent<CtrlAudio>();
		trackVolume = ctrlAudio.getTrackVolume("Music");
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (isFading == true) 
		{
			ctrlAudio.setTrackVolume("Music", ctrlAudio.getTrackVolume("Music") - fadeSpeed * Time.deltaTime);
			if (ctrlAudio.getTrackVolume("Music") <= -80) 
			{
				isFading = false;
			}
		}
	}

	public void playMeSomething(int musicId)
	{
		stopTheMusic ();
		ctrlAudio.setTrackVolume ("Music", trackVolume);
		ctrlAudio.playOneSound("Music", musicList[musicId], Vector3.zero, 0.6f, 0f, 1, true);
    }

	public void stopTheMusic()
	{
		ctrlAudio.stopSound (idbackgroundMusic);
		isFading = false;
	}

	public void fadeOut()
	{
		isFading = true;
	}
}
