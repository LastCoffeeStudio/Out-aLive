using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModifyWallLed3 : MonoBehaviour
{

    public float rango;
    private Transform playerTransform;
    private Renderer rend ;
    // Use this for initialization
    void Start ()
	{
	    playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
	    rend = GetComponent<Renderer>();
    }
	
	// Update is called once per frame
	void Update ()
	{
	    if (playerTransform.position.z < (-60.6f + rango))
	    {
	        Color c = rend.material.color;
	        c.r =  1 - ((-((playerTransform.position.z) - (-60.6f + rango))) / rango);
	        if (c.r > 1)
	        {
	            c.r = 1;

	        }
	        else if (c.r < 0)
            {
                c.r = 0;

            }
	        c.g = c.b = c.r;
            rend.material.SetColor("_Color", c);
        }
	   
    }
}
