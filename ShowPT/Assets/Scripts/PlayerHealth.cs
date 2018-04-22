using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour {

    public int maxHealth = 100;
    public int health;
    public HudController hudUI;
    public GameObject ctrlGame;
    private CtrlGameState ctrlGameState;

    // Use this for initialization
    void Start ()
    {
        health = maxHealth;
        ctrlGameState = ctrlGame.GetComponent<CtrlGameState>();
    }

    //TODO: Final version projectiles are going to function with raycasts, so chances are the following method will completely change
    void OnTriggerEnter(Collider collider)
    {
		Projectile bullet = collider.GetComponent<Projectile> ();
        if (bullet != null)
        {
			ChangeHealth(-bullet.damage);
            Destroy(collider.gameObject);
        }
    }

    public void ChangeHealth(int value)
    {
        health += value;
        hudUI.ChangeHealthBar(health);

        if (health > maxHealth)
        {
            health = maxHealth;
        }
        else if (health <= 0)
        {
            //TODO: Call for Game Over
            ctrlGameState.setGameState(CtrlGameState.gameStates.DEATH);
        }
    }
}
