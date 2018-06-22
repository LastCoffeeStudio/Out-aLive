using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : MonoBehaviour {

    [Header("Interactable info")]
    public KeyCode keycodeToInteract;
    public string action;
    public string nameObject;

    private bool active;
    
	void Start() {
        active = false;
        InteractableObjectsManager.addInteractableObject(name, keycodeToInteract.ToString(), action, nameObject);
	}
	
	void Update()
    {
        if (active && Input.GetKeyDown(keycodeToInteract))
        {
            executeAction();
        }
    }

    protected virtual void executeAction() {}

    public void setActive(bool active)
    {
        this.active = active;
    }
}
