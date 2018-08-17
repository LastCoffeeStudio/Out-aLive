using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnitchEnemy : Enemy {
    
    public Transform player;

	[SerializeField]
	GameObject bridge;

    // Use this for initialization
    private void Start()
    {
        
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
            ScoreController.addDead(ScoreController.Enemy.DRON);
			bridge.SetActive (true);
            Destroy(transform.parent.gameObject);
        }
    }
    
}
