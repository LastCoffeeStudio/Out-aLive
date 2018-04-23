using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartMusicOnPlay : MonoBehaviour {

	// Use this for initialization
    public  AudioClip music;
	void Start () {
	    gameObject.GetComponent<CtrlAudio>().playOneSound("Music", music, transform.position, 1.0f, 0.0f, 100);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
