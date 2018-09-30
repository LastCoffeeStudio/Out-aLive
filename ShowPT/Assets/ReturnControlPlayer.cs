using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnControlPlayer : MonoBehaviour
{
    private GameObject HUD;

	void Start ()
	{
	    CtrlGameState.gameState = CtrlGameState.gameStates.INITIALINTRO;
        GameObject sceneUI = GameObject.FindGameObjectWithTag("SceneUI");
	    HUD = sceneUI.GetComponentInChildren<HudController>().gameObject;
        HUD.SetActive(false);
        PlayerMovment.overrideControls = true;
    }

    public void returnToMove()
    {
        HUD.SetActive(true);
        CtrlGameState.gameState = CtrlGameState.gameStates.ACTIVE;
        PlayerMovment.overrideControls = false;
    }
}
