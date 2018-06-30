using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlowUp : MonoBehaviour {

	[SerializeField]
	float explosionForce = 10f;

	Rigidbody[] listOfChildren;

	void OnEnable()
	{
		listOfChildren = GetComponentsInChildren<Rigidbody> ();
		foreach (Rigidbody body in listOfChildren) 
		{
			body.AddExplosionForce (explosionForce, transform.position, 100f);
		}
	}
}
