using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{

    public WEAPON_TYPE weapon;
    public GameObject[] weaponsInventory;

    public enum AMMO_TYPE
    {
        SHOTGUNAMMO,
        GUNAMMO,
        RIFLEAMMO
    }

    public enum WEAPON_TYPE
    {
        GUN,
        SHOTGUN,
        RIFLE,

        TOTAL_WEAPONS,
        NO_WEAPON
    }

    Dictionary<AMMO_TYPE, uint> ammoInvenotry = new Dictionary<AMMO_TYPE, uint>();
    bool[] weaponsCarrying;


    // Use this for initialization
    void Start ()
    {
        weapon = WEAPON_TYPE.GUN;
		ammoInvenotry.Add(AMMO_TYPE.SHOTGUNAMMO, 10);
        ammoInvenotry.Add(AMMO_TYPE.GUNAMMO, 15);
        ammoInvenotry.Add(AMMO_TYPE.RIFLEAMMO, 35);

        weaponsCarrying = new bool[(int)WEAPON_TYPE.TOTAL_WEAPONS];
        weaponsCarrying[(int)WEAPON_TYPE.GUN] = true;
        weaponsCarrying[(int)WEAPON_TYPE.SHOTGUN] = true;
        weaponsCarrying[(int)WEAPON_TYPE.RIFLE] = true;
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
    }

    public uint getAmmo(AMMO_TYPE typeAmmo)
    {
        return ammoInvenotry[typeAmmo];
    }

    public void decreaseAmmo(AMMO_TYPE typeAmmo, uint value)
    {
        ammoInvenotry[typeAmmo] -= value;
    }

    public void increaseAmmo(AMMO_TYPE typeAmmo, uint value)
    {
        ammoInvenotry[typeAmmo] += value;
    }

    private void switchWeapon(WEAPON_TYPE type)
    {
        weaponsInventory[(int)weapon].SetActive(false);
        weapon = type;
        weaponsInventory[(int)weapon].SetActive(true);
        gameObject.GetComponent<PlayerMovment>().animator = weaponsInventory[(int)weapon].GetComponentInChildren<Animator>();
    }
}
