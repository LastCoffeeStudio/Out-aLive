using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDeath : MonoBehaviour {

    public float deathTime;

	// Use this for initialization
	void Start () {
        deathTime = 4f;
	}
	
	// Update is called once per frame
	void Update () {
        deathTime -= Time.deltaTime;
        if (deathTime < 0f)
            Destroy(this.gameObject);
	}
}
