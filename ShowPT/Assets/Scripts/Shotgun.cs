using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

//TODO: Make Inheritance with Weapon, shared with all weapon scripts

public class Shotgun : Weapon
{
    public int shotgunBullets;
    public float bulletDispersion;

    public override void decreaseAmmo()
    {
        --ammunition;
        inventory.setAmmo(typeAmmo, ammunition);
        ScoreController.weaponUsed(type);
        for (uint i = 0; i < shotgunBullets; ++i)
        {
            Vector3 dir = transform.forward + (Vector3.Cross(cameraPlayer.forward, Random.insideUnitSphere) * Random.Range(0f, bulletDispersion));
            shotBullet(dir);
        }
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
