using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kamikaze : Enemy {

    Transform player;
    public float explosionDistance = 20f;
    public int explosionDamage = 3;

    // Use this for initialization
    void Start() 
	{
		player = GameObject.FindGameObjectWithTag ("Player").GetComponent<Transform>();
	}

    // Update is called once per frame
    void Update()
    {
        explode();
    }

    public override void getHit()
    {
        --enemyHealth;
        Debug.Log(enemyHealth);
        //Execute properly Animation
        checkHealth();
    }

    private void explode()
    {
        if (player != null && Vector3.Distance(player.position, transform.position) <= explosionDistance)
        {
            //Explosion animation
            RaycastHit hitInfo;
            if (Physics.Raycast(transform.position, player.position - transform.position, out hitInfo, explosionDistance) && hitInfo.transform.tag == "Player")
            {
                player.GetComponent<PlayerHealth>().ChangeHealth(explosionDamage);
            }
            Destroy(gameObject);
        }
    }
}
