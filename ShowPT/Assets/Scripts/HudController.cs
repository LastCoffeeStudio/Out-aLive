using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HudController : MonoBehaviour
{
    private enum LabelType
    {
        NONE = -1,

        AMMO,
        ENERGY,

        TOTAL_LABELS
    }
    private const int MAX_GREEN_LIFE = 60;
    private const int MAX_YELLOW_LIFE = 20;

    private Color32 YELLOW = new Color32(247, 221, 85, 255);
    private Color32 RED = new Color32(255, 0, 85, 255);

    private Color32 STATE_DISABLED = new Color32(255, 255, 255, 80);
    private Color32 STATE_SELECTED = new Color32(255, 255, 255, 255);

    [Header("Labels")]
    public Text valueHealth;
    public Text bulletsLabel;
    public Text totalBulletsLabel;
    public Slider energyLabel;
    public GameObject Ammo;
    public GameObject Energy;

    private int bullets;
    private int totalBullets;

    [SerializeField]
    Slider healthBar;

    public GameObject fillHealth;
    [Header("States")]
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
    private GameObject currentLabel;

    public GameObject menDown;

	[Header("Rotating Health Thingy")]
	[SerializeField]
	Image rotatingBar;
	[SerializeField]
	Sprite rotatingGreen;
	[SerializeField]
	Sprite rotatingYellow;
	[SerializeField]
	Sprite rotatingRed;

    [Header("Crosshair")]
    public GameObject[] weaponsCrosshairs;

    private GameObject currentCrosshair;
    private LabelType currentLabelType;

    // Use this for initialization
    void Start ()
    {
        for (int i = 0; i < weaponsCrosshairs.Length; ++i)
        {
            weaponsCrosshairs[i].SetActive(false);
        }

        //Test
        //selectWeapon(Inventory.WEAPON_TYPE.GUN, 15, 0);
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void ChangeHealthBar(int value)
    {
        healthBar.value = value;
        valueHealth.text = value.ToString();

		/*if (value < MAX_YELLOW_LIFE) 
		{
			rotatingBar.sprite = rotatingRed;
			//fillHealth.GetComponent<Image> ().color = RED;
		} 
		else if (value < MAX_GREEN_LIFE) 
		{
			rotatingBar.sprite = rotatingYellow;
			//fillHealth.GetComponent<Image> ().color = YELLOW;
		} 
		else 
		{
			rotatingBar.sprite = rotatingGreen;
			//fillHealth.GetComponent<Image> ().color = Color.green;
		}*/
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
            case Inventory.WEAPON_TYPE.CANON:
                canonState.GetComponent<Image>().color = STATE_DISABLED;
                break;
        }
    }

    public void selectWeapon(Inventory.WEAPON_TYPE type, int bullets, int totalBullets)
    {
        if (currentWeapon != null && currentSelected != null)
        {
            currentWeapon.GetComponent<Image>().color = STATE_DISABLED;
            currentSelected.SetActive(false);
            currentCrosshair.SetActive(false);
        }

        switch (type)
        {
            case Inventory.WEAPON_TYPE.GUN:
                currentWeapon = pistolState;
                currentSelected = pistolSelected;
                currentCrosshair = weaponsCrosshairs[(int)Inventory.WEAPON_TYPE.GUN];
                currentCrosshair.SetActive(true);
                changeLabel(LabelType.ENERGY);
                break;
            case Inventory.WEAPON_TYPE.SHOTGUN:
                currentWeapon = shotGunState;
                currentSelected = shotGunSelected;
                currentCrosshair = weaponsCrosshairs[(int)Inventory.WEAPON_TYPE.SHOTGUN];
                currentCrosshair.SetActive(true);
                changeLabel(LabelType.AMMO);
                break;
            case Inventory.WEAPON_TYPE.CANON:
                currentWeapon = canonState;
                currentSelected = canonSelected;
                currentCrosshair = weaponsCrosshairs[(int)Inventory.WEAPON_TYPE.CANON];
                currentCrosshair.SetActive(true);
                changeLabel(LabelType.AMMO);
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

    public void updateStats(float actual, float max)
    {
        switch (currentLabelType)
        {
            case LabelType.ENERGY:
                energyLabel.value = actual / max;
                break;
        }
    }

    private void changeLabel(LabelType LType)
    {
        if (currentLabel != null)
        {
            currentLabel.SetActive(false);
        }

        switch (LType)
        {
            case LabelType.AMMO:
                bulletsLabel.text = bullets.ToString();
                totalBulletsLabel.text = totalBullets.ToString();
                currentLabel = Ammo;
                currentLabel.SetActive(true);
                currentLabelType = LabelType.AMMO;
                break;
            case LabelType.ENERGY:
                currentLabel = Energy;
                currentLabel.SetActive(true);
                currentLabelType = LabelType.ENERGY;
                break;
            case LabelType.NONE:
                currentLabel = null;
                currentLabelType = LabelType.NONE;
                break;
        }
    }
}
