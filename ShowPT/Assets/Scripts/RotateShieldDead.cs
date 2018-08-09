using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateShieldDead : MonoBehaviour
{

    private int speedX;
    private int speedY;
    private int speedZ;

    // Use this for initialization
    void Start ()
    {
        speedX = Random.Range(400, 800);
        speedY = Random.Range(400, 800);
        speedZ = Random.Range(400, 800);
        GetComponent<RotateShieldDead>().enabled = false;
    }
    

    private void OnTriggerEnter(Collider other)
    {
        if (enabled && other.gameObject.layer != LayerMask.GetMask("Drone") && other.gameObject.name != "GunProjectile(Clone)")
        {
           GetComponent<ShieldDroneEnemy>().explode();
        }
    }


    // Update is called once per frame
	void Update () {
	    transform.Rotate(Time.deltaTime* speedX, Time.deltaTime* speedY, Time.deltaTime* speedZ);
    }
}
