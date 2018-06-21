using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlowUp : MonoBehaviour {

	[SerializeField]
	float explosionForce = 10f;

	// Use this for initialization
	void Start () {
		Rigidbody[] listOfChildren = GetComponentsInChildren<Rigidbody> ();
		foreach (Rigidbody body in listOfChildren) 
		{
			body.AddExplosionForce (explosionForce, transform.position, 100f);
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
