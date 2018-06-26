using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : Enemy
{
    [Header("Shoot Components")]
    public GameObject laserEffect;
    public LineRenderer lineRenderer;
    public ParticleSystem particlesInitial;
    public ParticleSystem particlesCollision;

    [Header("Shoot Info")]
    public float timeShooting = 3.0f;
    public float timeNoShooting = 1.0f;
    public int shootDamage = 1;
    public float minDistToAtack = 10;
    private float shootTimerTurret = 0.0f;
    private bool particlesInited = false;
    private GameObject player;
    private bool electrified;

    private void Start()
    {
        ctrAudio = GameObject.FindGameObjectWithTag("CtrlAudio").GetComponent<CtrlAudio>();
        player = GameObject.FindGameObjectWithTag("Player");
        hitAudio = ctrAudio.hit;
        electrified = false;
    }

    // Update is called once per frame
	private void Update()
    {
        shoot();
        checkStatus();
    }

    public override void shoot()
    {
        shootTimerTurret += Time.deltaTime;
        float distWithPlayer = Vector3.Distance(player.transform.position, transform.position);

        if (distWithPlayer > minDistToAtack)
        {
            shootTimerTurret = 0.0f;
            laserEffect.SetActive(false);
            particlesInited = false;
        }

        else if (shootTimerTurret >= timeNoShooting && !electrified)
        {
            laserEffect.SetActive(true);
            if (!particlesInited)
            {
                particlesInitial.Play();
                particlesInited = true;
            }

            RaycastHit hit;
            if (Physics.Raycast(shotPoint.position, shotPoint.transform.forward, out hit))
            {
                lineRenderer.SetPosition(0, shotPoint.position);
                lineRenderer.SetPosition(1, hit.point);

                particlesCollision.transform.rotation = Quaternion.LookRotation(hit.normal, hit.transform.up);
                particlesCollision.transform.position = hit.point;

                switch (hit.transform.gameObject.tag)
                {
                    case "Player":
                        PlayerHealth player = hit.transform.gameObject.GetComponent<PlayerHealth>();
                        player.ChangeHealth(-shootDamage);
                        break;
                }
            }

            if (shootTimerTurret >= timeShooting + timeNoShooting)
            {
                shootTimerTurret = 0.0f;
                laserEffect.SetActive(false);
                particlesInited = false;
            }
        }
    }

    public override void getHit(int damage)
    {
        ctrAudio.playOneSound("Enemies", hitAudio, transform.position, 1.0f, 0.0f, 128);
        enemyHealth -= damage;
        //Execute properly Animation
        base.checkHealth();
    }

    public override void checkHealth()
    {
        if (enemyHealth <= 0f)
        {
            Destroy(gameObject);
            ScoreController.addDead(ScoreController.Enemy.TURRET);
        }
    }
    public override void setStatusParalyzed()
    {
        if (status == Status.NONE && !electrified)
        {
            //Debug.Log("a");
            shootTimerTurret = 0.0f;
            laserEffect.SetActive(false);
            particlesInited = false;
            electrified = true;
            status = Status.PARALYZED;
        }
    }

    private void checkStatus()
    {
        if (electrified)
        {
            paralyzedActualTime -= Time.deltaTime;
            if (paralyzedActualTime <= 0f)
            {
                //Debug.Log("b");
                status = Status.NONE;
                electrified = false;
                paralyzedActualTime = paralyzedTotalTime;
            }
        }
    }
}
