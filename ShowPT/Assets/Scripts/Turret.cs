using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : Enemy
{
	// Update is called once per frame
	void Update() 
	{
        shoot();
    }

    public override void getHit(int damage)
    {
        enemyHealth -= damage;
        //Execute properly Animation
        base.checkHealth();
    }
}
