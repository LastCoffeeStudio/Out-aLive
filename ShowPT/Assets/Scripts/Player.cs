using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour 
{

	//float speed = 7f;
	public float noiseValue = 0f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		/*Vector3 inputDirection = new Vector3 (Input.GetAxisRaw ("Horizontal"), 0, Input.GetAxisRaw ("Vertical")).normalized;
		float inputMagnitude = inputDirection.magnitude;

		float targetAngle = Mathf.Atan2 (inputDirection.x, inputDirection.z) * Mathf.Rad2Deg;
		transform.eulerAngles = Vector3.up * targetAngle;

		transform.Translate (transform.forward * speed * inputMagnitude * Time.deltaTime, Space.World);*/

		noiseValue = 0f;
		if (Input.GetKeyDown (KeyCode.Z)) 
		{
			noiseValue = 20f;
		}

		if (Input.GetKeyDown (KeyCode.Escape)) 
		{
			Application.Quit();
		}
	}
}
