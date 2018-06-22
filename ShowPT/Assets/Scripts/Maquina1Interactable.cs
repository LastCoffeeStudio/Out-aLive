using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Maquina1Interactable : InteractableObject
{
    protected override void executeAction()
    {
        Debug.Log("Executing custom action");
    }
}

