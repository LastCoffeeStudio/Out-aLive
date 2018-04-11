using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour {

	[SerializeField]
	GameObject pauseScreen;

	Main mainManager;

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
			pauseScreen.SetActive(false);
		} 
		else 
		{
			pauseScreen.SetActive(true);
		}
	}

	public void ChangeHealthBar(int value)
	{
		healthBar.value = value;
	}

	public void GoBackToMain()
	{
		
	}
}
