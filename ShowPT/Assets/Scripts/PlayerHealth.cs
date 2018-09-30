using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour {

    public int maxHealth = 100;
    public int health;
    public  GameObject ctrlGame;
    private CtrlGameState ctrlGameState;
	TweetSystem tweetSystem;

    public HudController hudController;

	[SerializeField]
	DamageFlash damageOverlay;

	bool damageable = true;
	[SerializeField]
	float invincibilityTime = 0.5f;
	float invincibilityCounter = 0f;

	bool isDead = false;
	[SerializeField]
	float secondsBeforeGameOver = 4f;
	float gameOverTimer = 0f;

	[SerializeField]
	Transform playerCamera;
	[SerializeField]
	Transform deathPose;
	Animator deathAnimation;
	[SerializeField]
	Animator gun;
	[SerializeField]
	Animator shotgun;
	[SerializeField]
	Animator cannon;

    [Header("Sounds")]
    private CtrlAudio ctrlAudio;
    public AudioCollection damageCollection;
    public AudioCollection tvCollection;
    private ulong idClip = 0;
    private bool playerDamagedFirstTimeLil;
    private bool playerDamagedFirstTimeTurret;

	[Header("Health Tweets")]
	[SerializeField]
	Tweet damagedTweet;
	[SerializeField]
	Tweet halfLifeTweet;
	[SerializeField]
	Tweet almostDeadTweet;

    // Use this for initialization
    void Start ()
    {
        health = maxHealth;
        ctrlGameState = ctrlGame.GetComponent<CtrlGameState>();
		deathAnimation = playerCamera.GetComponent<Animator> ();
        ctrlAudio = GameObject.FindGameObjectWithTag("CtrlAudio").GetComponent<CtrlAudio>();
		tweetSystem = FindObjectOfType<TweetSystem> ();
        playerDamagedFirstTimeLil = false;
        playerDamagedFirstTimeTurret = false;
    }

	void Update()
	{
		if (isDead == true) 
		{
			gameOverTimer += Time.deltaTime;
			playerCamera.rotation = Quaternion.Lerp (playerCamera.rotation, deathPose.rotation, Time.deltaTime);
			if (gameOverTimer >= secondsBeforeGameOver) 
			{
				ctrlGameState.setGameState (CtrlGameState.gameStates.DEATH);
            }
		} 
		else 
		{
			//Manage invincibility timer
			if (damageable == false) 
			{
				invincibilityCounter += Time.deltaTime;
				if (invincibilityCounter >= invincibilityTime) 
				{
					invincibilityCounter = 0f;
					damageable = true;
				}
			}
		}
	}

    void OnTriggerEnter(Collider collider)
    {
		Projectile bullet = collider.GetComponent<Projectile> ();
        if (bullet != null && bullet.layerMask == LayerMask.NameToLayer("EnemiesProjectiles"))
        {
			ChangeHealth(-bullet.damage);
            Destroy(collider.gameObject);
        }
    }

    public int GetHealth()
    {
        return health;
    }

    public void ChangeHealth(int value)
    {
		if (value > 0 || damageable == true)
		{
        	health += value;
			if (health < 0) 
			{
				health = 0;
			}
        	hudController.ChangeHealthBar(health);
		}

		if (damageable == true) 
		{
			if (value < 0) 
			{
				damageOverlay.damageFlash (health <= 0);
				ScoreController.addLoseLife (-value);
			    AudioSource audioSource = ctrlAudio.getSoundAudiSource(idClip);
			    if (audioSource == null)
			    {
			        idClip = ctrlAudio.playOneSound(damageCollection.audioGroup, damageCollection[0], transform.position, damageCollection.volume, damageCollection.spatialBlend, damageCollection.priority);
                }
                damageable = false;
				callForDamageTweets ();
			}

			if (health > maxHealth) 
			{
				health = maxHealth;
				hudController.ChangeHealthBar (health);
			} 
			else if (health <= 0) 
			{
                ctrlAudio.playOneSound(tvCollection.audioGroup, tvCollection[(int)GenericEvent.EventType.PLAYERDEAD], Vector3.zero, 0.05f, 0f, tvCollection.priority);
                //Player DIES; Start dying sequence
                disablePlayer();
                isDead = true;
			}
		}
    }

    public void buyHealth()
    {
        ChangeHealth(maxHealth);
    }

    public void hitLil()
    {
        if (!playerDamagedFirstTimeLil)
        {
            playerDamagedFirstTimeLil = true;
            ctrlAudio.playOneSound(tvCollection.audioGroup, tvCollection[(int)GenericEvent.EventType.FIRSTLIL], Vector3.zero, 0.05f, 0f, tvCollection.priority);
        }
    }

    public void hitTurret()
    {
        if (!playerDamagedFirstTimeTurret)
        {
            playerDamagedFirstTimeTurret = true;
            ctrlAudio.playOneSound(tvCollection.audioGroup, tvCollection[(int)GenericEvent.EventType.FIRSTORRET], Vector3.zero, 0.05f, 0f, tvCollection.priority);
        }
    }

    public void disablePlayer()
    {
        tweetSystem.deactivateSystem();
        PlayerMovment.overrideControls = true;
        if (gun.isActiveAndEnabled)
            gun.SetTrigger("playerDead");
        if (shotgun.isActiveAndEnabled)
            shotgun.SetTrigger("playerDead");
        if (cannon.isActiveAndEnabled)
            cannon.SetTrigger("playerDead");
    }

	void callForDamageTweets()
	{
		if (health <= (maxHealth / 4) * 3 && damagedTweet != null) 
		{
			tweetSystem.requestTweet (damagedTweet);
			damagedTweet = null;
		} 
		else if (health <= maxHealth / 2 && halfLifeTweet != null) 
		{
			tweetSystem.requestTweet (halfLifeTweet);
			halfLifeTweet = null;
		} 
		else if (health <= maxHealth / 4 && almostDeadTweet != null) 
		{
			tweetSystem.requestTweet (almostDeadTweet);
			almostDeadTweet = null;
		}
	}
}
