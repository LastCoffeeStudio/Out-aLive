using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CtrlPause : MonoBehaviour 
{

	[SerializeField]
	GameUI gameUI;

	public static bool gamePaused = false;

	// Use this for initialization
	void Start () 
	{
		Cursor.lockState = CursorLockMode.Locked;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (Input.GetKeyDown(KeyCode.P))
		{
			PauseUnPause ();
		}
	}

	public void PauseUnPause()
	{
		gamePaused = !gamePaused;
		if (gameUI) 
		{
			gameUI.TogglePauseScreen();
		}
	}

	public void ReturnToTitle()
	{
		SceneManager.LoadScene ("Menu Test");
	}
}
