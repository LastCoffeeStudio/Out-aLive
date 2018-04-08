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
    public float weaponRange;
    public GameObject esferaVerde;
    public GameObject esferaRoja;
    public GameObject explosion;

    public int numDrons = 0;
    
    public Inventory.AMMO_TYPE typeAmmo;

    public Text wazPuntuation;
    public Text torretPuntuation;
    public Text dronsPuntuation;
    public Text totalEnemies;
    public Text totalScore;

    private Inventory inventory;
    private bool playLastReload = false;

    // Use this for initialization
    void Start () {
        ammunition = maxAmmo;
        initialposition = transform.localPosition;
        animator = gameObject.GetComponent<Animator>();
        inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();
        typeAmmo = Inventory.AMMO_TYPE.GUNAMMO;
        dronsPuntuation.text = "-";
        torretPuntuation.text = "-";
        wazPuntuation.text = "-";
        totalEnemies.text = "-";
        totalScore.text = "0";
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

    void aimAmmo()
    {
        if (!reloading)
        {
            if (Input.GetButton("Fire2"))
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
        if (Input.GetKey(KeyCode.Mouse0))
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
        if (Input.GetKey(KeyCode.R) && ammunition < maxAmmo)
        {
            if (inventory.getAmmo(typeAmmo) > 0)
            {
                reloading = true;
                animator.SetBool("reloading", true);
            }
        }
    }

    void checkMouseInput()
    {
        if (!Input.GetKey(KeyCode.Mouse0) || ammunition == 0)
        {
            animator.SetBool("shooting", false);
        }
    }

    void decreaseAmmo()
    {
        --ammunition;
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
            else
            {
                GameObject.Instantiate(esferaVerde, hitInfo.point, Quaternion.Euler(0f, 0f, 0f));
            }
        }

        if (destroyed)
        {
            ++numDrons;
            dronsPuntuation.text = numDrons.ToString();
            totalEnemies.text = numDrons.ToString();
            int totalScoreInt = numDrons * 1236;
            totalScore.text = totalScoreInt.ToString();
        }
    }


    void increaseAmmo()
    {
        inventory.increaseAmmo(typeAmmo, maxAmmo - ammunition);
        ammunition = maxAmmo;
    }

    void endReload()
    {
        reloading = false;
        animator.SetBool("reloading", false);
    }
}
