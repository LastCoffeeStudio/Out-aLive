using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waz : Enemy
{
	Animator wazAnimator;
	public bool imAlreadyDead = false;

    // Use this for initialization
    void Start()
    {
		wazAnimator = gameObject.GetComponent<Animator> ();
        ctrAudio = GameObject.FindGameObjectWithTag("CtrlAudio").GetComponent<CtrlAudio>();
        hitAudio = ctrAudio.hit;
    }

    // Update is called once per frame
    void Update()
    {
        shoot();

		if(imAlreadyDead && wazAnimator.GetCurrentAnimatorStateInfo (0).normalizedTime > 1) 
		{
			Destroy (gameObject);
			ScoreController.addDead (ScoreController.Enemy.WAZ);
		}
    }

    public override void getHit(int damage)
    {
        ctrAudio.playOneSound("Enemies", hitAudio, transform.position, 1.0f, 0.0f, 128);
        enemyHealth -= damage;
        Debug.Log(enemyHealth);
        //Execute properly Animation
        checkHealth();
    }

    public override void checkHealth()
    {
        if (enemyHealth <= 0f)
        {
			if (!imAlreadyDead) {
				wazAnimator.SetTrigger ("dying");
				imAlreadyDead = true;
			} 
        }
    }
}
