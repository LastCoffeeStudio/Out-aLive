using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractuableLift : InteractableObject
{
    protected override void executeAction()
    {
        GetComponent<LiftRoomBehivor>().openLift();
        setState(InteractableObjectState.DISABLE);
    }
}
