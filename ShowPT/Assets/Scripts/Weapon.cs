using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Crosshair))]
[RequireComponent(typeof(Recoil))]

public class Weapon : MonoBehaviour
{
    public LayerMask maskBullets;
    public float amount;
    public float maxAmount;
    public float smoothAmount;
    public int damage;
    public Transform shootPoint;
    public Transform cameraPlayer;
    public int maxAmmo = 10;
    public int ammunition;

    protected bool firing = false;
    private bool reloading = false;
    private bool aimming = false;
    protected Animator animator;
    private Vector3 initialposition;

    public Vector3 aimPosition;
    public Vector3 originalPosition;
    public float aimSpeed;
    public float weaponRange;
    public ParticleSystem shootEffect;
    public GameObject trailEffect;
    public GameObject sparks;

    protected Inventory inventory;
    public Inventory.AMMO_TYPE typeAmmo;
    public Inventory.WEAPON_TYPE type;
    public AudioClip shot;
    public AudioClip reload;
    private CtrlAudio ctrlAudio;

    [Header("Crosshair Settings")]
    public float shotSpreadFactor;
    protected Crosshair crosshair;

    protected Recoil recoil;

    public GameObject redSphere;

    // Use this for initialization
    private void Start ()
    {
        ammunition = maxAmmo;
        initialposition = transform.localPosition;
        animator = gameObject.GetComponent<Animator>();
        inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();
        ctrlAudio = GameObject.FindGameObjectWithTag("CtrlAudio").GetComponent<CtrlAudio>();
        crosshair = gameObject.GetComponent<Crosshair>();
        recoil = gameObject.GetComponent<Recoil>();
    }
	
	// Update is called once per frame
	protected virtual void Update ()
    {
        if (!firing && !reloading)
        {
            aimAmmo();

            if (!aimming)
            {
                swagWeaponMovement();
            }
        }

        if (CtrlPause.gamePaused == false)
        {
            checkInputAnimations();
        }
    }

    private void swagWeaponMovement()
    {
        float movX = -Input.GetAxis("Mouse X") * amount;
        float movY = -Input.GetAxis("Mouse Y") * amount;
        movX = Mathf.Clamp(movX, -maxAmount, maxAmount);
        movY = Mathf.Clamp(movY, -maxAmount, maxAmount);

        Vector3 finalPosition = new Vector3(movX, movY, 0);
        transform.localPosition = Vector3.Lerp(transform.localPosition, finalPosition + initialposition, Time.deltaTime * smoothAmount);
    }

    private void aimAmmo()
    {
        if (Input.GetButton("Fire2") || Input.GetAxis("AxisLT") > 0.5f)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, aimPosition, Time.deltaTime * aimSpeed);
            aimming = true;
        }
        else
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, originalPosition, Time.deltaTime * aimSpeed);
            aimming = false;
        }
    }

    private void checkInputAnimations()
    {
        if ((Input.GetButtonDown("Fire1") || Input.GetAxis("AxisRT") > 0.5f) && animator.GetBool("shooting") == false)
        {
            if (ammunition > 0)
            {
                firing = true;
                animator.SetBool("shooting", true);
                if (animator.GetBool("reloading"))
                {
                    reloading = false;
                    animator.SetBool("reloading", false);
                }
            }
        }
        if ((Input.GetKey(KeyCode.R) || Input.GetButton("ButtonX")) && ammunition < maxAmmo && animator.GetBool("reloading") == false)
        {
            if (inventory.getAmmo(typeAmmo) > 0)
            {
                reloading = true;
                animator.SetBool("reloading", true);
            }

            if (type == Inventory.WEAPON_TYPE.SHOTGUN && ammunition == maxAmmo - 1)
            {
                animator.SetBool("lastBullet", true);
            }
        }
    }

    protected virtual void checkMouseInput()
    {
        if ((!Input.GetButton("Fire1") && Input.GetAxis("AxisRT") < 0.5f) || ammunition == 0)
        {
            animator.SetBool("shooting", false);
            firing = false;
        }
    }

    public virtual void decreaseAmmo()
    {
        --ammunition;
        inventory.setAmmo(typeAmmo, ammunition);
        ScoreController.weaponUsed(type);
        shotBullet(crosshair.getRayCrosshairArea());
        if (!crosshair.isFixed)
        {
            crosshair.increaseSpread(shotSpreadFactor);
        }
        recoil.addRecoil();
    }

    public virtual void increaseAmmo()
    {
        int ammoTemp = inventory.getAmmo(typeAmmo);

        //Infinite ammo for gun
        if (type != Inventory.WEAPON_TYPE.GUN)
        {
            inventory.decreaseAmmo(typeAmmo, maxAmmo - ammunition);
        }

        if ((maxAmmo - ammunition) > ammoTemp)
        {
            ammunition += ammoTemp;
        }
        else
        {
            ammunition = maxAmmo;
        }

        inventory.setAmmo(typeAmmo, ammunition);
    }

    protected void shotBullet(Ray ray)
    {
        RaycastHit hitInfo;
        
        if (Physics.Raycast(ray, out hitInfo, weaponRange, maskBullets))
        {
            Debug.Log(hitInfo.transform.tag);
            if (hitInfo.transform.tag == "Enemy" || hitInfo.transform.tag == "Drone" || hitInfo.transform.tag == "Snitch")
            {
                hitInfo.collider.gameObject.GetComponent<Enemy>().getHit(damage);
                ScoreController.weaponHit(type);
                GameObject spark = Instantiate(sparks, hitInfo.point, Quaternion.Euler(0f, 0f, 0f));
                spark.transform.up = hitInfo.normal;
            }

            //Trail effect
            if (trailEffect != null)
            {
                trailEffect.transform.LookAt(hitInfo.point);
                trailEffect.GetComponent<ParticleSystem>().Play();
            }
            Instantiate(redSphere, hitInfo.point, hitInfo.transform.rotation);
        }
    }

    private void endReload()
    {
        reloading = false;
        animator.SetBool("reloading", false);

        if (type == Inventory.WEAPON_TYPE.SHOTGUN)
        {
            animator.SetBool("lastBullet", false);
        }
    }
    private void shoot()
    {
        ctrlAudio.playOneSound("Player", shot, transform.position, 1.0f, 0f, 150);
        shootEffect.Play();
    }
}
