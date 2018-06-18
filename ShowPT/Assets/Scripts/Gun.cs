using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : Weapon
{
    [Header("Gun Settings")]
    [SerializeField]
    Projectile projectileToShoot;
    public float chargeWhenShoot;
    public float maxCharge;
    [SerializeField]
    private float actualCharge;
    public float dischargeSpeed;
    public float dischargeSpeedOverflow;

    protected override void Update()
    {
        if (!firing && !reloading)
        {
            aimAmmo();
            //swagWeaponMovement();
        }

        if (CtrlPause.gamePaused == false)
        {
            if (!playerState.buying)
            {
                checkInputAnimations();
            }
            updateCharge();
        }
    }

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

    protected override void checkInputAnimations()
    {
        if ((Input.GetButtonDown("Fire1") || Input.GetAxis("AxisRT") > 0.5f) && animator.GetBool("shooting") == false && animator.GetBool("reloading") == false)
        {
            if (actualCharge < maxCharge)
            {
                firing = true;
                animator.SetBool("shooting", true);
            }
        }
        if (actualCharge >= maxCharge && animator.GetBool("reloading") == false)
        {
            reloading = true;
            animator.SetBool("reloading", true);
        }
    }

    public override void decreaseAmmo()
    {
        actualCharge += chargeWhenShoot;
        Mathf.Clamp(actualCharge, 0, maxCharge);
        ScoreController.weaponUsed(type);
        shotBullet(crosshair.getRayCrosshairArea());
        if (!crosshair.isFixed)
        {
            crosshair.increaseSpread(shotSpreadFactor);
        }
        recoil.addRecoil();
    }

    private void updateCharge()
    {
        float discharge = dischargeSpeed;
        if (animator.GetBool("reloading") == true)
        {
            discharge = dischargeSpeedOverflow;
        }

        actualCharge -= discharge * Time.deltaTime;
        if (actualCharge < 0f)
        {
            actualCharge = 0f;
        }
        inventory.updateStats(actualCharge, maxCharge);
    }

    protected override void checkMouseInput()
    {
        if ((!Input.GetButton("Fire1") && Input.GetAxis("AxisRT") < 0.5f) || animator.GetBool("reloading") == true)
        {
            animator.SetBool("shooting", false);
            firing = false;
        }
    }
}
