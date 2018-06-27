﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drone1 : Enemy {

    public Transform player;
    public float shootingDistance = 20f;

	// Use this for initialization
	private void Start ()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        ctrAudio = GameObject.FindGameObjectWithTag("CtrlAudio").GetComponent<CtrlAudio>();
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
        ctrAudio.playOneSound("Enemies", hitAudio, transform.position, 1.0f, 0.0f, 128);
        enemyHealth -= damage;
        //Debug.Log(enemyHealth);
        //Execute properly Animation
        checkHealth();
    }

    private void checkPlayerDistance()
    {
        if (player != null && Vector3.Distance(player.position,transform.position) <= shootingDistance)
        {
            active = true;
        }
        else
        {
            active = false;
        }
    }

    public override void checkHealth()
    {
        if (enemyHealth <= 0f)
        {
            Destroy(gameObject);
            ScoreController.addDead(ScoreController.Enemy.DRON);
        }
    }

}
