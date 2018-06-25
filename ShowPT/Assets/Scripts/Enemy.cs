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

    public enum Status
    {
        NONE,
        PARALYZED,

        NUM_STATUS
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
    public float enemyHealth = 10f;
    protected EnemyType enemyType = EnemyType.NONE;

	[SerializeField]
	GameObject deathAnimation;
	[SerializeField]
	AudioClip deathAudio;

    [Header("Status parameters")]
    public float paralyzedTotalTime;
    protected float paralyzedActualTime;
    protected Status status = Status.NONE;

    public virtual void getHit(int damage) {}

    public virtual void shoot()
    {
        if (active)
        {
            shootTimer += Time.deltaTime;
            if (shootTimer >= timeBetweenShots)
            {
                Instantiate(projectileToShoot, shotPoint.position, shotPoint.rotation);
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

	protected virtual void generateDeathEffect()
	{
		if (ctrAudio != null) 
		{
			ctrAudio.playOneSound("Enemies", deathAudio, transform.position, 1.0f, 0.0f, 128);
		}
		if (deathAnimation != null) 
		{
			GameObject.Instantiate (deathAnimation, transform.position, Quaternion.identity);
		}
	}

    public virtual void setStatusParalyzed() {}

    public bool isDeath()
    {
        return enemyHealth <= 0f;
    }
}
