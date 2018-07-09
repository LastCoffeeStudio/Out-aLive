using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaScriptTrigger : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            GameObject thePlayer = other.gameObject;
            PlayerHealth playerScript = thePlayer.GetComponent<PlayerHealth>();
            playerScript.ChangeHealth(-playerScript.GetHealth());
            Time.timeScale = 0;
        }
    }
}
