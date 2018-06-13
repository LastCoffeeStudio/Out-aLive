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

    protected override void shotBullet(Ray ray)
    {
        GameObject projectile = Instantiate(projectileToShoot, shootPoint.position, Quaternion.LookRotation(Vector3.Normalize((ray.origin + ray.direction * weaponRange) - shootPoint.position)));
        projectile.transform.Rotate(0f, 180f, 0f);
    }

    public override void increaseAmmo()
    {
        ++ammunition;
        inventory.decreaseAmmo(typeAmmo, 1);
        inventory.setAmmo(typeAmmo, (int)ammunition);
    }

    private void reloadAndCheck()
    {
        if (ammunition == maxAmmo - 1)
        {
            animator.SetBool("lastBullet", true);
        }
    }
    private void shoot()
    {
        shootEffect.Play();
    }
}
