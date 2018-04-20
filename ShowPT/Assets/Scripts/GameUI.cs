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

    public void enableResourceMachineUI()
    {
        resourceMachineMenu.SetActive(true);
    }

    public void disableResourceMachineUI()
    {
        resourceMachineMenu.SetActive(false);
    }
}
