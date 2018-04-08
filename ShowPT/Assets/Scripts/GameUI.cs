using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour {

	[SerializeField]
	Image pauseScreen;

	[SerializeField]
	Slider healthBar;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void TogglePauseScreen()
	{
		if (CtrlPause.gamePaused == false) 
		{
			pauseScreen.enabled = false;
		} 
		else 
		{
			pauseScreen.enabled = true;
		}
	}

	public void ChangeHealthBar(int value)
	{
		print ("Value to apply to lifebar " + value);
		healthBar.value = value;
		print ("Value in lifebar now " + healthBar.value);
	}
}
