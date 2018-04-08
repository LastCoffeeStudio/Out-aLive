using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyWinScript : MonoBehaviour
{
    public  GameObject ctrlGame;
	// Use this for initialization
	void Start () {
	   
	}

    void OnCollisionEnter(Collision collision)
    {
        ctrlGame.GetComponent<CtrlGameState>().setGameState(CtrlGameState.gameStates.WIN);
    }
}
