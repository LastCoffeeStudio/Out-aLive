using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CtrlDebug : MonoBehaviour
{
    public GameObject player;
    public GameObject camerasDebug;
    private bool activeDebug;
    private int indexChildren;
    private int sizeChildren;

    public GameObject gun;
    public GameObject shotgun;
    public GameObject rifle;
    public GameObject canon;

	public GameObject neons;
	public GameObject enemies;

    // Use this for initialization
    void Start ()
	{
	    activeDebug = false;
        player = GameObject.FindGameObjectWithTag("Player");
        camerasDebug = GameObject.Find("CamerasDebug");
    }
	
	// Update is called once per frame
	void Update () {
	    if (activeDebug)
	    {
	        if (Input.GetMouseButtonDown(0))
	        {
	            camerasDebug.transform.GetChild(indexChildren).gameObject.SetActive(false);
	            indexChildren = (indexChildren + 1) % sizeChildren;
	            camerasDebug.transform.GetChild(indexChildren).gameObject.SetActive(true);
	        }
	    }
	    if (Input.GetKeyDown(KeyCode.X))
	    {
	        if (activeDebug)
	        {
                disableDebug();
	        }
	        else
	        {
	            enableDebug();
	        }
        }
	    if (Input.GetKeyDown(KeyCode.T))
	    {
	       Vector3 positionCamera = camerasDebug.transform.GetChild(indexChildren).gameObject.transform.position;
           disableDebug();
	       player.transform.position = positionCamera;
	    }
	    if (Input.GetKeyDown(KeyCode.G))
	    {
	        player.GetComponent<PlayerHealth>().maxHealth = Int32.MaxValue;
	        player.GetComponent<PlayerHealth>().health = Int32.MaxValue;

            for (int i = 0; i < (int) Inventory.WEAPON_TYPE.TOTAL_WEAPONS; ++i)
	        {
	            player.GetComponent<Inventory>().weaponsCarrying[i] = true;
            }
	        for (int i = 0; i < (int)Inventory.AMMO_TYPE.TOTAL_AMMO; ++i)
	        {
	            player.GetComponent<Inventory>().totalAmmoInvenotry[(Inventory.AMMO_TYPE)i] = Int32.MaxValue;
                player.GetComponent<Inventory>().ammoInvenotry[(Inventory.AMMO_TYPE)i] = Int32.MaxValue;
            }

	        gun.GetComponent<Weapon>().maxAmmo = Int32.MaxValue;
	        gun.GetComponent<Weapon>().ammunition = Int32.MaxValue;
	        shotgun.GetComponent<Shotgun>().maxAmmo = Int32.MaxValue;
	        shotgun.GetComponent<Shotgun>().ammunition = Int32.MaxValue;
	        rifle.GetComponent<Weapon>().maxAmmo = Int32.MaxValue;
	        rifle.GetComponent<Weapon>().ammunition = Int32.MaxValue;
            canon.GetComponent<Weapon>().maxAmmo = Int32.MaxValue;
            canon.GetComponent<Weapon>().ammunition = Int32.MaxValue;
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
	        for (int i = 0; i < enemies.Length; i++)
	        {
                if (enemies[i].GetComponent<Enemy>() != null)
                {
	                enemies[i].GetComponent<Enemy>().enemyHealth = 1;
                }
            }
	        enemies = GameObject.FindGameObjectsWithTag("Drone");
	        for (int i = 0; i < enemies.Length; i++)
	        {
	            if (enemies[i].GetComponent<Enemy>() != null)
	            {
	                enemies[i].GetComponent<Enemy>().enemyHealth = 1;
	            }
	        }
	        enemies = GameObject.FindGameObjectsWithTag("Snitch");
	        for (int i = 0; i < enemies.Length; i++)
	        {
	            if (enemies[i].GetComponent<Enemy>() != null)
	            {
	                enemies[i].GetComponent<Enemy>().enemyHealth = 1;
	            }
	        }
        }

		//Activate/Deactivate neons
		if (Input.GetKeyDown (KeyCode.N)) 
		{
			if (neons.activeInHierarchy) 
			{
				neons.SetActive (false);
			} 
			else 
			{
				neons.SetActive (true);
			}
		}

		//Activate/Deactivate enemies
		if (Input.GetKeyDown (KeyCode.M)) 
		{
			if (enemies.activeInHierarchy) 
			{
				enemies.SetActive (false);
			} 
			else 
			{
				enemies.SetActive (true);
			}
		}
    }

    void enableDebug()
    {
        activeDebug = true;
        player.SetActive(false);
        indexChildren = 0;
        sizeChildren = camerasDebug.transform.childCount;
        camerasDebug.transform.GetChild(indexChildren).gameObject.SetActive(true);
    }

    void disableDebug()
    {
        camerasDebug.transform.GetChild(indexChildren).gameObject.SetActive(false);
        activeDebug = false;
        player.SetActive(true);
    }
}
