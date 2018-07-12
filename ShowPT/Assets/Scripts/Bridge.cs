using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bridge : MonoBehaviour {

	[SerializeField]
	GameObject bridge;

	[SerializeField]
	GameObject finalPosition;

	void Start () {}

	void Update () 
	{
		bridge.transform.position = Vector3.Lerp (bridge.transform.position, finalPosition.transform.position, Time.deltaTime);
	}
}
