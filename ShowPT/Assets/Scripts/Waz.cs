using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waz : Enemy
{
    // Use this for initialization
    void Start()
    {
        ctrAudio = GameObject.FindGameObjectWithTag("CtrlAudio").GetComponent<CtrlAudio>();
        hitAudio = ctrAudio.hit;
    }

    // Update is called once per frame
    void Update()
    {
        shoot();
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
            Destroy(gameObject);
            ScoreController.addDead(ScoreController.Enemy.WAZ);
        }
    }
}
