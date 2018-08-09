using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartMusicOnPlay : MonoBehaviour
{
    public  AudioClip music;
	void Start () {
	    GameObject.FindGameObjectWithTag("CtrlAudio").GetComponent<CtrlAudio>().playOneSound("Music", music, transform.position, 0.30f, 0.0f, 100);
    }
	
}
