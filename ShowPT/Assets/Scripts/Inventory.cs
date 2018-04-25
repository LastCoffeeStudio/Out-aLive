using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{

    public WEAPON_TYPE weapon;
    public GameObject[] weaponsInventory;
    private int selectedIdWeapond = 0;

    public enum AMMO_TYPE
    {
        SHOTGUNAMMO,
        GUNAMMO,
        RIFLEAMMO,

        TOTAL_AMMO,
        NO_AMMO
    }

    public enum WEAPON_TYPE
    {
        GUN,
        SHOTGUN,
        RIFLE,

        TOTAL_WEAPONS,
        NO_WEAPON
    }

    public Dictionary<AMMO_TYPE, int> ammoInvenotry = new Dictionary<AMMO_TYPE, int>();
    public Dictionary<AMMO_TYPE, int> totalAmmoInvenotry = new Dictionary<AMMO_TYPE, int>();
    public bool[] weaponsCarrying;

    public HudController hudController;

    // Use this for initialization
    void Start ()
    {
        weapon = WEAPON_TYPE.GUN;

        totalAmmoInvenotry.Add(AMMO_TYPE.SHOTGUNAMMO, 10);
        totalAmmoInvenotry.Add(AMMO_TYPE.GUNAMMO, 15);
        totalAmmoInvenotry.Add(AMMO_TYPE.RIFLEAMMO, 35);

        ammoInvenotry.Add(AMMO_TYPE.SHOTGUNAMMO, 12);
        ammoInvenotry.Add(AMMO_TYPE.GUNAMMO, 15);
        ammoInvenotry.Add(AMMO_TYPE.RIFLEAMMO, 35);

        weaponsCarrying = new bool[(int)WEAPON_TYPE.TOTAL_WEAPONS];
        weaponsCarrying[(int)WEAPON_TYPE.GUN] = true;
        weaponsCarrying[(int)WEAPON_TYPE.SHOTGUN] = false;
        weaponsCarrying[(int)WEAPON_TYPE.RIFLE] = false;
        gameObject.GetComponent<PlayerMovment>().animator = weaponsInventory[(int)weapon].GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) && weapon != WEAPON_TYPE.GUN && weaponsCarrying[(int)WEAPON_TYPE.GUN])
        {
            switchWeapon(WEAPON_TYPE.GUN);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2) && weapon != WEAPON_TYPE.SHOTGUN && weaponsCarrying[(int)WEAPON_TYPE.SHOTGUN])
        {
            switchWeapon(WEAPON_TYPE.SHOTGUN);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3) && weapon != WEAPON_TYPE.RIFLE && weaponsCarrying[(int)WEAPON_TYPE.RIFLE])
        {
            switchWeapon(WEAPON_TYPE.RIFLE);
        }

        if (Input.GetButtonDown("ButtonLB") || Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            nextWeapond(-1);
        }
        if (Input.GetButtonDown("ButtonRB") || Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            nextWeapond(+1);
        }
    }

    void nextWeapond(int direction)
    {
        bool switched = false;
        while (switched == false) 
        {
            selectedIdWeapond += direction;
            if (selectedIdWeapond < 0) selectedIdWeapond = (int)WEAPON_TYPE.TOTAL_WEAPONS-1;
            selectedIdWeapond %= (int)WEAPON_TYPE.TOTAL_WEAPONS;
            if (weaponsCarrying[selectedIdWeapond] == true)
            {
               switchWeapon((WEAPON_TYPE)selectedIdWeapond);
               switched = true;
            }
        }
    }

    public int getAmmo(AMMO_TYPE typeAmmo)
    {
        return totalAmmoInvenotry[typeAmmo];
    }

    public void decreaseAmmo(AMMO_TYPE typeAmmo, int value)
    {
        totalAmmoInvenotry[typeAmmo] -= value;
        if (totalAmmoInvenotry[typeAmmo] < 0)
        {
            totalAmmoInvenotry[typeAmmo] = 0;
        }
        hudController.setTotalAmmo(typeAmmo, totalAmmoInvenotry[typeAmmo]);
    }

    public void increaseAmmo(AMMO_TYPE typeAmmo, int value)
    {
        totalAmmoInvenotry[typeAmmo] += value;
        hudController.setTotalAmmo(typeAmmo, totalAmmoInvenotry[typeAmmo]);
    }

    public void setAmmo(AMMO_TYPE typeAmmo, int value)
    {
        if (ammoInvenotry.ContainsKey(typeAmmo)) {

            ammoInvenotry[typeAmmo] = value;
        }
        else
        {
            ammoInvenotry.Add(typeAmmo, value);
        }

        hudController.setAmmo(ammoInvenotry[typeAmmo]);
    }

    private void switchWeapon(WEAPON_TYPE type)
    {
        weaponsInventory[(int)weapon].SetActive(false);
        weapon = type;
        weaponsInventory[(int)weapon].SetActive(true);

        switch(type)
        {
            case WEAPON_TYPE.GUN:
                hudController.selectWeapon(type, ammoInvenotry[AMMO_TYPE.GUNAMMO], getAmmo(AMMO_TYPE.GUNAMMO));
                break;
            case WEAPON_TYPE.SHOTGUN:
                hudController.selectWeapon(type, ammoInvenotry[AMMO_TYPE.SHOTGUNAMMO], getAmmo(AMMO_TYPE.SHOTGUNAMMO));
                break;
            case WEAPON_TYPE.RIFLE:
                hudController.selectWeapon(type, ammoInvenotry[AMMO_TYPE.RIFLEAMMO], getAmmo(AMMO_TYPE.RIFLEAMMO));
                break;
        }
        gameObject.GetComponent<PlayerMovment>().animator = weaponsInventory[(int)weapon].GetComponentInChildren<Animator>();
    }

    public bool hasWeapon(WEAPON_TYPE type)
    {
        return (weaponsCarrying[(int)type]);
    }

    public void addWeapon(WEAPON_TYPE type)
    {
        weaponsCarrying[(int)type] = true;
        hudController.addWeapon(type);
        //Launch some animation or sound that has bought weapon
    }
}
