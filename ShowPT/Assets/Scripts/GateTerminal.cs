using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateTerminal : MonoBehaviour {

	[SerializeField]
	GameObject gate;

	[SerializeField]
	GameObject openedPosition;

	bool gateOpened = false;

	Animator myAnimator;
    
	void Start () 
	{
		myAnimator = GetComponent<Animator> ();
	}
	
	void Update () 
	{
		if (gateOpened) 
		{
			gate.transform.position = Vector3.Lerp (gate.transform.position, openedPosition.transform.position, Time.deltaTime);
		}
	}

	public void Activate()
	{
		gateOpened = true;
		myAnimator.SetTrigger ("activation");
		gameObject.GetComponent<Renderer> ().material.SetColor ("_Color", Color.green);
	}
}
