using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drone1 : Enemy {

    public Transform player;

	// Use this for initialization
	private void Start ()
    {
        active = false;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        //ctrAudio = GameObject.FindGameObjectWithTag("CtrlAudio").GetComponent<CtrlAudio>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (player != null)
        {
            checkPlayerDistance();
        }
        
    }

    public override void getHit(int damage)
    {
        enemyHealth -= damage;
        checkHealth();
    }

    private void checkPlayerDistance()
    {
       
    }

    public override void checkHealth()
    {
        if (enemyHealth <= 0f)
        {
            Destroy(transform.parent.gameObject);
            ScoreController.addDead(ScoreController.Enemy.DRON);
        }
    }

}
