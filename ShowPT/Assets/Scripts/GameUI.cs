using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour {

	[SerializeField]
	GameObject pauseScreen;

	Main mainManager;

    public GameObject resourceMachineMenu;

	// Use this for initialization
	void Start ()
    {
        resourceMachineMenu.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

	public void TogglePauseScreen()
	{
		if (CtrlPause.gamePaused == false) 
		{
			pauseScreen.SetActive(false);
            setCursorScreen(false);
		} 
		else 
		{
			pauseScreen.SetActive(true);
            setCursorScreen(true);
        }
	}

	public void GoBackToMain()
	{
		
	}

    public void enableResourceMachineUI()
    {
        resourceMachineMenu.SetActive(true);
        setCursorScreen(true);
    }

    public void disableResourceMachineUI()
    {
        resourceMachineMenu.SetActive(false);
        setCursorScreen(false);
    }

    private void setCursorScreen(bool active)
    {
        if (active)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Time.timeScale = 0;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            Time.timeScale = 1;
        }
    }
}
