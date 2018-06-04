using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : Weapon
{
    [Header("Gun Settings")]
    [SerializeField]
    Projectile projectileToShoot;

    protected override void shotBullet(Ray ray)
    {
        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo, weaponRange, maskBullets))
        {
            Instantiate(projectileToShoot, shootPoint.position, Quaternion.LookRotation(Vector3.Normalize(hitInfo.point - shootPoint.position)));
        }
        else
        {
            Instantiate(projectileToShoot, shootPoint.position, Quaternion.LookRotation(Vector3.Normalize((ray.origin + ray.direction * weaponRange) - shootPoint.position)));
        }
    }
}
