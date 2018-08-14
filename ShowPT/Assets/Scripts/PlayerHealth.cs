using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour {

    public int maxHealth = 100;
    public int health;
    public  GameObject ctrlGame;
    private CtrlGameState ctrlGameState;

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
	GameObject playerCamera;
	Animator deathAnimation;

    // Use this for initialization
    void Start ()
    {
        health = maxHealth;
        ctrlGameState = ctrlGame.GetComponent<CtrlGameState>();
		deathAnimation = playerCamera.GetComponent<Animator> ();
    }

	void Update()
	{
		if (isDead == true) 
		{
			gameOverTimer += Time.deltaTime;
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
        	hudController.ChangeHealthBar(health);
		}

		if (damageable == true) 
		{
			if (value < 0) 
			{
				damageOverlay.damageFlash (health <= 0);
				ScoreController.addLoseLife (-value);
				damageable = false;
			}

			if (health > maxHealth) 
			{
				health = maxHealth;
				hudController.ChangeHealthBar (health);
			} 
			else if (health <= 0) 
			{
				//Player DIES; Start dying sequence
				PlayerMovment.overrideControls = true;
				deathAnimation.SetTrigger ("playerDied");
				isDead = true;
			}
		}
    }

    public void buyHealth()
    {
        ChangeHealth(maxHealth);
    }
}
