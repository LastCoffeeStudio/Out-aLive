using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Waz : Enemy
{
    public enum state
    {
        WALKING,
        WAITING,
        I_SEE_YOU,
        SHOOTING,
        I_HEAR_YOU,
        ALERT
    }

    [Header("Waz parameters")]
    [SerializeField]
    float waitTime = 3.0f;

    /*[SerializeField]
    float switchProbability = 0.5f;*/

    [SerializeField]
    List<Waypoint> nodeList;

    [SerializeField]
    float viewDistance = 50.0f;

    [SerializeField]
    float absoluteAwarenessRadius = 5.0f;

    [SerializeField]
    float shootingDistance = 20.0f;

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

    public state NPCstate;

    private Animator wazAnimator;
	private bool imAlreadyDead = false;

    public GameObject body;
    public GameObject eye;

    // Use this for initialization
    void Start()
    {
		wazAnimator = gameObject.GetComponent<Animator> ();
        ctrAudio = GameObject.FindGameObjectWithTag("CtrlAudio").GetComponent<CtrlAudio>();
        hitAudio = ctrAudio.hit;
        navMeshAgent = this.GetComponent<NavMeshAgent>();

        if (navMeshAgent == null)
        {
            Debug.LogError("Nav mesh agent not found on object " + gameObject.name);
        }
        else
        {
            if (nodeList != null && nodeList.Count >= 2)
            {
                nodeIndex = 0;
                SetDestination();
                NPCstate = state.WALKING;
            }
            else
            {
                Debug.LogError("Not enough nodes for the object " + gameObject.name + " to patrol at all.");
            }
        }
        
        player = GameObject.FindGameObjectWithTag("Player");
        playerMovment = player.GetComponent<PlayerMovment>();
    }

    // Update is called once per frame
    void Update()
    {
        updateState();

        //These two will always happen, no matter the state
        if (CanHearPlayer())
        {
            goToPlayer();
            NPCstate = state.I_HEAR_YOU;
        }
        if (CanSeePlayer())
        {
            aggressiveDestination = player.transform.position;
            if (NPCstate != state.SHOOTING)
            {
                NPCstate = state.I_SEE_YOU;
            }
        }

        /*if (NPCstate == state.I_SEE_YOU || NPCstate == state.SHOOTING)
        {
            Vector3 vectorToPlayer = gameObject.transform.position - player.transform.position;
            vectorToPlayer.y = 0;
            Vector3.Normalize(vectorToPlayer);
            Vector3 myVectorSpeed = navMeshAgent.velocity;
            myVectorSpeed.y = 0;
            Vector3.Normalize(myVectorSpeed);
            float angle = Vector3.Angle(vectorToPlayer, myVectorSpeed);
            if (angle > -50 && angle < 50)
            {
                eye.transform.LookAt(player.transform);
                eye.transform.Rotate(90f,0f,0f);
                //Same high like Waz
                Transform playerTranformLinear = player.transform;
                Vector3 playerPosition = player.transform.position;
                playerPosition.y = transform.position.y;
                playerTranformLinear.position = playerPosition;
                body.transform.LookAt(playerTranformLinear);
            }
        }*/

        shoot();

		if(imAlreadyDead && wazAnimator.GetCurrentAnimatorStateInfo (0).normalizedTime > 1) 
		{
			Destroy (gameObject);
			ScoreController.addDead (ScoreController.Enemy.WAZ);
		}
    }

    void goToPlayer()
    {
        aggressiveDestination = player.transform.position;
        navMeshAgent.SetDestination(aggressiveDestination);
        
    }
    public override void getHit(int damage)
    {
        if (state.SHOOTING != NPCstate && state.I_SEE_YOU != NPCstate)
        {
            goToPlayer();
            NPCstate = state.I_HEAR_YOU;
        }
        ctrAudio.playOneSound("Enemies", hitAudio, transform.position, 1.0f, 0.0f, 128);
        enemyHealth -= damage;
        Debug.Log(enemyHealth);
        checkHealth();
    }

    public override void checkHealth()
    {
        if (enemyHealth <= 0f)
        {
			if (!imAlreadyDead) {
				wazAnimator.SetTrigger ("dying");
				imAlreadyDead = true;
			} 
        }
        else if (NPCstate == state.WALKING || NPCstate == state.WAITING)
        {
            aggressiveDestination = player.transform.position;
            navMeshAgent.SetDestination(aggressiveDestination);
            NPCstate = state.I_HEAR_YOU;
        }
    }

    private void updateState()
    {
        switch (NPCstate)
        {
            case state.WALKING:
                wazAnimator.SetBool("walking", true);
                if (navMeshAgent.remainingDistance <= 1.0f)
                {
                    waitTimer = 0.0f;
                    NPCstate = state.WAITING;
                }
                break;

            case state.WAITING:
                wazAnimator.SetBool("walking", false);
                waitTimer += Time.deltaTime;
                if (waitTimer >= waitTime)
                {
                    ChangePatrolNode();
                    SetDestination();
                    NPCstate = state.WALKING;
                }
                break;

            case state.I_SEE_YOU:
                wazAnimator.SetBool("walking", true);
                LookAtSomething(aggressiveDestination);
                navMeshAgent.SetDestination(aggressiveDestination);

                //Should I start shooting?
                if (navMeshAgent.remainingDistance <= shootingDistance && CanSeePlayer())
                {
                    navMeshAgent.SetDestination(gameObject.transform.position);
                    navMeshAgent.isStopped = true;
                    if (!imAlreadyDead)
                    {
                        active = true;
                    }
                    NPCstate = state.SHOOTING;
                }

                //Did I lose track of the objective?
                if (navMeshAgent.remainingDistance <= 1.0f && !CanSeePlayer())
                {
                    alertTimer = 0f;
                    alertRotationTimer = 0f;
                    LookAtSomething(aggressiveDestination);
                    NPCstate = state.ALERT;
                }
                break;

            case state.SHOOTING:
                wazAnimator.SetBool("walking", false);
                navMeshAgent.SetDestination(aggressiveDestination);
                LookAtSomething(aggressiveDestination);
                if (!CanSeePlayer() || navMeshAgent.remainingDistance > shootingDistance || imAlreadyDead)
                {
                    navMeshAgent.isStopped = false;
                    active = false;
                    NPCstate = state.I_SEE_YOU;
                }
                break;

            case state.I_HEAR_YOU:
                wazAnimator.SetBool("walking", true);
                if (navMeshAgent.remainingDistance <= 1.0f && !CanSeePlayer())
                {
                    alertTimer = 0f;
                    alertRotationTimer = 0f;
                    NPCstate = state.ALERT;
                }
                break;

            case state.ALERT:
                wazAnimator.SetBool("walking", false);
                Vector3 rotation = new Vector3(0f, alertRotation, 0f);
                gameObject.transform.Rotate(rotation * Time.deltaTime);
                alertRotationTimer += Time.deltaTime;
                if (alertRotationTimer >= alertRotationTime)
                {
                    alertRotation = alertRotation * (-1);
                    alertRotationTimer = 0;
                }

                alertTimer += Time.deltaTime;
                if (alertTimer >= alertTime)
                {
                    SetDestination();
                    NPCstate = state.WALKING;
                }
                break;
        }
    }

    private bool CanSeePlayer()
    {
        if (Vector3.Distance(transform.position, player.transform.position) < absoluteAwarenessRadius)
        {
            return true;
        }
        else if (Vector3.Distance(transform.position, player.transform.position) < viewDistance)
        {
            Vector3 directionToPlayer = (player.transform.position - transform.position).normalized;
            float angleBetweenThisAndPlayer = Vector3.Angle(transform.forward, directionToPlayer);
            if (angleBetweenThisAndPlayer < viewAngle)
            {
                if (!Physics.Linecast(transform.position, player.transform.position, viewMask))
                {
                    return true;
                }
            }
        }
        return false;
    }

    bool CanHearPlayer()
    {
        if (playerMovment.noiseValue > Vector3.Distance(transform.position, player.transform.position))
        {
            return true;
        }
        return false;
    }

    private void SetDestination()
    {
        Vector3 targetPosition = nodeList[nodeIndex].transform.position;
        navMeshAgent.SetDestination(targetPosition);
    }

    private void ChangePatrolNode()
    {
        if (nodeList.Count >= 2)
        {
            if (patrolForward)
            {
                if (nodeIndex >= nodeList.Count - 1)
                {
                    --nodeIndex;
                    patrolForward = false;
                }
                else
                {
                    ++nodeIndex;
                }
            }
            else
            {
                if (nodeIndex <= 0)
                {
                    ++nodeIndex;
                    patrolForward = true;
                }
                else
                {
                    --nodeIndex;
                }
            }
        }
    }

    void LookAtSomething(Vector3 something)
    {
        var lookPos = something - gameObject.transform.position;
        transform.rotation = Quaternion.Slerp(gameObject.transform.rotation, Quaternion.LookRotation(lookPos), Time.deltaTime * 6);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawRay(transform.position, transform.forward * viewDistance);
    }
}
