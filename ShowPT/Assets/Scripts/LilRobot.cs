using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LilRobot : Enemy
{

    enum LilRobotState
    {
        IDLE,
        ATTACK,
        JUMP,
        RECOVER,
        PARALYZED
    }

    [Header("LilRobot parameters")]
    public Transform target;
    public Transform targetHead;
    public Transform targetFeet;
    public Transform obstacleDetector;
    public float obstacleDetectorDistance;
    public float detectionDistance;
    public float jumpDistance;
    public float climbForce;
    [Range(0f, 0.95f)]
    public float deceleration;
    public Transform agentTransform;
    public float recoverTime;
    public float maxSpeed;
    public float jumpForceVertical;
    public float jumpForceHorizontal;
    [Range(1, 10)]
    public float rotationSpeed;
    public int damage;

    [SerializeField]
    LayerMask detectionLayer;
    private Rigidbody rb;
    private NavMeshAgent agent;
    [SerializeField]
    private LilRobotState state;
    private LilRobotState lastState;
    private float recoverResetTime;
    private bool climbing;
    private bool playerDamaged;
    [SerializeField]
    private Vector3 destination;

    // Use this for initialization
    void Start()
    {
        ctrAudio = GameObject.FindGameObjectWithTag("CtrlAudio").GetComponent<CtrlAudio>();
        hitAudio = ctrAudio.hit;
        target = GameObject.FindGameObjectWithTag("Player").transform;
        obstacleDetector = transform.Find("ObstacleDetector");
        targetHead = target.Find("Head");
        targetFeet = target.Find("Feet");
        rb = GetComponent<Rigidbody>();
        agent = agentTransform.GetComponent<NavMeshAgent>();
        agent.updatePosition = false;
        agent.updateRotation = false;
        state = LilRobotState.IDLE;
        recoverResetTime = recoverTime;
        climbing = false;
        playerDamaged = false;
        destination = target.position;
    }

    private void FixedUpdate()
    {
        float distance = Vector3.Distance(target.position, transform.position);

        switch (state)
        {
            case LilRobotState.IDLE:

                rb.angularVelocity = new Vector3(rb.angularVelocity.x * deceleration, rb.angularVelocity.y, rb.angularVelocity.z * deceleration);
                rb.velocity = new Vector3(rb.velocity.x * deceleration, rb.velocity.y, rb.velocity.z * deceleration);
                if (rb.velocity.magnitude < 0.1f)
                {
                    rb.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ |
                        RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
                }
                if (distance < detectionDistance && detectPlayer())
                {
                    destination = target.position;
                    rb.constraints = RigidbodyConstraints.None;
                    state = LilRobotState.ATTACK;
                }
                break;
            case LilRobotState.ATTACK:
                //Update destination
                if (detectPlayer() && distance < detectionDistance)
                {
                    destination = target.position;
                }

                if (agent.SetDestination(destination))
                {
                    Vector3 nextPosition = transform.position;
                    if (agent.path.corners.Length > 1)
                    {
                        nextPosition = agent.path.corners[1];
                    }
                    //Move Lil
                    Vector3 dir = nextPosition - transform.position;
                    rb.AddForce(dir.normalized * maxSpeed * Time.deltaTime);

                    //Climb if necessary
                    obstacleDetector.LookAt(new Vector3(nextPosition.x, obstacleDetector.position.y, nextPosition.z));
                    if (Physics.Raycast(obstacleDetector.position, obstacleDetector.forward, obstacleDetectorDistance))
                    {
                        climbing = true;
                        rb.useGravity = false;
                        rb.AddForce(new Vector3(-rb.velocity.x, climbForce, -rb.velocity.z));
                    }
                    else
                    {
                        climbing = false;
                        rb.useGravity = true;
                    }
                }
                if (!climbing && agent.remainingDistance <= agent.stoppingDistance && !detectPlayer())
                {
                    state = LilRobotState.IDLE;
                }
                else if (distance < jumpDistance)
                {
                    climbing = false;
                    rb.useGravity = true;
                    state = LilRobotState.JUMP;
                }
                break;
            case LilRobotState.JUMP:
                Vector3 playerDir = target.transform.position - transform.position;
                rb.velocity = Vector3.zero;
                rb.AddForce(Vector3.up * jumpForceVertical);
                rb.AddForce(playerDir * jumpForceHorizontal);
                state = LilRobotState.RECOVER;
                break;
            case LilRobotState.RECOVER:
                if (recoverTime > 0f)
                {
                    recoverTime -= Time.deltaTime;
                }
                else
                {
                    recoverTime = recoverResetTime;
                    state = LilRobotState.IDLE;
                    playerDamaged = false;
                }
                break;
            case LilRobotState.PARALYZED:
                paralyzedActualTime -= Time.deltaTime;
                if (paralyzedActualTime <= 0f)
                {
                    status = Status.NONE;
                    state = LilRobotState.IDLE;
                    paralyzedActualTime = paralyzedTotalTime;
                }
                break;
            default:
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        updateObjects();
    }

    private void updateObjects()
    {
        agentTransform.position = transform.position;
        agent.nextPosition = transform.position;
    }

    public override void getHit(int damage)
    {
        ctrAudio.playOneSound("Enemies", hitAudio, transform.position, 1.0f, 0.0f, 128);
        enemyHealth -= damage;
        rb.constraints = RigidbodyConstraints.None;
        state = LilRobotState.ATTACK;
        checkHealth();
    }

    public override void checkHealth()
    {
        if (enemyHealth <= 0f)
        {
            Destroy(gameObject);
            ScoreController.addDead(ScoreController.Enemy.LIL);
        }
    }

    private bool detectPlayer()
    {
        return (!Physics.Linecast(transform.position, target.position, detectionLayer) ||
            !Physics.Linecast(transform.position, targetHead.position, detectionLayer) ||
            !Physics.Linecast(transform.position, targetFeet.position, detectionLayer));
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (state == LilRobotState.RECOVER && collision.gameObject.tag == "Player" && !playerDamaged)
        {
            playerDamaged = true;
            collision.gameObject.GetComponent<PlayerHealth>().ChangeHealth(-damage);
        }
    }

    public override void setStatusParalyzed()
    {
        if (status == Status.NONE && state != LilRobotState.PARALYZED)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            state = LilRobotState.PARALYZED;
            status = Status.PARALYZED;
        }
    }
}
