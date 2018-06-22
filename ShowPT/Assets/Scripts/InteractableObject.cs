using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : MonoBehaviour {

    [Header("Interactable info")]
    public KeyCode keycodeToInteract;
    public string action;
    public string nameObject;

    protected virtual void Start() {
        InteractableObjectsManager.addInteractableObject(name, keycodeToInteract.ToString(), action, nameObject);
        enabled = false;
	}
	
	void Update()
    {
        if (Input.GetKeyDown(keycodeToInteract))
        {
            executeAction();
        }
    }

    protected virtual void init() { }

    protected virtual void executeAction() {}
}
