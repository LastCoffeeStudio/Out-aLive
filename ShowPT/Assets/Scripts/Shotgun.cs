using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

//TODO: Make Inheritance with Weapon, shared with all weapon scripts

public class Shotgun : Weapon
{
    [Header("Shotgun Settings")]
    [SerializeField]
    GameObject projectileToShoot;

    // Update is called once per frame
    protected override void Update()
    {
        if (CtrlGameState.gameState == CtrlGameState.gameStates.ACTIVE)
        {
			if (PlayerMovment.overrideControls == false) 
			{
				checkInputAnimations ();
				swagWeaponMovement ();
			}
        }
    }

    protected override void shotBullet(Ray ray)
    {
        GameObject projectile = Instantiate(projectileToShoot, shootPoint.position, Quaternion.LookRotation(Vector3.Normalize((ray.origin + ray.direction * weaponRange) - shootPoint.position)), shootPoint);
        projectile.transform.Rotate(-90f, 0f, 0f);
    }

    private void shoot()
    {
        shootEffect.Play();
        ctrlAudio.playOneSound("Weaponds", shotAudio, transform.position, 1.0f, 0f, 150);
        shootEffect.Play();
    }
}
