using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDoorTrigger : MonoBehaviour {

	[SerializeField]
	BossDoor bossDoor;

	[SerializeField]
	bool opensDoor = true;

	void OnTriggerEnter(Collider col)
	{
		if (col.tag == "Player") 
		{
			if (opensDoor == true) 
			{
				bossDoor.OpenSesame ();
			} 
			else 
			{
				bossDoor.CloseSesame ();
			}

			gameObject.SetActive (false);
		}
	}
}
