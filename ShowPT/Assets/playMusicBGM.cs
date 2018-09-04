using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playMusicBGM : MonoBehaviour
{
	// Use this for initialization
	void Start ()
	{
	    GameObject.FindGameObjectWithTag("CtrlAudio").GetComponent<BGM>().playMeSomething(1);

	}
	
}
