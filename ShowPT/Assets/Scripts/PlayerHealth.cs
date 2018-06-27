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

    // Use this for initialization
    void Start ()
    {
        health = maxHealth;
        ctrlGameState = ctrlGame.GetComponent<CtrlGameState>();
    }

	void Update()
	{
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

    //TODO: Final version projectiles are going to function with raycasts, so chances are the following method will completely change
    void OnTriggerEnter(Collider collider)
    {
		Projectile bullet = collider.GetComponent<Projectile> ();
        if (bullet != null && bullet.layerMask == LayerMask.NameToLayer("EnemiesProjectiles"))
        {
			ChangeHealth(-bullet.damage);
            Destroy(collider.gameObject);
        }
    }

    public void ChangeHealth(int value)
    {
		if (value > 0 || damageable == true)
		{
        	health += value;
        	hudController.ChangeHealthBar(health);
		}

		if (value < 0) 
		{
			damageOverlay.damageFlash ();
			damageable = false;
		}

        if (health > maxHealth)
        {
            health = maxHealth;
            hudController.ChangeHealthBar(health);
        }
        else if (health <= 0)
        {
            //TODO: Call for Game Over
            ctrlGameState.setGameState(CtrlGameState.gameStates.DEATH);
        }
    }
}
