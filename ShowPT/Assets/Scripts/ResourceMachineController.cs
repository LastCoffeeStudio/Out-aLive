using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ResourceMachineController : MonoBehaviour {

    public enum ResourceCategory
    {
        NONE = -1,
        WEAPON,
        AMMO,
        HEALTH,

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
        HEALTH,

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
        public int limit;
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
    private Inventory playerInventory;
    private LimitShopController limitShopController;
    private ScoreController scoreController;
    private PlayerHealth playerHealth;

    [Header("Resources Data")]
    [SerializeField]
    private Resource[] resources;
    private int indexActualResource;
    [SerializeField]
    private Resource[] resourcesAmmo;
    private List<Resource> actualResources;

    [Header("Player stats (Debug)")]
    public int score;

    [Header("Sounds")]
    public AudioClip negativeSelection;
    public AudioClip soldAmmo;
    public AudioClip heathSold;

    private CtrlAudio ctrlAudio;

    public delegate void WeapondBought(ResourceType weapondType, ResourceType ammoType);
    public static event WeapondBought weapondBought;

    private void Start()
    {
        playerInventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();
        limitShopController = GameObject.FindGameObjectWithTag("Player").GetComponent<LimitShopController>();
        ctrlAudio = GameObject.FindGameObjectWithTag("CtrlAudio").GetComponent<CtrlAudio>();
        playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
        scoreController = GameObject.FindGameObjectWithTag("SceneUI").GetComponent<ScoreController>();
        actualResources = new List<Resource>();
        actualResources = resources.ToList();
        indexActualResource = 0;
        updateSelected(0);
        weapondBought += changeWeapondToAmmo;
    }

    public void updateSelected(int index)
    {
        indexActualResource += index;
        if (indexActualResource >= actualResources.Count)
        {
            indexActualResource = 0;
        }
        if (indexActualResource < 0)
        {
            indexActualResource = actualResources.Count - 1;
        }

        //updateResourceData();
        updateUI();
    }

    private void updateResourceData()
    {
        if (actualResources[indexActualResource].locked && score >= actualResources[indexActualResource].cost)
        {
            Resource resourceAct =  actualResources[indexActualResource];
            resourceAct.locked = false;
            actualResources[indexActualResource] = resourceAct;
        }
    }

    private void updateUI()
    {
        resourceImage.sprite = actualResources[indexActualResource].image;
        resourceImage.rectTransform.sizeDelta = new Vector2(actualResources[indexActualResource].width, actualResources[indexActualResource].height);
        resourceText.text = LocalizationManager.instance.getLocalizedValue(actualResources[indexActualResource].text);
        descriptionText.text = LocalizationManager.instance.getLocalizedValue(actualResources[indexActualResource].description);

        switch (actualResources[indexActualResource].category)
        {
            case ResourceCategory.WEAPON:
                costLayer.SetActive(true);
                costText.text = actualResources[indexActualResource].cost.ToString();
                lockImage.gameObject.SetActive(true);
                if (scoreController.getTotalScore() < actualResources[indexActualResource].cost)
                {
                    lockImage.sprite = locked;
                }
                else
                {
                    lockImage.sprite = unlocked;
                }
                break;
            case ResourceCategory.AMMO:
            case ResourceCategory.HEALTH:
                costLayer.SetActive(false);
                lockImage.gameObject.SetActive(false);
                break;
        }
    }

    public void buyResource()
    {
        switch (actualResources[indexActualResource].category)
        {
            case ResourceCategory.WEAPON:
                buyWeapon();
                break;
            case ResourceCategory.AMMO:
                buyAmmo();
                break;
            case ResourceCategory.HEALTH:
                buyHealth();
                break;
        }
    }

    private void buyWeapon()
    {
        Inventory.WEAPON_TYPE type = Inventory.WEAPON_TYPE.NO_WEAPON;
        int cost = actualResources[indexActualResource].cost;
        ResourceType ammoSelect = ResourceType.NONE;
        ResourceType weapondSelect = ResourceType.NONE;
        switch (actualResources[indexActualResource].type)
        {
            case ResourceType.GUN:
                type = Inventory.WEAPON_TYPE.GUN;
                deleteElement();
                break;
            case ResourceType.SHOTGUN:
                type = Inventory.WEAPON_TYPE.SHOTGUN;
                weapondSelect = ResourceType.SHOTGUN;
                ammoSelect = ResourceType.SHOTGUNAMMO;
                break;
            case ResourceType.CANNON:
                type = Inventory.WEAPON_TYPE.CANON;
                weapondSelect = ResourceType.CANNON;
                ammoSelect = ResourceType.CANNONAMMO;
                break;
        }

        if (type != Inventory.WEAPON_TYPE.NO_WEAPON && !playerInventory.hasWeapon(type) && cost <= scoreController.getTotalScore())
        {
            playerInventory.addWeapon(type);
            if (ammoSelect != ResourceType.NONE)
            {
                //changeWeapondToAmmo(ammoWeapondSelect);
                weapondBoughtEvent(weapondSelect, ammoSelect);
            }
        }
        else
        {
            ctrlAudio.playOneSound("UI", negativeSelection, transform.position, 1.0f, 0f, 150);
        }
    }

    private void buyAmmo()
    {
        Inventory.AMMO_TYPE type = Inventory.AMMO_TYPE.NO_AMMO;
        int ammoIncrease = actualResources[indexActualResource].ammount;

        switch (actualResources[indexActualResource].type)
        {
            case ResourceType.SHOTGUNAMMO:
                type = Inventory.AMMO_TYPE.SHOTGUNAMMO;
                break;
            case ResourceType.CANNONAMMO:
                type = Inventory.AMMO_TYPE.CANONAMMO;
                break;
        }

        if (type != Inventory.AMMO_TYPE.NO_AMMO && actualResources[indexActualResource].limit > 0)
        {
           // --resources[indexActualResource].limit;
            playerInventory.increaseAmmo(type, ammoIncrease);
            ctrlAudio.playOneSound("UI", soldAmmo, transform.position, 1.0f, 0f, 43);
        }
        else
        {
            ctrlAudio.playOneSound("UI", negativeSelection, transform.position, 1.0f, 0f, 43);
        }
        deleteElement();
    }

    private void buyHealth()
    {
        if (actualResources[indexActualResource].limit > 0 && playerHealth.health < playerHealth.maxHealth)
        {
            //--resources[indexActualResource].limit;
            ctrlAudio.playOneSound("UI", heathSold, transform.position, 0.5f, 0f, 43);
            playerHealth.buyHealth();
            deleteElement();
        }
        else
        {
            ctrlAudio.playOneSound("UI", negativeSelection, transform.position, 1.0f, 0f, 43);
        }
    }

    private void changeWeapondToAmmo(ResourceType weapondType, ResourceType ammoType)
    {
        for (int i = 0; i < actualResources.Count; ++i)
        {
            if (actualResources[i].type == weapondType)
            {
                for (int j = 0; j < resourcesAmmo.Length; ++j)
                {
                    if (resourcesAmmo[j].type == ammoType)
                    {
                        actualResources[i] = resourcesAmmo[j];
                    }
                }
            }
        }
        updateSelected(indexActualResource);
    }

    private void deleteElement()
    {
        actualResources.RemoveAt(indexActualResource);
        if (actualResources.Count == 0)
        {
            for (int i = 0; i < resourcesAmmo.Length; ++i)
            {
                if (resourcesAmmo[i].category == ResourceCategory.NONE)
                {
                    actualResources.Add(resourcesAmmo[resourcesAmmo.Length - 1]);
                }
            }
        }
        updateSelected(indexActualResource);
    }

    protected virtual void weapondBoughtEvent(ResourceType weapondType, ResourceType ammoType)
    {
        var handler = weapondBought;
        if (handler != null)
        {
            handler(weapondType, ammoType);
        }
    }
}
