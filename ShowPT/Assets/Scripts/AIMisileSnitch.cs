using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIMisileSnitch : MonoBehaviour
{
    [HideInInspector]
    public AISnitch aiSnitch;
    private Transform originalTransform;
    private Transform originalParent;
    public float maxSpeed;
    public float maxAcceleration;
    public float distanceExplosion = 5f;
    public int animationBlast;
    private bool trackPlayer = false;
    private Vector3 position;
    private Vector3 speed = Vector3.zero;
    private Vector3 acceleration = Vector3.zero;
    private GameObject player;
    
    // Use this for initialization
    void Start ()
    {
        position = transform.position;
        originalTransform = transform;
        player = GameObject.FindGameObjectWithTag("Player");
        originalParent = transform.parent;

    }
	
	// Update is called once per frame
	void Update ()
	{
	    if (trackPlayer)
	    {
	        //transform.parent = null;
	        float t = Time.deltaTime;

	        acceleration = combine();
	        acceleration = Vector3.ClampMagnitude(acceleration, maxAcceleration);

	        speed = speed + acceleration * t;
	        speed = Vector3.ClampMagnitude(speed, maxSpeed);

	        position = position + speed ;

	        transform.position = position;

	        if (speed.magnitude > 0)
	        {
	           transform.LookAt(position+speed);
	        }
        }
	}


    private Vector3 combine()
    {
        return Vector3.Normalize(player.transform.position - this.position);
    }
    public void blast()
    {
        switch (animationBlast)
        {
            case 0:
                GetComponent<Animator>().Play("MisileBlast");
                break;
            case 1:
                GetComponent<Animator>().Play("MisileBlast2");
                break;
            case 2:
                GetComponent<Animator>().Play("MisileBlast3");
                break;
            case 3:
                GetComponent<Animator>().Play("MisileBlast4");
                break;
        }
        
    }

    private void OnCollisionEnter(Collision other)
    {
        RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(transform.position, player.transform.position - transform.position, out hit, distanceExplosion))
        {
            if (hit.transform.tag == "Player")
            {
                player.GetComponent<PlayerHealth>().health -= 1;
            }
        }
        transform.position = originalTransform.position;
        transform.rotation = originalTransform.rotation;
        GetComponent<CapsuleCollider>().enabled = false;
        GetComponent<Animator>().enabled = true;
        aiSnitch.misileFree(gameObject);
    }

    public void finishBlast()
    {
        position = transform.position;
        GetComponent<CapsuleCollider>().enabled = true;
        speed = transform.GetChild(0).transform.position - transform.position;
        speed = Vector3.ClampMagnitude(speed, 0.1f);
        trackPlayer = true;
        GetComponent<Animator>().enabled = false;
    }
    
    
}
