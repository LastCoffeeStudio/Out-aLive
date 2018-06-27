using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricShot : MonoBehaviour {

    public float lifeTime;
    public int damage;

    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Hey");
        if (other.tag == "Enemy" || other.tag == "Agent" || other.tag == "Snitch")
        {
            other.gameObject.GetComponent<Enemy>().getHit(damage);
            other.gameObject.GetComponent<Enemy>().setStatusParalyzed();
        }
    }
}
