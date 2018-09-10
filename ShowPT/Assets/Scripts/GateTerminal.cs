using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateTerminal : MonoBehaviour {

	[SerializeField]
	GameObject gate;
	[SerializeField]
	GameObject deactivatedButton;
	[SerializeField]
	GameObject activatedButton;

	[SerializeField]
	GameObject openedPosition;

	bool gateOpened = false;

	Animator myAnimator;

	[Header("Audio")]
	public AudioClip doorOpenAudio;
	public AudioClip buttonAudio;
	protected CtrlAudio ctrlAudio;
    
	void Start () 
	{
		myAnimator = activatedButton.GetComponent<Animator> ();
		ctrlAudio = GameObject.FindGameObjectWithTag("CtrlAudio").GetComponent<CtrlAudio>();
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
		ctrlAudio.playOneSound("Scene", doorOpenAudio, transform.position, 0.5f, 0f, 150);
		ctrlAudio.playOneSound("Scene", buttonAudio, transform.position, 0.5f, 0f, 150);
		gateOpened = true;
		myAnimator.SetTrigger ("activation");
		activatedButton.GetComponent<Renderer> ().material.SetColor ("_Color", Color.green);
	}

	public void activateButton()
	{
		deactivatedButton.SetActive (false);
		activatedButton.SetActive (true);
	}
}
