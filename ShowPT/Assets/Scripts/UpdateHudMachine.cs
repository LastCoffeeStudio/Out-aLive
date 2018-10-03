using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateHudMachine : MonoBehaviour
{
    private ResourceMachineController resourceMachineController;

	// Use this for initialization
	void Start ()
	{
	    resourceMachineController = transform.GetComponentInChildren<ResourceMachineController>();
	}
	
	// Update is called once per frame
	public void updateHUD () {
	    resourceMachineController.updateSelected(0);
    }
}
