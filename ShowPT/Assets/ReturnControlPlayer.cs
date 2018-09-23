using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnControlPlayer : MonoBehaviour {
    
	void Start ()
    {
        PlayerMovment.overrideControls = true;
    }

    public void returnToMove()
    {
        PlayerMovment.overrideControls = false;
    }
}
