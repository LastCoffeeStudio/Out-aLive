using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CtrlMenuButtons : MonoBehaviour
{
    public  Main main;

    // Use this for initialization
    void Start ()
    {
        main = FindObjectOfType<Main>();
    }

    public void clickStart()
    {
        //main.changeSceneTo(Main.Scenes.GAME);
    }

    public void clickExit()
    {
        Application.Quit();
    }
}
