using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrel : MonoBehaviour {

	enum typeOfBarrel 
	{
		normal,
		explosive
	}

	[SerializeField]
	typeOfBarrel barrelType = typeOfBarrel.normal;

	[SerializeField]
	float floatiness = 50f;

	[SerializeField]
	float rotationWhenBlasted = 180f;

	[SerializeField]
	GameObject explosion;

	[SerializeField]
	float timeBeforeExploding = 0.5f;

	[SerializeField]
	int explosionDamage = 10;

	[SerializeField]
	float explosionDistance = 5f;

	Transform player;
	//CtrlAudio ctrAudio;
	public Rigidbody myRigidBody;
	public bool activable = true;
	bool immaExplodeNow = false;
	float explosionTimer = 0f;

	[SerializeField]
	LayerMask affectedByExplosion;

    [Header("Player Shake Settings")]
    [SerializeField]
    float maxDistancePlayer;
    [SerializeField]
    float shakeTime;
    [SerializeField]
    float fadeInTime;
    [SerializeField]
    float fadeOutTime;
    [SerializeField]
    float speed;
    [SerializeField]
    float magnitude;
    
    CameraShake cameraShake;

	//Barrel[] allBarrels;

	void Start() 
	{
		myRigidBody = gameObject.GetComponent<Rigidbody>();
		player = GameObject.FindGameObjectWithTag ("Player").GetComponent<Transform>();
        cameraShake = GameObject.FindGameObjectWithTag("CameraShake").GetComponent<CameraShake>();
        //ctrAudio = GameObject.FindGameObjectWithTag("CtrlAudio").GetComponent<CtrlAudio>();

        //allBarrels = FindObjectsOfType<Barrel> ();
    }

	void Update()
	{
		if(immaExplodeNow){
			explosionTimer += Time.deltaTime;
			if (explosionTimer >= timeBeforeExploding) 
			{
				explode ();
			}
		}
	}

	public void shotBehavior(Vector4 hitData/*Vector3 hitPoint, int damage*/)
	{
		Vector3 forceOrigin = new Vector3(hitData[0], hitData[1], hitData[2]);
		float hitForce = hitData[3];
			
		switch (barrelType) 
		{
		case typeOfBarrel.normal:
			myRigidBody.AddExplosionForce (floatiness * hitForce, forceOrigin, explosionDistance);
			myRigidBody.AddForce (Vector3.up * hitForce * floatiness);
			float rotation = Random.Range (-rotationWhenBlasted, rotationWhenBlasted);
			myRigidBody.AddTorque (new Vector3 (rotation, rotation, rotation) * hitForce);
			break;
		case typeOfBarrel.explosive:
			activable = false;
			immaExplodeNow = true;
			break;
		default:
			break;
		}
	}

	void explode()
	{
		GameObject.Instantiate(explosion, transform.position, Quaternion.identity);

		RaycastHit hitInfo;
		if (Vector3.Distance(transform.position, player.transform.position) <= explosionDistance)
		{
			player.GetComponent<PlayerHealth>().ChangeHealth(-explosionDamage);
		}
			
		Collider[] hitColliders = Physics.OverlapSphere(transform.position, explosionDistance, affectedByExplosion);
		int i = 0;
		while (i < hitColliders.Length)
		{
			if (hitColliders [i].gameObject.layer == LayerMask.NameToLayer ("PhysicsObjects")) {
				Vector4 dataToPass = new Vector4 (transform.position.x, transform.position.y, transform.position.z, explosionDamage);
				hitColliders [i].SendMessage ("shotBehavior", dataToPass);
			} 
			else if (hitColliders [i].gameObject.layer == LayerMask.NameToLayer ("Enemy"))
			{
				hitColliders [i].SendMessage ("getHit", explosionDamage);
			}
			i++;
		}

        //Camera Shake
        float playerDistance = Vector3.Distance(transform.position, player.position);
        cameraShake.startShake(shakeTime, fadeInTime, fadeOutTime, speed, (magnitude * (1 - Mathf.Clamp01(playerDistance / maxDistancePlayer))));

        Destroy(gameObject);
	}

	/*void OnDrawGizmos()
	{
		Gizmos.color = Color.black;
		Gizmos.DrawWireSphere (transform.position, explosionDistance);
	}*/
}
