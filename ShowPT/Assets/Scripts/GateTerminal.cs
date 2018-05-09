using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateTerminal : MonoBehaviour {

	[SerializeField]
	GameObject gate;

	[SerializeField]
	GameObject openedPosition;

	bool gateOpened = false;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		if (gateOpened) {
			gate.transform.position = Vector3.Lerp (gate.transform.position, openedPosition.transform.position, Time.deltaTime);
		}
		if (Input.GetKeyDown (KeyCode.E)) {
			
		}
	}

	public void Activate()
	{
		gateOpened = true;
		gameObject.GetComponent<Renderer> ().material.SetColor ("_Color", Color.green);
	}
}
