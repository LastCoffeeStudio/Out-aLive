using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Interact : MonoBehaviour {

    public float interactDistance;
    public LayerMask interactables;
    public Camera head;
    
    private Button actualButton;
	
	// Update is called once per frame
	void Update ()
    {
        checkInput();
        checkInteractable();
	}

    private void checkInput()
    {
        if (Input.GetKeyDown(KeyCode.E) && actualButton != null)
        {
            actualButton.onClick.Invoke();
        }
    }

    private void checkInteractable()
    {
        RaycastHit info;
        Debug.DrawRay(head.transform.position, head.transform.forward * 10f, new Color(255,0,0,255));
        if (Physics.Raycast(head.transform.position, head.transform.forward, out info, interactDistance, interactables) && info.transform.tag == "InteractableUI")
        {
            actualButton = info.transform.GetComponent<Button>();
            actualButton.interactable = true;
        }
        else if (actualButton != null)
        {
            actualButton.interactable = false;
            actualButton = null;
        }
        
    }
}
