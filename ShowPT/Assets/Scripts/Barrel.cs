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
	float floatiness = 400f;

	[SerializeField]
	float rotationWhenBlasted = 60f;

	[SerializeField]
	GameObject explosion;

	[SerializeField]
	float timeBeforeExploding = 0.5f;

	[SerializeField]
	int explosionDamage = 10;

	[SerializeField]
	float explosionDistance = 20f;

	Transform player;
	CtrlAudio ctrAudio;
	public Rigidbody myRigidBody;
	public bool activable = true;

	Barrel[] allBarrels;

	void Start() 
	{
		myRigidBody = gameObject.GetComponent<Rigidbody>();
		player = GameObject.FindGameObjectWithTag ("Player").GetComponent<Transform>();
		ctrAudio = GameObject.FindGameObjectWithTag("CtrlAudio").GetComponent<CtrlAudio>();

		allBarrels = FindObjectsOfType<Barrel> ();
	}

	public void shotBehavior(Vector3 hitPoint, int damage)
	{
		switch (barrelType) 
		{
		case typeOfBarrel.normal:
			myRigidBody.AddExplosionForce (floatiness * damage, hitPoint, 100f);
			break;
		case typeOfBarrel.explosive:
			StartCoroutine (explode ());
			//explode ();
			break;
		default:
			break;
		}
	}

	IEnumerator explode()
	{
		activable = false;

		yield return new WaitForSeconds (timeBeforeExploding);

		GameObject.Instantiate(explosion, transform.position, Quaternion.identity);

		RaycastHit hitInfo;
		if (Physics.Raycast(transform.position, player.transform.position - transform.position, out hitInfo, explosionDistance) && hitInfo.transform.tag == "Player")
		{
			player.GetComponent<PlayerHealth>().ChangeHealth(-explosionDamage);
		}

		foreach (Barrel barrel in allBarrels) {
			if (barrel.activable == true && Vector3.Distance (transform.position, barrel.transform.position) < explosionDistance)
			{
				if (Physics.Raycast (transform.position, barrel.transform.position - transform.position, out hitInfo, explosionDistance)) {
					
					barrel.myRigidBody.AddTorque (new Vector3 (rotationWhenBlasted, rotationWhenBlasted, rotationWhenBlasted));
					barrel.shotBehavior (hitInfo.point, explosionDamage / 3);
					//barrel.myRigidBody.AddForce (Vector3.up * explosionDamage);
				}
			}
		}

		Destroy(gameObject);
	}

	/*void OnDrawGizmos()
	{
		Gizmos.color = Color.black;
		Gizmos.DrawWireSphere (transform.position, explosionDistance);
	}*/
}
