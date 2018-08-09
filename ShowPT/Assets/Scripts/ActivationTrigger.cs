using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivationTrigger : MonoBehaviour {

	[SerializeField]
	GameObject objectToActivate;

	void OnTriggerEnter(Collider col)
	{
		if (col.tag == "Player") 
		{
			objectToActivate.SetActive (true);
			gameObject.SetActive (false);
		}
	}
}
