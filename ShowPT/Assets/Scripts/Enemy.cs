using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    public enum EnemyType
    {
        NONE = 0,

        TURRET,
        LILONE,
        DRONE,
        WAZ,
        KAMIKAZE,

        TOTAL_DRONES
    }

    protected float enemyHealth = 10f;
    protected EnemyType enemyType = EnemyType.NONE;

	// Use this for initialization
	void Start () {}
	
	// Update is called once per frame
	void Update () {}

    public virtual void getHit() {}

    public void checkHealth()
    {
        if (enemyHealth <= 0f)
        {
            Destroy(gameObject);
        }
    }
}
