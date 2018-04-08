using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CtrlPause : MonoBehaviour {

	[SerializeField]
	GameUI gameUI;

	public static bool gamePaused = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.P))
		{
			PauseUnPause ();
		}
	}

	void PauseUnPause()
	{
		gamePaused = !gamePaused;
		if (gameUI) 
		{
			gameUI.TogglePauseScreen ();
		}

		if (gamePaused) 
		{
			Time.timeScale = 0;
		} 
		else 
		{
			Time.timeScale = 1;
		}
	}
}
