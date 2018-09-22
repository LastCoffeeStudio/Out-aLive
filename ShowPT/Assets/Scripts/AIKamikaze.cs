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

	//[SerializeField]
	//float waitTime = 3.0f;

	[SerializeField]
	float switchProbability = 0.5f;

	[SerializeField]
	List<Waypoint> nodeList;

	[SerializeField]
	float viewDistance = 50.0f;

	[SerializeField]
	float explodingDistance = 25.0f;

	CtrlAudio audioCtr;
	[SerializeField]
	AudioClip detectSound;
    [SerializeField]
    AudioClip bipbipSound;
    private ulong idBipBipSound;

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
	bool alreadyOnSuicideJump = false;

    [SerializeField]
	state NPCstate;
    private Animator animKamikaze;

	[SerializeField]
	bool aggressiveOnActivation = true;

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
		audioCtr = GameObject.FindGameObjectWithTag("CtrlAudio").GetComponent<CtrlAudio>();
		gameObject.SetActive (false);
	}

    private void OnDestroy()
    {
        if (audioCtr != null)
        {
            audioCtr.stopSound(idBipBipSound);
        }
    }

    // Update is called once per frame
    void Update()
    {
		if (alreadyOnSuicideJump == false) 
		{
			aggressiveDestination = player.transform.position;
		}

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
				audioCtr.playOneSound("Enemies", detectSound, transform.position, 1.0f, 1.0f, 52, false, null, 50f, 0f, AudioRolloffMode.Linear);
                idBipBipSound = audioCtr.playOneSound("Enemies", bipbipSound, transform.position, 0.7f, 1.0f, 52, true, gameObject, 25f, 0f, AudioRolloffMode.Linear);
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

                if (Mathf.Abs(Vector3.Distance(gameObject.transform.position, player.transform.position)) < explodingDistance )
                {
					gameObject.GetComponent<Kamikaze>().forceExplode();
                }
                break;
        }
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

	void OnEnable()
	{
		if (aggressiveOnActivation == true) 
		{
			NPCstate = state.I_SEE_YOU;
		}
	}

	void OnDrawGizmos()
	{
		Gizmos.color = Color.white;
		Gizmos.DrawRay (transform.position, transform.forward * viewDistance);
	}
}
