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
	CtrlAudio ctrAudio;
	public Rigidbody myRigidBody;
	public bool activable = true;
	bool immaExplodeNow = false;
	float explosionTimer = 0f;

	//Barrel[] allBarrels;

	void Start() 
	{
		myRigidBody = gameObject.GetComponent<Rigidbody>();
		player = GameObject.FindGameObjectWithTag ("Player").GetComponent<Transform>();
		ctrAudio = GameObject.FindGameObjectWithTag("CtrlAudio").GetComponent<CtrlAudio>();

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
		if (Physics.Raycast(transform.position, player.transform.position - transform.position, out hitInfo, explosionDistance) && hitInfo.transform.tag == "Player")
		{
			player.GetComponent<PlayerHealth>().ChangeHealth(-explosionDamage);
		}

		int layerMask = 1 << LayerMask.NameToLayer ("PhysicsObjects");
		Collider[] hitColliders = Physics.OverlapSphere(transform.position, explosionDistance, layerMask);
		int i = 0;
		while (i < hitColliders.Length)
		{
			Debug.Log (hitColliders [i].name);
			Vector4 dataToPass = new Vector4(transform.position.x, transform.position.y, transform.position.z, explosionDamage);
			hitColliders[i].SendMessage ("shotBehavior", dataToPass);
			i++;
		}
		/*foreach (Barrel barrel in allBarrels) {
			if (barrel.activable == true && Vector3.Distance (transform.position, barrel.transform.position) < explosionDistance)
			{
				if (Physics.Raycast (transform.position, barrel.transform.position - transform.position, out hitInfo, explosionDistance)) {
					
					barrel.myRigidBody.AddTorque (new Vector3 (rotationWhenBlasted, rotationWhenBlasted, rotationWhenBlasted));
					barrel.shotBehavior (hitInfo.point, explosionDamage / 3);
					//barrel.myRigidBody.AddForce (Vector3.up * explosionDamage);
				}
			}
		}*/

		Destroy(gameObject);
	}

	/*void OnDrawGizmos()
	{
		Gizmos.color = Color.black;
		Gizmos.DrawWireSphere (transform.position, explosionDistance);
	}*/
}
