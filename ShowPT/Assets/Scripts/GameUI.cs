using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour {

    private const int MAX_GREEN_LIFE = 60;
    private const int MAX_YELLOW_LIFE = 20;

    private Color32 YELLOW = new Color32(247, 221, 85, 255);
    private Color32 RED = new Color32(255, 0, 85, 255);

    [SerializeField]
	GameObject pauseScreen;

	Main mainManager;

	[SerializeField]
	Slider healthBar;

    public Text valueHealth;
    public GameObject fillHealth;
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
        valueHealth.text = value.ToString();

        if(value < MAX_YELLOW_LIFE)
        {
            fillHealth.GetComponent<Image>().color = RED;
        }
        else if(value < MAX_GREEN_LIFE)
        {
            fillHealth.GetComponent<Image>().color = YELLOW;
        }
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
