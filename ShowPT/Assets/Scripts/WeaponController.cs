using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

//TODO: Make Inheritance with Weapon, shared with all weapon scripts

public class WeaponController : MonoBehaviour
{
    public AudioSource shotAudio;
    public AudioSource reloadAudio;
    public AudioSource lastReloadAudio;
    public float amount;
    public float maxAmount;
    public float smoothAmount;
    public int damage;
    public Transform shootPoint;
    public Transform camera;
    public uint maxAmmo = 10;
    public uint ammunition;
    public Text text;
    public Text numDronsText;

    private bool firing = false;
    private bool reloading = false;
    private bool aimming = false;
    private Animator animator;
    private Vector3 initialposition;

    public Vector3 aimPosition;
    public Vector3 originalPosition;
    public float aimSpeed;
    public int shotgunBullets;
    public float bulletDispersion;
    public float weaponRange;
    public GameObject esferaVerde;
    public GameObject esferaRoja;
    public GameObject explosion;

    public int numDrons = 0;

    private Inventory inventory;
    public Inventory.AMMO_TYPE typeAmmo;

    public Text wazPuntuation;
    public Text turretPuntuation;
    public Text dronsPuntuation;
    public Text totalEnemies;
    public Text totalScore;

    void Start () {
        ammunition = maxAmmo;
        initialposition = transform.localPosition;
        animator = gameObject.GetComponent<Animator>();
        inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();
        typeAmmo = Inventory.AMMO_TYPE.SHOTGUNAMMO;
        if (dronsPuntuation != null)
        {
            dronsPuntuation.text = "-";
        }
        if (turretPuntuation != null)
        {
            turretPuntuation.text = "-";
        }
        if (wazPuntuation != null)
        {
            wazPuntuation.text = "-";
        }
        if (totalEnemies != null)
        {
            totalEnemies.text = "-";
        }
        if (totalScore != null)
        {
            totalScore.text = "0";
        }
    }

    private bool playLastReload = false;
	void Update () {
        
        if (GameManager.debugMode)
        {
            Vector3 dir = transform.forward + (Vector3.Cross(camera.forward,Random.insideUnitSphere) * Random.Range(0f, bulletDispersion));
            Debug.DrawRay(camera.position, dir*2, new Color(0f, 255f, 0f));
        }

        //text.text = ammunition.ToString();

        if (!firing && !reloading && !aimming)
        {
            swagWeaponMovement();
        }
        
	    if (!playLastReload && reloading && ammunition == maxAmmo)
	    {
	        playLastReload = true;

            lastReloadAudio.Play();
	    }
		if (CtrlPause.gamePaused == false) 
		{
			checkInputAnimations ();
		}
        aimAmmo();
    }

    void aimAmmo()
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

    private void swagWeaponMovement()
    {
        reloadAudio.Stop();
        float movX = -Input.GetAxis("Mouse X") * amount;
        float movY = -Input.GetAxis("Mouse Y") * amount;
        movX = Mathf.Clamp(movX, -maxAmount, maxAmount);
        movY = Mathf.Clamp(movY, -maxAmount, maxAmount);

        Vector3 finalPosition = new Vector3(movX, movY, 0);
        transform.localPosition = Vector3.Lerp(transform.localPosition, finalPosition + initialposition, Time.deltaTime * smoothAmount);
    }

    private void checkInputAnimations()
    {
        if (Input.GetButton("Fire1") || Input.GetAxis("AxisRT") > 0.5f)
        {
            if (ammunition > 0)
            {
                animator.SetBool("shooting",true);
                if (animator.GetBool("reloading"))
                {
                    reloading = false;
                    animator.SetBool("reloading",false);
                }
            }
        }
        if ((Input.GetKey(KeyCode.R) || Input.GetButton("ButtonX")) && ammunition < maxAmmo)
        {
            if (inventory.getAmmo(typeAmmo) > 0)
            {
                reloading = true;
                animator.SetBool("reloading", true);
                playLastReload = false;
                reloadAudio.Play();
                if (ammunition == maxAmmo - 1)
                {
                    reloadAudio.Stop();
                    lastReloadAudio.Play();
                    animator.SetBool("lastBullet", true);
                }
            }
        }
    }

    void checkMouseInput()
    {
        if (!Input.GetButton("Fire1") || Input.GetAxis("AxisRT") > 0.5f || ammunition == 0)
        {
            animator.SetBool("shooting", false);
        }
    }

    void decreaseAmmo()
    {
        --ammunition;
        bool destroyed = false;
        shotAudio.Play();
        //switch with different weapons
        for (uint i = 0; i < shotgunBullets; ++i)
        {
            Vector3 dir = transform.forward + (Vector3.Cross(camera.forward, Random.insideUnitSphere) * Random.Range(0f, bulletDispersion));
            RaycastHit hitInfo;


            if (Physics.Raycast(camera.position, dir, out hitInfo, weaponRange))
            {
                if (hitInfo.transform.tag == "Agent")
                {
                    destroyed = true;
                    Destroy(hitInfo.collider.gameObject);
                    GameObject.Instantiate(explosion, hitInfo.point, Quaternion.Euler(0f, 0f, 0f));
                }
                else
                {
                    GameObject.Instantiate(esferaVerde, hitInfo.point, Quaternion.Euler(0f, 0f, 0f));
                }

                if (hitInfo.transform.tag == "Enemy" || hitInfo.transform.tag == "Drone")
                {
                    hitInfo.collider.gameObject.GetComponent<Enemy>().getHit(damage);
                }
            }
            
        }
        if (destroyed)
        {
            ++numDrons;
            int totalScoreInt = numDrons * 1236;

            if (dronsPuntuation != null)
            {
                dronsPuntuation.text = numDrons.ToString();
            }
            if (totalEnemies != null)
            {
                totalEnemies.text = numDrons.ToString();
            }
            if (totalScore != null)
            {
                totalScore.text = totalScoreInt.ToString();
            }
        }
    }
       

    void increaseAmmo()
    {
        ++ammunition;
        inventory.decreaseAmmo(typeAmmo, 1);
    }

    void endReload()
    {
        reloading = false;
        animator.SetBool("lastBullet", false);
        animator.SetBool("reloading",false);
    }

    void reloadAndCheck()
    {
        if (ammunition == maxAmmo - 1)
        {
            animator.SetBool("lastBullet", true);
        }
    }
    
}
