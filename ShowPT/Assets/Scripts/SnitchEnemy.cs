using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnitchEnemy : Enemy {
    
    public Transform player;

	[SerializeField]
	GameObject bridge;

    [Header("Sounds")]
    public AudioClip snichSound;
    private ulong idSnichSound;

    // Use this for initialization
    private void Start()
    {
        
        player = GameObject.FindGameObjectWithTag("Player").transform;
        ctrAudio = GameObject.FindGameObjectWithTag("CtrlAudio").GetComponent<CtrlAudio>();
        idSnichSound = ctrAudio.playOneSound("Enemies", snichSound, transform.position, 0.3f, 1f, 90, true, gameObject, 30, 0f,
            AudioRolloffMode.Linear);

    }

    // Update is called once per frame
    private void Update()
    {
        if (player != null)
        {
            checkPlayerDistance();
        }

    }

    public override float getHit(int damage)
    {
        enemyHealth -= damage;
        checkHealth();
        return enemyHealth;
    }

    private void checkPlayerDistance()
    {

    }

    

    public override void checkHealth()
    {
        if (enemyHealth <= 0f)
        {
            ctrAudio.stopSound(idSnichSound);
            ScoreController.addDead(ScoreController.Enemy.DRON);
			bridge.SetActive (true);
			generateDeathEffect ();
        }
    }
    
}
