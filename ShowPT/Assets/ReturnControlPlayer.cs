using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnControlPlayer : MonoBehaviour {
    
	void Start ()
	{
	    CtrlGameState.gameState = CtrlGameState.gameStates.INITIALINTRO;
        GameObject sceneUI = GameObject.FindGameObjectWithTag("SceneUI");
	    sceneUI.GetComponentInChildren<HudController>().gameObject.SetActive(false);
        PlayerMovment.overrideControls = true;
    }

    public void returnToMove()
    {
        CtrlGameState.gameState = CtrlGameState.gameStates.ACTIVE;
        PlayerMovment.overrideControls = false;
    }
}
