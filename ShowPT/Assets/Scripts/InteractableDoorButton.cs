using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableDoorButton : InteractableObject {

    protected override void executeAction()
    {
        GetComponent<GateTerminal>().Activate();
		setState(InteractableObjectState.DISABLE);
    }
}
