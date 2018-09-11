using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kamikaze : Enemy {

    Transform player;
    public float explosionDistance = 20f;
    public int explosionDamage = 3;

	public AudioCollection stepSounds;
	ulong idClip;

	public AudioClip openDoorAudio;

	[SerializeField]
	GameObject explosion;

    // Use this for initialization
    void Start() 
	{
		player = GameObject.FindGameObjectWithTag ("Player").GetComponent<Transform>();
	    ctrAudio = GameObject.FindGameObjectWithTag("CtrlAudio").GetComponent<CtrlAudio>();
        cameraShake = GameObject.FindGameObjectWithTag("CameraShake").GetComponent<CameraShake>();
    }

    public override float getHit(int damage)
    {
        ctrAudio.playOneSound("Enemies", hitAudio, transform.position, 1.0f, 0.0f, 128);
        //hitAudio = ctrAudio.hit;
        enemyHealth -= damage;
        //Debug.Log(enemyHealth);
        /*if (enemyHealth <= 0)
        {
            forceExplode();
        }*/
        checkHealth();
        return enemyHealth;
    }

    public void forceExplode()
    {
        //Explosion animation
        RaycastHit hitInfo;
		if (Vector3.Distance(transform.position, player.transform.position) <= explosionDistance)
        {
            player.GetComponent<PlayerHealth>().ChangeHealth(explosionDamage);
        }
        GameObject.Instantiate(explosion, transform.position, Quaternion.identity);

        //Camera Shake
        float playerDistance = Vector3.Distance(transform.position, player.position);
        cameraShake.startShake(shakeTime, fadeInTime, fadeOutTime, speed, (magnitude * (1 - Mathf.Clamp01(playerDistance / maxDistancePlayer))));

        generateDeathEffect ();
    }

    public override void checkHealth()
    {
        if (enemyHealth <= 0f)
        {
            forceExplode();
            ScoreController.addDead(ScoreController.Enemy.KAMIKAZE);
        }
    }

	public void makeStepSounds()
	{
		//ctrAudio.playOneSound("Weaponds", openDoorAudio, transform.position, 0.5f, 0f, 150);
		idClip = ctrAudio.playOneSound(stepSounds.audioGroup, stepSounds[0], transform.position, stepSounds.volume, stepSounds.spatialBlend, stepSounds.priority);
	}
}
