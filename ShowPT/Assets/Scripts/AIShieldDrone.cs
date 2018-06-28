using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldDrone : MonoBehaviour
{

    private Transform shieldTransform;
    private Transform playerTranform;

	// Use this for initialization
	void Start ()
	{
	    shieldTransform = transform.GetChild(0);
	    playerTranform = GameObject.FindGameObjectWithTag("Player").transform;
	}
	
	// Update is called once per frame
	void Update ()
	{
	    //Vector2 pp = new Vector2(playerTranform.position.z, playerTranform.position.x);
	   // Vector2 sp = new Vector2(shieldTransform.position.z, shieldTransform.position.x);

	    int rotationY = orientation2D(transform.position.x, transform.position.y, shieldTransform.position.x,
	        shieldTransform.position.y, playerTranform.position.x, playerTranform.position.y);
        transform.Rotate(0, rotationY, 0);
	   /* Quaternion q = Quaternion.FromToRotation(sp, pp);
        q = Quaternion.Euler();*/
	}

    private int orientation2D(float centerA, float centerB, float pointA, float pointB, float targetA, float targetB)
    {
        double result = ((pointA - centerA) * (targetB - centerB)) - ((pointB - centerB) * (targetA - centerA));

        if (result > 0) return 1;
        else if (result < 0) return -1;
        return 0;
    }
}
