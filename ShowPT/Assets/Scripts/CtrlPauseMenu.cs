using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CtrlPauseMenu : MonoBehaviour {

    [Range(0.4f,0.9f)]
    public float minimumAxisValue;
    public Button[] buttons;
    
    private int selected;
    private bool dPadVerticalPressed;

    // Use this for initialization
    void Start ()
    {
        dPadVerticalPressed = false;
        selected = -1;
    }

    // Update is called once per frame
    void Update ()
    {
        checkPlayerInput();
	}

    private void checkPlayerInput()
    {
        if ((Input.GetAxis("CrossAxisY") > minimumAxisValue && !dPadVerticalPressed) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (selected >= 1)
            {
                selectButton(selected - 1);
            }
            dPadVerticalPressed = true;
        }
        else if ((Input.GetAxis("CrossAxisY") < -minimumAxisValue && !dPadVerticalPressed) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            if(selected <= 0)
            {
                selectButton(selected + 1);
            }
            dPadVerticalPressed = true;
        }
        else if (Input.GetAxis("CrossAxisY") == 0f)
        {
            dPadVerticalPressed = false;
        }
    }

    private void selectButton(int index)
    {
        if (buttons[index] != null)
        {
            selected = index;
            buttons[selected].Select();
        }
    }
}
