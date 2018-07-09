using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserCanon : Weapon {

    [Header("Canon Settings")]
    public float overheatMaxTime;
    public float minBulletSize;
    public float maxBulletSize;
    [SerializeField]
    Projectile projectileToShoot;

    private float overheatTime;
    private Vector3 minBulletScale;
    private Vector3 maxBulletScale;

    protected override void Start()
    {
        base.Start();
        minBulletScale = new Vector3(minBulletSize, minBulletSize, minBulletSize);
        maxBulletScale = new Vector3(maxBulletSize, maxBulletSize, maxBulletSize);
    }

    // Update is called once per frame
    protected override void Update ()
    {
        if (firing)
        {
            overheatTime += Time.deltaTime;
            checkMouseInput();
        }

        if (CtrlGameState.gameState == CtrlGameState.gameStates.ACTIVE)
        {
            checkInputAnimations();
        }
    }

    protected override void checkMouseInput()
    {
        if (overheatTime >= overheatMaxTime || (!Input.GetButton("Fire1") && Input.GetAxis("AxisRT") < 0.5f) || ammunition == 0)
        {
            animator.SetBool("shooting", false);
            firing = false;
        }
    }

    protected override void shotBullet(Ray ray)
    {
        RaycastHit hitInfo;
        Projectile projectile;

        if (Physics.Raycast(ray, out hitInfo, weaponRange, maskBullets))
        {
            projectile = Instantiate(projectileToShoot, shootPoint.position, Quaternion.LookRotation(Vector3.Normalize(hitInfo.point - shootPoint.position)));
        }
        else
        {
            projectile = Instantiate(projectileToShoot, shootPoint.position, Quaternion.LookRotation(Vector3.Normalize((ray.origin + ray.direction * weaponRange) - shootPoint.position)));
        }
        projectile.transform.localScale = Vector3.Lerp(minBulletScale, maxBulletScale, overheatTime / overheatMaxTime);
        overheatTime = 0f;
    }

    protected override void shoot()
    {
        ctrlAudio.playOneSound("CannonShot", shot, transform.position, 1.0f, 0f, 150);
        shootEffect.Play();
    }
}
