﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIMisileSnitch : MonoBehaviour
{
    [HideInInspector]
    public AISnitch aiSnitch;
    private Transform originalTransform;
    public float maxSpeed;
    public float maxAcceleration;
    [HideInInspector]
    public int distanceExplosion;
    public int animationBlast;
    private bool trackPlayer = false;
    private Vector3 position;
    private Vector3 speed = Vector3.zero;
    private Vector3 acceleration = Vector3.zero;
    private GameObject player;
    private Transform parentObj;
    private ParticleSystem fire;
    private ParticleSystem trail;

	[SerializeField]
	GameObject explosionEffect;

    [Header("Sounds")]
    public AudioClip audioBlast;
    public AudioClip audioMisile;
    public AudioClip audioExplode;
    private ulong idAudioBlast;
    private ulong idAudioMisile;
    private ulong idAudioExplode;
    private CtrlAudio ctrlAudio;

    // Use this for initialization
    void Start ()
    {

        position = transform.position;
        originalTransform = transform;
        player = GameObject.FindGameObjectWithTag("Player");
        parentObj = gameObject.transform.parent;
        fire = transform.GetChild(1).gameObject.GetComponent<ParticleSystem>();
        trail = transform.GetChild(2).gameObject.GetComponent<ParticleSystem>();
        fire.Stop();
        trail.Stop();
        distanceExplosion = 6;
        ctrlAudio = GameObject.FindGameObjectWithTag("CtrlAudio").GetComponent<CtrlAudio>();
    }
	
	// Update is called once per frame
	void Update ()
	{
	    if (trackPlayer && Time.timeScale != 0)
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
        ctrlAudio.playOneSound("Enemies", audioBlast, transform.position, 0.5f, 1f, 70, false, null, 50f, 0f, AudioRolloffMode.Linear);
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
        ctrlAudio.stopSound(idAudioMisile);
        ctrlAudio.playOneSound("Enemies", audioExplode, transform.position, 0.5f, 1f, 70, false, null, 30f, 0f, AudioRolloffMode.Linear);
		Instantiate (explosionEffect, transform.position, Quaternion.identity);
        RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer
        //Debug.DrawRay(transform.position, player.transform.position - transform.position, Color.green);

        if (Physics.Raycast(transform.position, player.transform.position - transform.position, out hit, distanceExplosion))
        {
            if (hit.transform.tag == "Player")
            {
                int value = (int) Math.Abs(Vector3.Distance(player.transform.position, transform.position));
                player.GetComponent<PlayerHealth>().ChangeHealth((value-distanceExplosion)*3);
            }
        }
        //Iff colision player
        trackPlayer = false;
        if (parentObj != null)
        {
            gameObject.transform.parent = parentObj;
        }
        else
        {
            Destroy(gameObject);
        }
        fire.Stop();
        trail.Stop();
        transform.position = originalTransform.position;
        transform.rotation = originalTransform.rotation;
        GetComponent<CapsuleCollider>().enabled = false;
        GetComponent<Animator>().Play("Nothing");
        GetComponent<Animator>().enabled = true;
        aiSnitch.misileFree(gameObject);
    }

    public void finishBlast()
    {
        position = transform.position;
        transform.localScale = new Vector3(2f, 2f, 3f);
        GetComponent<CapsuleCollider>().enabled = true;
        speed = transform.GetChild(0).transform.position - transform.position;
        speed = Vector3.ClampMagnitude(speed, 0.1f);
        acceleration = Vector3.zero;
        trackPlayer = true;
        GetComponent<Animator>().enabled = false;
        gameObject.transform.parent = null;
    }
    public void startBlast()
    {
       idAudioMisile = ctrlAudio.playOneSound("Enemies", audioMisile, transform.position, 0.3f, 1f, 70, true, gameObject, 30f, 0f, AudioRolloffMode.Linear);
       fire.Play();
       trail.Play();
    }
}
