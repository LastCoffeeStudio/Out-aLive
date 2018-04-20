using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourceMachine : MonoBehaviour {

    [Range(0.4f,0.9f)]
    public float minimumAxisValue;
    public Inventory playerInventory;
    public Button[] buttons;
    public uint ammoShotgun;
    public uint ammoRifle;
    public int points;

    private struct MachineButton
    {
        public Button up;
        public Button left;
        public Button right;
        public Button down;
    }

    private int selected;
    private bool dPadVerticalPressed;
    private bool dPadHorizontalPressed;
    private MachineButton[] sideButtons;
    private Dictionary<Button, int> buttonsIndices;

    // Use this for initialization
    void Start ()
    {
        dPadVerticalPressed = false;
        dPadHorizontalPressed = false;

        buttonsIndices = new Dictionary<Button, int>(buttons.Length);
        for (int i = 0; i < buttons.Length; ++i)
        {
            buttonsIndices.Add(buttons[i], i);
        }

        sideButtons = new MachineButton[buttons.Length];
        for (int i = 0; i < buttons.Length; ++i)
        {
            sideButtons[i].up = buttons[i].GetComponent<SideButtons>().up;
            sideButtons[i].left = buttons[i].GetComponent<SideButtons>().left;
            sideButtons[i].right = buttons[i].GetComponent<SideButtons>().right;
            sideButtons[i].down = buttons[i].GetComponent<SideButtons>().down;
        }
    }

    private void OnEnable()
    {
        if (buttons.Length > 0)
        {
            selected = 0;
            buttons[0].Select();
        }
    }

    // Update is called once per frame
    void Update ()
    {
        checkPlayerInput();
	}

    private void checkPlayerInput()
    {
        if (Input.GetAxis("CrossAxisX") > minimumAxisValue && !dPadHorizontalPressed)
        {
            selectSideButton(sideButtons[selected].right);
            dPadHorizontalPressed = true;
        }
        else if (Input.GetAxis("CrossAxisX") < -minimumAxisValue && !dPadHorizontalPressed)
        {
            selectSideButton(sideButtons[selected].left);
            dPadHorizontalPressed = true;
        }
        else if (Input.GetAxis("CrossAxisX") == 0f)
        {
            dPadHorizontalPressed = false;
        }

        if (Input.GetAxis("CrossAxisY") > minimumAxisValue && !dPadVerticalPressed)
        {
            selectSideButton(sideButtons[selected].up);
            dPadVerticalPressed = true;
        }
        else if (Input.GetAxis("CrossAxisY") < -minimumAxisValue && !dPadVerticalPressed)
        {
            selectSideButton(sideButtons[selected].down);
            dPadVerticalPressed = true;
        }
        else if (Input.GetAxis("CrossAxisY") == 0f)
        {
            dPadVerticalPressed = false;
        }
    }

    public void buyWeapon(string name)
    {
        //TODO: Change points cost into score system
        Inventory.WEAPON_TYPE type = Inventory.WEAPON_TYPE.NO_WEAPON;
        int cost = 0;

        switch (name)
        {
            case "Gun":
                type = Inventory.WEAPON_TYPE.GUN;
                cost = 5;
                Debug.Log("Buy gun");
                break;
            case "Shotgun":
                type = Inventory.WEAPON_TYPE.SHOTGUN;
                cost = 20;
                Debug.Log("Buy shotgun");
                break;
            case "Rifle":
                type = Inventory.WEAPON_TYPE.RIFLE;
                cost = 50;
                Debug.Log("Buy rifle");
                break;
        }

        if (points > cost && type != Inventory.WEAPON_TYPE.NO_WEAPON && !playerInventory.hasWeapon(type))
        {
            playerInventory.addWeapon(type);
            points -= cost;
        }
    }

    public void buyAmmo(string name)
    {
        //TODO: Change points cost into score system
        int cost = 0;
        switch (name)
        {
            case "Shotgun":
                cost = 15;
                if (points > cost)
                {
                    playerInventory.addAmmo(Inventory.AMMO_TYPE.SHOTGUNAMMO, ammoShotgun);
                    points -= cost;
                    Debug.Log("Buy shotgun ammo");
                }
                break;
            case "Rifle":
                cost = 25;
                if (points > cost)
                {
                    playerInventory.addAmmo(Inventory.AMMO_TYPE.RIFLEAMMO, ammoRifle);
                    points -= cost;
                    Debug.Log("Buy rifle ammo");
                }
                break;
        }
    }

    private void selectSideButton(Button button)
    {
        if (button != null)
        {
            selected = buttonsIndices[button];
            buttons[selected].Select();
        }
    }
}
