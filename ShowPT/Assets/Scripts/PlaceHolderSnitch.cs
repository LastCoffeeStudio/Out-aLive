using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceHolderSnitch : MonoBehaviour {

    GameObject player;
    Transform front;
    public float velocity;
    // Use this for initialization
    void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
        front = transform.GetChild(0);
    }
	
	// Update is called once per frame
	void Update () {

        float rotationY = orientation2D(transform.position.x, transform.position.z, front.position.x,
            front.position.z, player.transform.position.x, player.transform.position.z);
        transform.Rotate(0f, rotationY*velocity*Time.deltaTime, 0f);
    }

    private int orientation2D(float centerA, float centerB, float pointA, float pointB, float targetA, float targetB)
    {
        double result = ((pointA - centerA) * (targetB - centerB)) - ((pointB - centerB) * (targetA - centerA));

        if (result > 0) return -1;
        else if (result < 0) return 1;
        return 0;
    }
    
}
