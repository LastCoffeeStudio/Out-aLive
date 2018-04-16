using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waz : Enemy
{
    // Use this for initialization
    void Start() {}

    // Update is called once per frame
    void Update()
    {
        shoot();
    }

    public override void getHit()
    {
        --enemyHealth;
        Debug.Log(enemyHealth);
        //Execute properly Animation
        checkHealth();
    }
}
