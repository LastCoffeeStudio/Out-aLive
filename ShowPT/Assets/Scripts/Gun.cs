using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : Weapon
{
    [Header("Gun Settings")]
    [SerializeField]
    GameObject projectileToShoot;
    public float chargeWhenShoot;
    public float maxCharge;
    [SerializeField]
    private float actualCharge;
    public float dischargeSpeed;
    public float dischargeSpeedOverflow;

    protected override void Update()
    {
        base.Update();
        if (CtrlGameState.gameState == CtrlGameState.gameStates.ACTIVE)
        {
            if (PlayerMovment.overrideControls == false)
            {
                checkInputAnimations();
                checkMouseInput();
                updateCharge();
                aimAmmo();
                if (!animator.GetBool("aiming"))
                {
                    swagWeaponMovement();
                }
            }
        }
    }

    protected override void shotBullet(Ray ray)
    {
        RaycastHit hitInfo;
        GameObject projectile;
        if (Physics.Raycast(ray, out hitInfo, weaponRange, maskBullets))
        {
            projectile = Instantiate(projectileToShoot, shootPoint.position, Quaternion.LookRotation(Vector3.Normalize(hitInfo.point - shootPoint.position)));
        }
        else
        {
            projectile = Instantiate(projectileToShoot, shootPoint.position, Quaternion.LookRotation(Vector3.Normalize((ray.origin + ray.direction * weaponRange) - shootPoint.position)));
        }
        ctrlAudio.playOneSound("Weaponds", shotAudio, transform.position, 0.5f, 0.0f, 128);
        pools.activeProjectiles.Add(projectile);
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
            animator.SetBool("aiming", false);
            reloading = true;
            animator.SetBool("reloading", true);
            ctrlAudio.playOneSound("Weaponds", reloadAudio, transform.position, 0.9f, 0.0f, 128);
        }
    }

    public override void decreaseAmmo()
    {
        Ray ray = crosshair.getRayCrosshairArea();
        actualCharge += chargeWhenShoot;
        Mathf.Clamp(actualCharge, 0, maxCharge);
        ScoreController.weaponUsed(type);
        shotBullet(ray);
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