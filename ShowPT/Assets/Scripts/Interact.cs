using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Interact : MonoBehaviour {

    public LayerMask interactables;
    public Camera head;

    [SerializeField]
    private Button actualButton;
    
	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        checkInput();
        checkInteractable();
	}

    private void checkInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            actualButton.onClick.Invoke();
        }
    }

    private void checkInteractable()
    {
        RaycastHit info;
        Debug.DrawRay(head.transform.position, head.transform.forward, new Color(255,0,0,255));
        if (Physics.Raycast(head.transform.position, head.transform.forward, out info, 100f, interactables))
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
