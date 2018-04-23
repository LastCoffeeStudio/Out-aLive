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

    public bool active = false;
    [SerializeField]
    public AudioClip hitAudio;
    [SerializeField]
    public CtrlAudio ctrAudio;
    [SerializeField]
    Projectile projectileToShoot;
    [SerializeField]
    float timeBetweenShots = 0.5f;
    float shootTimer = 0.0f;

    public Transform shotPoint;

    [SerializeField]
    protected float enemyHealth = 10f;
    protected EnemyType enemyType = EnemyType.NONE;
    

    // Use this for initialization
    void Start()
    {
       
    }
	
	// Update is called once per frame
	void Update () {}

    public virtual void getHit(int damage) {}

    public virtual void shoot()
    {
        if (active)
        {
            shootTimer += Time.deltaTime;
            if (shootTimer >= timeBetweenShots)
            {
                GameObject.Instantiate(projectileToShoot, shotPoint.position, shotPoint.rotation);
                shootTimer = 0.0f;
            }
        }
    }

    public virtual void checkHealth()
    {
        if (enemyHealth <= 0f)
        {
            Destroy(gameObject);
        }
    }
}
