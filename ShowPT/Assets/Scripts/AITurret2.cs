using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AITurret2 : MonoBehaviour {

	public enum state 
	{
		WAITING,
		SHOOTING,
		ALERT
	}

	[SerializeField]
	List<Waypoint> nodeList;

	[SerializeField]
	float viewDistance = 50.0f;

	[SerializeField]
	float alertTime = 6.0f;

	[SerializeField]
	float alertRotationTime = 1.0f;

	private GameObject player;

	[SerializeField]
	LayerMask viewMask;

	float alertTimer;
	float alertRotationTimer;
	public float alertRotation = -2f;
	Turret[] myTurrets;

	state NPCstate;


	// Use this for initialization
	void Start () 
	{
		player = GameObject.FindGameObjectWithTag("Player");
		myTurrets = gameObject.GetComponentsInChildren<Turret> ();
	}

	// Update is called once per frame
	void Update () 
	{
		switch (NPCstate) 
		{
		case state.WAITING:
			break;

		case state.SHOOTING:
			LookAtSomething (player.transform.position);
			if (!CanSeePlayer ()) 
			{
				if (myTurrets.Length > 0) 
				{
					for (int i = 0; i <= myTurrets.Length - 1; i++) 
					{
						myTurrets [i].active = false;
					}
				}
				NPCstate = state.ALERT;
			}
			break;

		case state.ALERT:
			Vector3 rotation = new Vector3 (0f, alertRotation, 0f);
			gameObject.transform.Rotate (rotation * Time.deltaTime);
			alertRotationTimer += Time.deltaTime;
			if (alertRotationTimer >= alertRotationTime) 
			{
				alertRotation = alertRotation * (-1);
				alertRotationTimer = 0;
			}

			alertTimer += Time.deltaTime;
			if (alertTimer >= alertTime) 
			{
				NPCstate = state.WAITING;
			}
			break;
		}

		//These two will always happen, no matter the state
		if (CanSeePlayer ()) 
		{
			if (myTurrets.Length > 0) 
			{
				for (int i = 0; i <= myTurrets.Length - 1; i++) 
				{
					myTurrets [i].active = true;
				}
			}
			NPCstate = state.SHOOTING;
		}
	}

	bool CanSeePlayer()
	{
		if (Vector3.Distance (transform.position, player.transform.position) < viewDistance) 
		{
			Vector3 directionToPlayer = (player.transform.position - transform.position).normalized;
			if(!Physics.Linecast(transform.position, player.transform.position, viewMask))
			{
				return true;
			}
		}
		return false;
	}

	void LookAtSomething(Vector3 something)
	{
		var lookPos = something - transform.position /*+ new Vector3(0f, 90f, 0f)*/;
		transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.LookRotation (lookPos), Time.deltaTime * 1);
	}

	void OnDrawGizmos()
	{
		Gizmos.color = Color.white;
		Gizmos.DrawRay (transform.position, transform.forward * viewDistance);
	}
}