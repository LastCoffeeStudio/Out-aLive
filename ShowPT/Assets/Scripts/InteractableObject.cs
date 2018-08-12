using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : MonoBehaviour {

    [Header("Interactable info")]
    public KeyCode keycodeToInteract;
    public string action;
    public string nameObject;

	public enum ObjectMaster
	{
		SAME_OBJECT,
		PARENT
	}
		
	public ObjectMaster masterObject = ObjectMaster.SAME_OBJECT;

    public enum InteractableObjectState
    {
        PREPARED,
        USING,
        DISABLE
    }

    InteractableObjectState objectState;

    protected virtual void Start() {
        InteractableObjectsManager.addInteractableObject(name, keycodeToInteract.ToString(), action, nameObject);
        objectState = InteractableObjectState.PREPARED;
        enabled = false;
    }

    protected virtual void Update()
    {
        if (Input.GetKeyDown(keycodeToInteract))
        {
            executeAction();
        }
    }

    protected virtual void init() { }

    protected virtual void executeAction() {}

    public void setState(InteractableObjectState interactableObjectState)
    {
        objectState = interactableObjectState;
    }

    public InteractableObjectState getState()
    {
        return objectState;
    }
}
