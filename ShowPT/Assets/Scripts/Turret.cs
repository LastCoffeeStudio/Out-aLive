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
	[SerializeField]
	AITurret2 myAI;

    [Header("Shoot Info")]
	/*public float timeShooting = 3.0f;
    public float timeNoShooting = 1.0f;

    public float minDistToAtack = 10;
    private float shootTimerTurret = 0.0f;*/
	public int shootDamage = 1;
    private bool particlesInited = false;
    private bool electrified;
	[SerializeField]
	LayerMask viewMask;

	GameObject player;
	GameObject playerHead;

    [Header("Sounds")]
    public AudioClip sparks;
    public AudioClip laserAttak;
    private ulong idSparks;
    private ulong idLaserAttak;
    private bool soundAttackingActive = false;
    private GameObject soundParticle;
    private bool soundPlaying = false;

    private void Start()
    {
        ctrAudio = GameObject.FindGameObjectWithTag("CtrlAudio").GetComponent<CtrlAudio>();
        cameraShake = GameObject.FindGameObjectWithTag("CameraShake").GetComponent<CameraShake>();
        if (player == null) 
		{
			player = GameObject.FindGameObjectWithTag ("Player");
		}
		if (playerHead == null) 
		{
			playerHead = GameObject.FindGameObjectWithTag ("PlayerHead");
		}
        electrified = false;
        soundParticle = new GameObject();
        soundParticle.name = "soundParticle";
        soundParticle.active = false;
    }

    // Update is called once per frame
	private void Update()
    {
		if (myAI.shooting == true)
		{
		    soundPlaying = true;
            shoot ();
		}
		else if(soundPlaying == true)
		{
		    soundAttackingActive = false;
		    soundParticle.active = false;
		    ctrAudio.stopSound(idSparks);
		    ctrAudio.stopSound(idLaserAttak);
		    laserEffect.SetActive(false);
		    soundPlaying = false;
		}
        checkStatus();
    }

    public override void shoot()
    {
        //shootTimerTurret += Time.deltaTime;
        float distWithPlayer = Vector3.Distance(player.transform.position, transform.position);


        /*if (distWithPlayer > minDistToAtack)
        {
            shootTimerTurret = 0.0f;
            soundParticle.active = false;
            ctrAudio.stopSound(idSparks);
            ctrAudio.stopSound(idLaserAttak);
            soundAttackingActive = false;
            laserEffect.SetActive(false);
            particlesInited = false;
        }
        else if (shootTimerTurret >= timeNoShooting && !electrified)
        {*/
			//Vector3 directionToPlayer = (player.transform.position - transform.position).normalized;
			if (!Physics.Linecast (shotPoint.transform.position, player.transform.position, viewMask) || !Physics.Linecast (shotPoint.transform.position, playerHead.transform.position, viewMask)) 
			{
			    if (soundAttackingActive == false)
			    {
			        idLaserAttak = ctrAudio.playOneSound("Enemies", laserAttak, transform.position, 0.3f, 1.0f, 60, true, null, 25f, 0f, AudioRolloffMode.Linear);
			        soundAttackingActive = true;
			    }
                laserEffect.SetActive (true);
				if (!particlesInited) {
					particlesInitial.Play ();
					particlesInited = true;
				}

				RaycastHit hit;
				int layerMask = (1 << 8) | (1 << 24);
				if (Physics.Raycast (shotPoint.position, shotPoint.transform.forward, out hit, 100f, layerMask)) {
				    if (soundParticle.active == false)
				    {
				        soundParticle.active = true;
				        idSparks = ctrAudio.playOneSound("Enemies", sparks, soundParticle.transform.position, 0.3f, 1.0f, 60, true, soundParticle, 25f, 0f, AudioRolloffMode.Linear);
                    }
					lineRenderer.SetPosition (0, shotPoint.position);
					lineRenderer.SetPosition (1, hit.point);

				    soundParticle.transform.position = hit.point;

                    particlesCollision.transform.rotation = Quaternion.LookRotation (hit.normal, hit.transform.up);
					particlesCollision.transform.position = hit.point;

					switch (hit.transform.gameObject.tag) {
					case "Player":
						PlayerHealth playerTemp = hit.transform.gameObject.GetComponent<PlayerHealth> ();
						playerTemp.ChangeHealth (-shootDamage);
						break;
					}
				}

				/*if (shootTimerTurret >= timeShooting + timeNoShooting) {
					shootTimerTurret = 0.0f;
				    soundAttackingActive = false;
				    soundParticle.active = false;
				    ctrAudio.stopSound(idSparks);
                    ctrAudio.stopSound(idLaserAttak);
                    laserEffect.SetActive (false);
					particlesInited = false;
				}
			} 
			/*else 
			{
				shootTimerTurret = 0.0f;
			    soundParticle.active = false;
			    ctrAudio.stopSound(idSparks);
                soundAttackingActive = false;
                ctrAudio.stopSound(idLaserAttak);
                laserEffect.SetActive(false);
				particlesInited = false;
			}*/
        }
    }

    public override float getHit(int damage)
    {
        ctrAudio.playOneSound("Enemies", hitAudio, transform.position, 1.0f, 0.0f, 128);
        enemyHealth -= damage;
        //Execute properly Animation
        checkHealth();
        return enemyHealth;
    }

    public override void checkHealth()
    {
        if (enemyHealth <= 0f)
        {
            ctrAudio.stopSound(idLaserAttak);
            ctrAudio.stopSound(idSparks);
            soundParticle.active = false;
            //Camera Shake
            float playerDistance = Vector3.Distance(transform.position, player.transform.position);
            cameraShake.startShake(shakeTime, fadeInTime, fadeOutTime, speed, (magnitude * (1 - Mathf.Clamp01(playerDistance / maxDistancePlayer))));

            generateDeathEffect ();
            ScoreController.addDead(ScoreController.Enemy.TURRET);
        }
    }
    /*public override void setStatusParalyzed()
    {
        if (status == Status.NONE && !electrified)
        {
            //Debug.Log("a");
            shootTimerTurret = 0.0f;
            soundParticle.active = false;
            ctrAudio.stopSound(idSparks);
            soundAttackingActive = false;
            ctrAudio.stopSound(idLaserAttak);
            laserEffect.SetActive(false);
            particlesInited = false;
            electrified = true;
            status = Status.PARALYZED;
        }
    }*/

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

    private void OnDestroy()
    {
        ctrAudio.stopSound(idLaserAttak);
        ctrAudio.stopSound(idSparks);
        if (soundParticle != null)
        {
            soundParticle.active = false;
        }
    }
}
