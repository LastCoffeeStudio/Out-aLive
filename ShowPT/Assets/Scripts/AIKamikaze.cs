using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIKamikaze : MonoBehaviour {

	public enum state 
	{
		WAITING,
		I_SEE_YOU,
	    JUMPING,
        WALKING

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
	float explodingDistance = 25.0f;

	[SerializeField]
	float alertTime = 6.0f;

	[SerializeField]
	float alertRotationTime = 1.0f;

	
	public float viewAngle;
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

    [SerializeField]
	state NPCstate;
    private Animator animKamikaze;

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
			    NPCstate = state.WAITING;
            }
		}

		viewAngle = 90f;
		player = GameObject.FindGameObjectWithTag("Player");
	    playerMovment = player.GetComponent<PlayerMovment>();
	    animKamikaze = gameObject.GetComponent<Animator>();

	}

	// Update is called once per frame
    void Update()
    {
        aggressiveDestination = player.transform.position;
        switch (NPCstate)
        {

            case state.WAITING:
                if (Vector3.Distance(transform.position, player.transform.position) < viewDistance)
                {
                    NPCstate = state.I_SEE_YOU;
                }
                break;

            case state.I_SEE_YOU:
                animKamikaze.SetBool("Jump", true);
                NPCstate = state.JUMPING;
                break;
            case state.JUMPING:
                if (animKamikaze.GetCurrentAnimatorStateInfo(0).IsName("stopJump"))
                {
                    LookAtSomething(aggressiveDestination);
                }
                if (animKamikaze.GetCurrentAnimatorStateInfo(0).IsName("run"))
                {
                    NPCstate = state.WALKING;
                }
                break;
            case state.WALKING:
                LookAtSomething(aggressiveDestination);
                navMeshAgent.SetDestination(aggressiveDestination);

                Vector3 localDesiredVelocity = transform.InverseTransformVector(navMeshAgent.desiredVelocity);
                float angle = Mathf.Atan2(localDesiredVelocity.x, localDesiredVelocity.z) * Mathf.Rad2Deg;
                float speed = localDesiredVelocity.z;
                /** Linea comentada para evitar warning, descomentar cuando se haya aplicado la animacion al kamikaze**/
                //animKamikaze.SetFloat("Speed", speed, 0.1f, Time.deltaTime);
                
                //Debug.Log(Mathf.Abs(Vector3.Distance(gameObject.transform.position, player.transform.position)) + "// " + explodingDistance);
                //Should I explode?
                if (Mathf.Abs(Vector3.Distance(gameObject.transform.position, player.transform.position)) < explodingDistance )
                {
                    animKamikaze.SetBool("Explode", true);
                }
                if (animKamikaze.GetCurrentAnimatorStateInfo(0).IsName("die"))
                {
                    gameObject.GetComponent<Kamikaze>().explode();
                }
                break;

        }
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
