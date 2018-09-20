using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimitShopController : MonoBehaviour {

    public int gunLimit = 0;
    public int shotgunLimit = 0;
    public int cannonLimit = 0;
    public int shotgunAmmoLimit = 0;
    public int cannonAmmoLimit = 0;
    public int healthLimit = 0;

    public static int gunReminder;
    public static int shotgunReminder;
    public static int cannonReminder;
    public static int shotgunAmmoReminder;
    public static int cannonAmmoReminder;
    public static int healthReminder;

    // Use this for initialization
    void Start () {
        gunReminder = gunLimit;
        shotgunReminder = shotgunLimit;
        cannonReminder = cannonLimit;
        shotgunAmmoReminder = shotgunAmmoLimit;
        cannonAmmoReminder = cannonAmmoLimit;
        healthReminder = healthLimit; 
    }

    public void decreaseReminder(ResourceMachineController.ResourceType type)
    {
        switch(type)
        {
            case ResourceMachineController.ResourceType.GUN:
                gunReminder--;
                break;
            case ResourceMachineController.ResourceType.SHOTGUN:
                shotgunReminder--;
                break;
            case ResourceMachineController.ResourceType.CANNON:
                cannonReminder--;
                break;
            case ResourceMachineController.ResourceType.SHOTGUNAMMO:
                shotgunAmmoReminder--;
                break;
            case ResourceMachineController.ResourceType.CANNONAMMO:
                cannonAmmoReminder--;
                break;
            case ResourceMachineController.ResourceType.HEALTH:
                healthReminder--;
                break;
        }
    }

    public bool canBuy(ResourceMachineController.ResourceType type)
    {
        bool canBuy = false;
        switch (type)
        {
            case ResourceMachineController.ResourceType.GUN:
                canBuy = gunReminder > 0;
                break;
            case ResourceMachineController.ResourceType.SHOTGUN:
                canBuy = shotgunReminder > 0;
                break;
            case ResourceMachineController.ResourceType.CANNON:
                canBuy = cannonReminder > 0;
                break;
            case ResourceMachineController.ResourceType.SHOTGUNAMMO:
                canBuy = shotgunAmmoReminder > 0;
                break;
            case ResourceMachineController.ResourceType.CANNONAMMO:
                canBuy = cannonAmmoReminder > 0;
                break;
            case ResourceMachineController.ResourceType.HEALTH:
                canBuy = healthReminder > 0;
                break;
        }
        return canBuy;
    }

}
