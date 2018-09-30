using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivationTrigger : MonoBehaviour {

	[SerializeField]
	GameObject objectToActivate;
    bool activated;

    private void Start()
    {
        activated = false;
    }

    void OnTriggerEnter(Collider col)
	{
		if (col.tag == "Player" && !activated) 
		{
            activated = true;
            objectToActivate.SetActive (true);
			gameObject.SetActive (false);
		}
	}

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player" && !activated)
        {
            activated = true;
            objectToActivate.SetActive(true);
            gameObject.SetActive(false);
        }
    }
}
