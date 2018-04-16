using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : Enemy {

    public bool active = false;

    [SerializeField]
	Projectile projectileToShoot;
	[SerializeField]
	float timeBetweenShots = 0.5f;
	float shootTimer = 0.0f;

    public Transform shotPoint;

	// Update is called once per frame
	void Update () 
	{
		if (active) 
		{
			shootTimer += Time.deltaTime;
			if (shootTimer >= timeBetweenShots) 
			{
				GameObject.Instantiate (projectileToShoot, shotPoint.position, shotPoint.rotation);
				shootTimer = 0.0f;
			}
		}
	}

    public override void getHit()
    {
        --enemyHealth;
        //Execute properly Animation
        base.checkHealth();
    }
}
