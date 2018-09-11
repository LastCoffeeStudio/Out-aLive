using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public enum EnemyType
    {
        ALL = -1,
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
    Projectile projectileToShoot;
    [SerializeField]
    float timeBetweenShots = 0.5f;
    float shootTimer = 0.0f;

	[Header("General Audio")]
	[SerializeField]
	public AudioClip hitAudio;
	[SerializeField]
	public CtrlAudio ctrAudio;

    public Transform shotPoint;

    [SerializeField]
    public float enemyHealth = 10f;
    protected EnemyType enemyType = EnemyType.NONE;

	[Header("Death Config")]
	[SerializeField]
	GameObject deathAnimation;
	[SerializeField]
	AudioClip deathAudio;

    [Header("Status parameters")]
    public float paralyzedTotalTime;
    protected float paralyzedActualTime;
    protected Status status = Status.NONE;

    [Header("Player Shake Settings")]
    [SerializeField]
    protected float maxDistancePlayer;
    [SerializeField]
    protected float shakeTime;
    [SerializeField]
    protected float fadeInTime;
    [SerializeField]
    protected float fadeOutTime;
    [SerializeField]
    protected float speed;
    [SerializeField]
    protected float magnitude;

    protected CameraShake cameraShake;

	void Start()
	{
		ctrAudio = GameObject.FindGameObjectWithTag("CtrlAudio").GetComponent<CtrlAudio>();
	}

    public virtual float getHit(int damage) 
	{
		enemyHealth -= damage;
		ctrAudio.playOneSound("Enemies", hitAudio, transform.position, 0.5f, 0.0f, 128);
		checkHealth ();
        return enemyHealth;
	}


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
			generateDeathEffect ();
            ScoreController.addDead(GetScoreControllerEnemyByName());
            //Destroy(gameObject);
        }
    }

	protected virtual void generateDeathEffect()
	{
		if (ctrAudio != null) 
		{
			ctrAudio.playOneSound("Enemies", deathAudio, transform.position, 1.0f, 1f, 128);
		}
		if (deathAnimation != null) 
		{
			deathAnimation.transform.position = transform.position;
			deathAnimation.transform.rotation = transform.rotation;
			deathAnimation.SetActive (true);
		}
		gameObject.SetActive (false);
	}

    public virtual void setStatusParalyzed() {}

    public bool isDeath()
    {
        return enemyHealth <= 0f;
    }

    private ScoreController.Enemy GetScoreControllerEnemyByName()
    {
        if (this.name == "TV") return ScoreController.Enemy.TV;
        else if(this.name == "Camera Head") return ScoreController.Enemy.CAMERA;
        else return ScoreController.Enemy.UNDEFINED;
    }
}
