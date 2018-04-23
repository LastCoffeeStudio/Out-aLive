using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HudController : MonoBehaviour {

    private const int MAX_GREEN_LIFE = 60;
    private const int MAX_YELLOW_LIFE = 20;

    private Color32 YELLOW = new Color32(247, 221, 85, 255);
    private Color32 RED = new Color32(255, 0, 85, 255);

    private Color32 STATE_DISABLED = new Color32(255, 255, 255, 80);
    private Color32 STATE_SELECTED = new Color32(255, 255, 255, 255);

    public Text valueHealth;
    public Text bulletsLabel;
    public Text totalBulletsLabel;

    private int bullets;
    private int totalBullets;

    [SerializeField]
    Slider healthBar;

    public GameObject fillHealth;

    public GameObject pistolState;
    public GameObject shotGunState;
    public GameObject smgState;
    public GameObject canonState;

    public GameObject pistolSelected;
    public GameObject shotGunSelected;
    public GameObject smgSelected;
    public GameObject canonSelected;

    private GameObject currentWeapon;
    private GameObject currentSelected;

    public GameObject menDown;

    // Use this for initialization
    void Start () {
        //Test 
        addWeapon(Inventory.WEAPON_TYPE.GUN);
        selectWeapon(Inventory.WEAPON_TYPE.GUN, 15, 0);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ChangeHealthBar(int value)
    {
        healthBar.value = value;
        valueHealth.text = value.ToString();

        if (value < MAX_YELLOW_LIFE)
        {
            fillHealth.GetComponent<Image>().color = RED;
        }
        else if (value < MAX_GREEN_LIFE)
        {
            fillHealth.GetComponent<Image>().color = YELLOW;
        }
    }

    public void setAmmo(int value)
    {
        bullets = value;
        bulletsLabel.text = bullets.ToString();
    }
    public void setTotalAmmo(Inventory.AMMO_TYPE type, int value)
    {
        totalBullets = value;
        if(type == Inventory.AMMO_TYPE.GUNAMMO)
        {
            totalBulletsLabel.text = "∞";
        } else
        {
            totalBulletsLabel.text = totalBullets.ToString();
        }
    }

    public void addWeapon(Inventory.WEAPON_TYPE type)
    {
       switch(type)
        {
            case Inventory.WEAPON_TYPE.GUN:
                pistolState.GetComponent<Image>().color = STATE_DISABLED;
                break;
            case Inventory.WEAPON_TYPE.SHOTGUN:
                shotGunState.GetComponent<Image>().color = STATE_DISABLED;
                break;
            case Inventory.WEAPON_TYPE.RIFLE:
                smgState.GetComponent<Image>().color = STATE_DISABLED;
                break;
        }
    }

    public void selectWeapon(Inventory.WEAPON_TYPE type, int bullets, int totalBullets)
    {
        if (currentWeapon != null && currentSelected != null)
        {
            currentWeapon.GetComponent<Image>().color = STATE_DISABLED;
            currentSelected.SetActive(false);
        }

        switch (type)
        {
            case Inventory.WEAPON_TYPE.GUN:
                currentWeapon = pistolState;
                currentSelected = pistolSelected;
                bulletsLabel.text = bullets.ToString();
                totalBulletsLabel.text = "∞";
                break;
            case Inventory.WEAPON_TYPE.SHOTGUN:
                currentWeapon = shotGunState;
                currentSelected = shotGunSelected;
                bulletsLabel.text = bullets.ToString();
                totalBulletsLabel.text = totalBullets.ToString();
                break;
            case Inventory.WEAPON_TYPE.RIFLE:
                currentWeapon = smgState;
                currentSelected = smgSelected;
                bulletsLabel.text = bullets.ToString();
                totalBulletsLabel.text = totalBullets.ToString();
                break;
        }

        currentWeapon.GetComponent<Image>().color = STATE_SELECTED;
        currentSelected.SetActive(true);
    }

    public void setMenDown(bool v)
    {
        if(v && !menDown.activeSelf)
        {
            menDown.SetActive(true);
        }
        else if (!v && menDown.activeSelf)
        {
            menDown.SetActive(false);
        }
    }
}
