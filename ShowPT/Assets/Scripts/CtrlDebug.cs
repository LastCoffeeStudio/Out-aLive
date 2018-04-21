using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CtrlDebug : MonoBehaviour
{
    public GameObject player;
    public GameObject camerasDebug;
    private bool activeDebug;
    private int indexChildren;
    private int sizeChildren;
    // Use this for initialization
    void Start ()
	{
	    activeDebug = false;
	    player = GameObject.FindGameObjectWithTag("Player");
        camerasDebug = GameObject.Find("CamerasDebug");
    }

    // Update is called once per frame
    void Update()
    {
        if (activeDebug)
        {
            if (Input.GetMouseButtonDown(0))
            {
                camerasDebug.transform.GetChild(indexChildren).gameObject.SetActive(false);
                indexChildren = (indexChildren + 1) % sizeChildren;
                camerasDebug.transform.GetChild(indexChildren).gameObject.SetActive(true);
            }
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            if (activeDebug)
            {
                disableDebug();
            }
            else
            {
                enableDebug();
            }
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            player.GetComponent<PlayerHealth>().ChangeHealth(-5);
        }
    }

    void enableDebug()
    {
        activeDebug = true;
        player.SetActive(false);
        indexChildren = 0;
        sizeChildren = camerasDebug.transform.childCount;
        camerasDebug.transform.GetChild(indexChildren).gameObject.SetActive(true);
    }

    void disableDebug()
    {
        camerasDebug.transform.GetChild(indexChildren).gameObject.SetActive(false);
        activeDebug = false;
        player.SetActive(true);
    }
}
