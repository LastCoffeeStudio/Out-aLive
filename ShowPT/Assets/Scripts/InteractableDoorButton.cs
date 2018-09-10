using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableDoorButton : InteractableObject {

	[SerializeField]
	bool activeButton = true;

	private CtrlAudio ctrlAudio;
	[SerializeField]
	AudioClip rejectSound;

	protected override void Start() 
	{
		ctrlAudio = GameObject.FindGameObjectWithTag("CtrlAudio").GetComponent<CtrlAudio>();
		base.Start ();
	}

    protected override void executeAction()
    {
		if (activeButton == true) 
		{
			transform.parent.GetComponent<GateTerminal> ().Activate ();
			setState (InteractableObjectState.DISABLE);
		} 
		else 
		{
			ctrlAudio.playOneSound("Scene", rejectSound, transform.position, 0.5f, 1f, 128);
		}
    }
}
