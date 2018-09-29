using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableBossDoor : InteractableObject {

	protected override void executeAction()
	{
		GetComponent<BossDoor>().OpenSesame();

		setState(InteractableObjectState.DISABLE);
	}
}
