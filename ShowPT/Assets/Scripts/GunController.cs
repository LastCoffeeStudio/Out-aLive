using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

//TODO: Make Inheritance with Weapon, shared with all weapon scripts

public class GunController : MonoBehaviour {
    
    public float amount;
    public float maxAmount;
    public float smoothAmount;
    public int damage;
    public Transform shootPoint;
    public Transform camera;
    public int maxAmmo = 10;
    public int ammunition;
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
    public float weaponRange;
    public GameObject esferaVerde;
    public GameObject esferaRoja;
    public GameObject explosion;
    public ParticleSystem shootEffect;

    public int numDrons = 0;
    
    public Inventory.AMMO_TYPE typeAmmo;

    private Inventory inventory;
    private bool playLastReload = false;

    public AudioClip shot;
    public AudioClip reload;
    private CtrlAudio ctrlAudio;

    // Use this for initialization
    void Start () {
        ammunition = maxAmmo;
        initialposition = transform.localPosition;
        animator = gameObject.GetComponent<Animator>();
        inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();
        typeAmmo = Inventory.AMMO_TYPE.GUNAMMO;
        ctrlAudio = GameObject.FindGameObjectWithTag("CtrlAudio").GetComponent<CtrlAudio>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        //text.text = ammunition.ToString();

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

    void playStep()
    {
        gameObject.GetComponentInParent<PlayerMovment>().PlayStep();
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
        float movX = -Input.GetAxis("Mouse X") * amount;
        float movY = -Input.GetAxis("Mouse Y") * amount;
        movX = Mathf.Clamp(movX, -maxAmount, maxAmount);
        movY = Mathf.Clamp(movY, -maxAmount, maxAmount);

        Vector3 finalPosition = new Vector3(movX, movY, 0);
        transform.localPosition = Vector3.Lerp(transform.localPosition, finalPosition + initialposition, Time.deltaTime * smoothAmount);
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
                //ctrlAudio.playOneSound("Player", shot, transform.position, 1.0f, 0f, 150);

            }
        }
        if ((Input.GetKey(KeyCode.R) || Input.GetButton("ButtonX")) && ammunition < maxAmmo && animator.GetBool("reloading") == false)
        {
            if (inventory.getAmmo(typeAmmo) > 0)
            {
                ctrlAudio.playOneSound("Player", reload, transform.position, 1.0f, 0f, 90);
                reloading = true;
                animator.SetBool("reloading", true);
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
        ScoreController.gunUsed();
        inventory.setAmmo(typeAmmo, (int)ammunition);
        bool destroyed = false;
        //switch with different weapons
        RaycastHit hitInfo;

        if (Physics.Raycast(camera.position, transform.forward, out hitInfo, weaponRange))
        {
            if (hitInfo.transform.tag == "Agent")
            {
                destroyed = true;
                Destroy(hitInfo.collider.gameObject);
                GameObject.Instantiate(explosion, hitInfo.point, Quaternion.Euler(0f, 0f, 0f));
            }

            if (hitInfo.transform.tag == "Enemy" || hitInfo.transform.tag == "Drone")
            {
                hitInfo.collider.gameObject.GetComponent<Enemy>().getHit(damage);
                ScoreController.gunHit();
            }
        }

        if (destroyed)
        {
            ++numDrons;
            int totalScoreInt = numDrons * 1236;
        }
    }

    void increaseAmmo()
    {
        int ammoTemp = inventory.getAmmo(Inventory.AMMO_TYPE.GUNAMMO);
        //Only Test
       // inventory.decreaseAmmo(typeAmmo, maxAmmo - ammunition);
        if ((maxAmmo - ammunition) > ammoTemp)
        {
            ammunition += ammoTemp;
        }
        else
        {
            ammunition = maxAmmo;
        }

        inventory.setAmmo(typeAmmo, (int)ammunition);
    }

    void endReload()
    {
        reloading = false;
        animator.SetBool("reloading", false);
    }

    private void shoot()
    {
        ctrlAudio.playOneSound("Player", shot, transform.position, 1.0f, 0f, 150);
        shootEffect.Play();
    }
}
