using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Crosshair))]
[RequireComponent(typeof(Recoil))]

public class Weapon : MonoBehaviour
{
    public LayerMask maskBullets;
    public int damage;
    public Transform shootPoint;
    public Transform cameraPlayer;
    public int maxAmmo = 10;
    public int ammunition;

    protected bool firing;
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
    protected PlayerMovment playerState;
    public Inventory.AMMO_TYPE typeAmmo;
    public Inventory.WEAPON_TYPE type;

	[Header("Audio")]
    public AudioClip shotAudio;
    public AudioClip reloadAudio;
	public AudioClip noBulletsAudio;
    protected CtrlAudio ctrlAudio;
    protected ObjectPools pools;

    [Header("Swag Weapon Settings")]
    public float amount;
    public float maxAmount;
    public float smoothAmount;

    [Header("Crosshair Settings")]
    public float shotSpreadFactor;
    public Crosshair crosshair;

    protected Recoil recoil;

    // Use this for initialization
    protected virtual void Start()
    {
        initialposition = transform.localPosition;
        animator = gameObject.GetComponent<Animator>();
        inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();
        playerState = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovment>();
        ctrlAudio = GameObject.FindGameObjectWithTag("CtrlAudio").GetComponent<CtrlAudio>();
        crosshair = gameObject.GetComponent<Crosshair>();
        recoil = gameObject.GetComponent<Recoil>();
        pools = GameObject.FindGameObjectWithTag("CtrlGame").GetComponent<ObjectPools>();

        aimPosition = new Vector3(-0.204f, -1.07f, 0);
        aimSpeed = 10;
        firing = false;
    }

    // Update is called once per frame
    protected virtual void Update() {}

    protected void swagWeaponMovement()
    {
        float movX = -Input.GetAxis("Mouse X") * amount;
        float movY = -Input.GetAxis("Mouse Y") * amount;
        movX = Mathf.Clamp(movX, -maxAmount, maxAmount);
        movY = Mathf.Clamp(movY, -maxAmount, maxAmount);

        Vector3 finalPosition = new Vector3(movX, movY, 0);
        transform.localPosition = Vector3.Lerp(transform.localPosition, finalPosition + initialposition, Time.deltaTime * smoothAmount);
    }

    protected void aimAmmo()
    {
        if ((Input.GetButton("Fire2") || Input.GetAxis("AxisLT") > 0.5f) && !animator.GetBool("reloading"))
        {
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, aimPosition, Time.deltaTime * 2);
            crosshair.hide();
            animator.SetBool("aiming", true);
        }
        else
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, originalPosition, Time.deltaTime * aimSpeed);
            crosshair.show();
            animator.SetBool("aiming", false);
        }
    }

    protected virtual void checkInputAnimations()
    {
        if ((Input.GetButtonDown("Fire1") || Input.GetAxis("AxisRT") > 0.5f) && animator.GetBool("shooting") == false)
        {
			if (ammunition > 0) 
			{
				firing = true;
				endReload ();
				animator.SetBool ("shooting", true);
				if (animator.GetBool ("reloading")) 
				{
					animator.SetBool ("reloading", false);
				}
			} 
			else 
			{
				ctrlAudio.playOneSound("Weaponds", noBulletsAudio, transform.position, 0.5f, 0f, 150);
			}
        }
        if (ammunition == 0 || ((Input.GetKey(KeyCode.R) || Input.GetButton("ButtonX")) && ammunition < maxAmmo && animator.GetBool("reloading") == false))
        {
            if (inventory.getAmmo(typeAmmo) > 0)
            {
				//idReloadSound = ctrlAudio.playOneSound("Weaponds", reloadAudio, transform.position, 0.5f, 0f, 150, true);
                animator.SetBool("reloading", true);
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

        inventory.decreaseAmmo(typeAmmo, maxAmmo - ammunition);

        if ((maxAmmo - ammunition) > ammoTemp)
        {
            ammunition += ammoTemp;
        }
        else
        {
            ammunition = maxAmmo;
        }
        
        inventory.setAmmo(typeAmmo, ammunition);

		ctrlAudio.playOneSound("Weaponds", reloadAudio, transform.position, 0.5f, 0f, 150);
    }

    protected virtual void shotBullet(Ray ray)
    {
        RaycastHit hitInfo;

        if (Physics.Raycast(ray, out hitInfo, weaponRange, maskBullets))
        {
            if (hitInfo.transform.tag == "Enemy" || hitInfo.transform.tag == "Drone" || hitInfo.transform.tag == "Snitch")
            {
                hitInfo.collider.gameObject.GetComponent<Enemy>().getHit(damage);
                ScoreController.weaponHit(type);
                GameObject spark = Instantiate(sparks, hitInfo.point, Quaternion.Euler(0f, 0f, 0f));
                spark.transform.up = hitInfo.normal;
            }
            else if (hitInfo.transform.tag == "Barrel")
            {
                //hitInfo.collider.gameObject.gameObject.GetComponent<Barrel>().shotBehavior(hitInfo.point, damage);
            }
        }

        //Trail effect
        /*if (trailEffect != null)
        {
            trailEffect.transform.LookAt(ray.origin + ray.direction * weaponRange);
            trailEffect.GetComponent<ParticleSystem>().Play();
        }*/
    }

    public void endReload()
    {
        animator.SetBool("reloading", false);
    }
    protected virtual void shoot()
    {
        ctrlAudio.playOneSound("Weaponds", shotAudio, transform.position, 1.0f, 0f, 150);
        if (shootEffect != null)
        {
            shootEffect.Play();
        }
    }

    public virtual void restartParameters()
    {
        firing = false;
        if (animator != null)
        {
            animator.SetBool("reloading", false);
        }
    }
}
