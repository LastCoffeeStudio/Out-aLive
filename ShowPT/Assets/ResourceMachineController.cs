using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourceMachineController : MonoBehaviour {

    public enum ResourceCategory
    {
        NONE = -1,
        WEAPON,
        AMMO,

        NUM_CATEGORIES
    }

    public enum ResourceType
    {
        NONE = -1,
        GUN,
        SHOTGUN,
        CANNON,
        SHOTGUNAMMO,
        CANNONAMMO,

        NUM_TYPES
    }

    [System.Serializable]
    public struct Resource
    {
        public ResourceCategory category;
        public ResourceType type;
        public Sprite image;
        public int width;
        public int height;
        public string text;
        public int cost;
        public bool locked;
        public string description;
        public int ammount;
    }

    [Header("Containers")]
    public Image resourceImage;
    public Text resourceText;
    public Text costText;
    public Image lockImage;
    public Text descriptionText;
    public GameObject costLayer;

    [Header("Lock Type")]
    public Sprite locked;
    public Sprite unlocked;

    [Header("Player Data")]
    [SerializeField]
    private Inventory playerInventory;

    [Header("Resources Data")]
    [SerializeField]
    private Resource[] resources;
    private int indexActualResource;

    [Header("Player stats (Debug)")]
    public int score;

    private void Start()
    {
        playerInventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();
        indexActualResource = 0;
        updateSelected(0);
    }

    public void updateSelected(int index)
    {
        indexActualResource += index;
        if (indexActualResource >= resources.Length)
        {
            indexActualResource = 0;
        }
        if (indexActualResource < 0)
        {
            indexActualResource = resources.Length - 1;
        }

        updateResourceData();
        updateUI();
    }

    private void updateResourceData()
    {
        if (resources[indexActualResource].locked && score >= resources[indexActualResource].cost)
        {
            resources[indexActualResource].locked = false;
        }
    }

    private void updateUI()
    {
        resourceImage.sprite = resources[indexActualResource].image;
        resourceImage.rectTransform.sizeDelta = new Vector2(resources[indexActualResource].width, resources[indexActualResource].height);
        resourceText.text = resources[indexActualResource].text;
        descriptionText.text = resources[indexActualResource].description;

        switch (resources[indexActualResource].category)
        {
            case ResourceCategory.WEAPON:
                costLayer.SetActive(true);
                costText.text = resources[indexActualResource].cost.ToString();
                lockImage.gameObject.SetActive(true);
                if (resources[indexActualResource].locked)
                {
                    lockImage.sprite = locked;
                }
                else
                {
                    lockImage.sprite = unlocked;
                }
                break;
            case ResourceCategory.AMMO:
                costLayer.SetActive(false);
                lockImage.gameObject.SetActive(false);
                break;
        }
    }

    public void buyResource()
    {
        switch (resources[indexActualResource].category)
        {
            case ResourceCategory.WEAPON:
                buyWeapon();
                break;
            case ResourceCategory.AMMO:
                buyAmmo();
                break;
        }
    }

    private void buyWeapon()
    {
        Inventory.WEAPON_TYPE type = Inventory.WEAPON_TYPE.NO_WEAPON;

        switch (resources[indexActualResource].type)
        {
            case ResourceType.GUN:
                type = Inventory.WEAPON_TYPE.GUN;
                break;
            case ResourceType.SHOTGUN:
                type = Inventory.WEAPON_TYPE.SHOTGUN;
                break;
            case ResourceType.CANNON:
                type = Inventory.WEAPON_TYPE.CANON;
                break;
        }

        if (type != Inventory.WEAPON_TYPE.NO_WEAPON && !playerInventory.hasWeapon(type))
        {
            playerInventory.addWeapon(type);
        }
    }

    private void buyAmmo()
    {
        Inventory.AMMO_TYPE type = Inventory.AMMO_TYPE.NO_AMMO;
        int ammoIncrease = resources[indexActualResource].ammount;

        switch (resources[indexActualResource].type)
        {
            case ResourceType.SHOTGUNAMMO:
                type = Inventory.AMMO_TYPE.SHOTGUNAMMO;
                
                break;
            case ResourceType.CANNONAMMO:
                type = Inventory.AMMO_TYPE.CANONAMMO;
                break;
        }

        if (type != Inventory.AMMO_TYPE.NO_AMMO)
        {
            playerInventory.increaseAmmo(type, ammoIncrease);
        }
    }
}
