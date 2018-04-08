using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

	[SerializeField]
	float speed = 5.0f;
    float timeLife = 10f;

	// Update is called once per frame
	void Update () {
		transform.Translate (Vector3.forward * speed * Time.deltaTime);

        if (timeLife > 0f)
            timeLife -= Time.deltaTime;
        else
            Destroy(gameObject);
	}
}
