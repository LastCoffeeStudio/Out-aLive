using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Weapon : MonoBehaviour
{
    public float amount;
    public float maxAmount;
    public float smoothAmount;
    public int damage;
    public Transform shootPoint;
    public Transform cameraPlayer;
    public int maxAmmo = 10;
    public int ammunition;

    private bool firing = false;
    private bool reloading = false;
    private bool aimming = false;
    protected Animator animator;
    private Vector3 initialposition;

    public Vector3 aimPosition;
    public Vector3 originalPosition;
    public float aimSpeed;
    public float weaponRange;
    public ParticleSystem shootEffect;
    public GameObject sparks;

    protected Inventory inventory;
    public Inventory.AMMO_TYPE typeAmmo;
    public Inventory.WEAPON_TYPE type;
    public AudioClip shot;
    public AudioClip reload;
    private CtrlAudio ctrlAudio;

    // Use this for initialization
    private void Start ()
    {
        ammunition = maxAmmo;
        initialposition = transform.localPosition;
        animator = gameObject.GetComponent<Animator>();
        inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();
        ctrlAudio = GameObject.FindGameObjectWithTag("CtrlAudio").GetComponent<CtrlAudio>();
    }
	
	// Update is called once per frame
	private void Update ()
    {
        if (!firing && !reloading && !aimming)
        {
            swagWeaponMovement();
        }

        if (CtrlPause.gamePaused == false)
        {
            checkInputAnimations();
        }

        aimAmmo();
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
        if (!reloading)
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
    }

    private void checkInputAnimations()
    {
        if (Input.GetButton("Fire1") || Input.GetAxis("AxisRT") > 0.5f && animator.GetBool("shooting") == false)
        {
            if (ammunition > 0)
            {
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

    private void checkMouseInput()
    {
        if (!Input.GetButton("Fire1") || Input.GetAxis("AxisRT") > 0.5f || ammunition == 0)
        {
            animator.SetBool("shooting", false);
        }
    }

    public virtual void decreaseAmmo()
    {
        --ammunition;
        inventory.setAmmo(typeAmmo, ammunition);
        ScoreController.weaponUsed(type);
        shotBullet(transform.forward);
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

    protected void shotBullet(Vector3 dir)
    {
        RaycastHit hitInfo;

        if (Physics.Raycast(cameraPlayer.position, dir, out hitInfo, weaponRange))
        {
            if (hitInfo.transform.tag == "Enemy" || hitInfo.transform.tag == "Drone" || hitInfo.transform.tag == "Snitch")
            {
                hitInfo.collider.gameObject.GetComponent<Enemy>().getHit(damage);
                ScoreController.weaponHit(type);
                GameObject spark = Instantiate(sparks, hitInfo.point, Quaternion.Euler(0f, 0f, 0f));
                spark.transform.up = hitInfo.normal;
            }
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
