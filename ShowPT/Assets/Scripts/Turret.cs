using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour {

	[SerializeField]
	Projectile projectileToShoot;

	[SerializeField]
	float timeBetweenShots = 0.5f;
	float shootTimer = 0.0f;

	public bool active = false;

	// Update is called once per frame
	void Update () 
	{
		if (active) 
		{
			shootTimer += Time.deltaTime;
			if (shootTimer >= timeBetweenShots) 
			{
				GameObject.Instantiate (projectileToShoot, gameObject.transform.position, gameObject.transform.rotation);
				shootTimer = 0.0f;
			}
		}
	}
}
