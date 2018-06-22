using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragToInteractableObjectAndRename : InteractableObject
{
    protected override void executeAction()
    {
        Debug.Log("Executing custom action");
    }
}

