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
	float rotationSpeed = 1f;

	[SerializeField]
	float burstTime = 2f;
	[SerializeField]
	float coolDownTime = 2f;
	float attackCountdown = 0f;
	bool shooting = false;

	[SerializeField]
	LayerMask viewMask;

	float alertTimer;
	float alertRotationTimer;
	public float alertRotation = -2f;
	public Turret myTurret;

	state NPCstate;

	[SerializeField]
	GameObject shooter;

	[SerializeField]
	GameObject player;

	void Start()
	{
		if (player == null) 
		{
			player = GameObject.FindGameObjectWithTag ("Player");
		}
		if (shooter == null)
		{
			shooter = this.gameObject;
		}
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
			if (!shooting && attackCountdown >= coolDownTime) 
			{
				if (myTurret != null) 
				{
					attackCountdown = 0f;
					shooting = true;
					myTurret.active = true;
				}
			}
			else if (shooting && attackCountdown >= burstTime) 
			{
				if (myTurret != null) 
				{
					attackCountdown = 0f;
					shooting = false;
					myTurret.active = false;
				}
			}

			if (!CanSeePlayer ()) 
			{
				if (myTurret != null) 
				{
					myTurret.active = false;
				}
				attackCountdown = 0f;
				shooting = false;
				NPCstate = state.WAITING;
			}

			attackCountdown += Time.deltaTime;
			break;
		}

		//These two will always happen, no matter the state
		if (CanSeePlayer ()) 
		{
            NPCstate = state.SHOOTING;
		}
	}

	bool CanSeePlayer()
	{
		if (Vector3.Distance (transform.position, player.transform.position) < viewDistance) 
		{
			Vector3 directionToPlayer = (player.transform.position - shooter.transform.position).normalized;
			if(!Physics.Linecast(shooter.transform.position, player.transform.position, viewMask))
			{
				return true;
			}
		}
		return false;
	}

	void LookAtSomething(Vector3 something)
	{
		var lookPos = something - transform.position;
		transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.LookRotation (lookPos), Time.deltaTime * rotationSpeed);
		//shooter.transform.rotation = Quaternion.Slerp (shooter.transform.rotation, Quaternion.LookRotation (lookPos), Time.deltaTime * rotationSpeed);
	}

	void OnDrawGizmos()
	{
		Gizmos.color = Color.white;
		Gizmos.DrawRay (transform.position, transform.forward * viewDistance);
	}
}