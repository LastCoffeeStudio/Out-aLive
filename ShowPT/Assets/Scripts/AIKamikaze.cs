using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIKamikaze : MonoBehaviour {

	public enum state 
	{
		WALKING,
		WAITING,
		I_SEE_YOU,
		I_HEAR_YOU,
		ALERT
	}

	[SerializeField]
	float waitTime = 3.0f;

	[SerializeField]
	float switchProbability = 0.5f;

	[SerializeField]
	List<Waypoint> nodeList;

	[SerializeField]
	float viewDistance = 50.0f;

	[SerializeField]
	float explodingDistance = 0.1f;

	[SerializeField]
	float alertTime = 6.0f;

	[SerializeField]
	float alertRotationTime = 1.0f;

	Light spotLight;
	float viewAngle;
	private GameObject player;
    private PlayerMovment playerMovment;

	[SerializeField]
	LayerMask viewMask;

	NavMeshAgent navMeshAgent;
	int nodeIndex;
	bool patrolForward = true;
	float waitTimer;
	float alertTimer;
	float alertRotationTimer;
	public float alertRotation = -2f;
	Vector3 aggressiveDestination;

	state NPCstate;


	// Use this for initialization
	void Start () {
		navMeshAgent = this.GetComponent<NavMeshAgent> ();

		if (navMeshAgent == null) 
		{
			Debug.LogError ("Nav mesh agent not found on object " + gameObject.name);
		} 
		else 
		{
			if (nodeList != null && nodeList.Count >= 2) 
			{
				nodeIndex = 0;
				SetDestination ();
				NPCstate = state.WALKING;
			} 
			else 
			{
				Debug.LogError ("Not enough nodes for the object " + gameObject.name + " to patrol at all.");
			}
		}

		spotLight = gameObject.GetComponentInChildren<Light> ();
		viewAngle = spotLight.spotAngle / 2;
		player = GameObject.FindGameObjectWithTag("Player");
	    playerMovment = player.GetComponent<PlayerMovment>();

	}

	// Update is called once per frame
	void Update () 
	{
		switch (NPCstate) 
		{
		case state.WALKING:
			spotLight.color = Color.green;
			if (navMeshAgent.remainingDistance <= 1.0f) 
			{
				waitTimer = 0.0f;
				NPCstate = state.WAITING;
			}
			break;

		case state.WAITING:
			spotLight.color = Color.green;
			waitTimer += Time.deltaTime;
			if (waitTimer >= waitTime) 
			{
				ChangePatrolNode ();
				SetDestination ();
				NPCstate = state.WALKING;
			}
			break;

		case state.I_SEE_YOU:
			LookAtSomething (aggressiveDestination);
			spotLight.color = Color.red;
			navMeshAgent.SetDestination (aggressiveDestination);

			//Should I explode?
			if (Vector3.Distance (transform.position, player.transform.position) < explodingDistance && CanSeePlayer ()) 
			{
				//TODO: Generate explosion
				Destroy (gameObject);
			}

			//Did I lose track of the objective?
			if (Vector3.Distance (transform.position, player.transform.position) <= 1.0f && !CanSeePlayer()) 
			{
				alertTimer = 0f;
				alertRotationTimer = 0f;
				LookAtSomething (aggressiveDestination);
				NPCstate = state.ALERT;
			}
			break;

		case state.I_HEAR_YOU:
			spotLight.color = Color.yellow;
			if (navMeshAgent.remainingDistance <= 1.0f && !CanSeePlayer()) 
			{
				alertTimer = 0f;
				alertRotationTimer = 0f;
				NPCstate = state.ALERT;
			}
			break;

		case state.ALERT:
			spotLight.color = Color.yellow;
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
				SetDestination ();
				NPCstate = state.WALKING;
			}
			break;
		}

		//These two will always happen, no matter the state
		if (CanHearPlayer ()) 
		{
			aggressiveDestination = player.transform.position;
			navMeshAgent.SetDestination (aggressiveDestination);
			NPCstate = state.I_HEAR_YOU;
		}
		if (CanSeePlayer ()) 
		{
			aggressiveDestination = player.transform.position;
			NPCstate = state.I_SEE_YOU;
		}

	}

	bool CanSeePlayer()
	{
		if (Vector3.Distance (transform.position, player.transform.position) < viewDistance) 
		{
			Vector3 directionToPlayer = (player.transform.position - transform.position).normalized;
			float angleBetweenThisAndPlayer = Vector3.Angle (transform.forward, directionToPlayer);
			if (angleBetweenThisAndPlayer < viewAngle) 
			{
				if(!Physics.Linecast(transform.position, player.transform.position, viewMask))
				{
					return true;
				}
			}
		}
		return false;
	}

	bool CanHearPlayer(){
		if (playerMovment.noiseValue > Vector3.Distance (transform.position, player.transform.position)) 
		{
			return true;
		}
		return false;
	}

	private void SetDestination()
	{
		Vector3 targetPosition = nodeList [nodeIndex].transform.position;
		navMeshAgent.SetDestination (targetPosition);
	}

	private void ChangePatrolNode()
	{
		if (UnityEngine.Random.Range (0.0f, 1.0f) <= switchProbability) 
		{
			patrolForward = !patrolForward;
		}

		if (patrolForward) 
		{
			nodeIndex = (nodeIndex + 1) % nodeList.Count;
		} 
		else 
		{
			if (--nodeIndex < 0) 
			{
				nodeIndex = nodeList.Count - 1;
			}
		}
	}

	void LookAtSomething(Vector3 something)
	{
		var lookPos = something - gameObject.transform.position;
		transform.rotation = Quaternion.Slerp (gameObject.transform.rotation, Quaternion.LookRotation (lookPos), Time.deltaTime * 6);
	}

	void OnDrawGizmos()
	{
		Gizmos.color = Color.white;
		Gizmos.DrawRay (transform.position, transform.forward * viewDistance);
	}
}
