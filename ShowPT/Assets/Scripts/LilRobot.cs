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
        RECOVER
    }

    public Transform lilRobCamera;
    public Transform target;
    public Transform targetHead;
    public Transform targetFeet;
    public Transform obstacleDetector;
    public float obstacleDetectorDistance;
    public float detectionDistance;
    public float jumpDistance;
    public float climbForce;
    [Range(0f,0.95f)]
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
    //private NavMeshPath path;
    [SerializeField]
    private LilRobotState state;
    private float recoverResetTime;
    private bool climbing;
    private bool playerDamaged;

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
        //path = new NavMeshPath();
        state = LilRobotState.IDLE;
        recoverResetTime = recoverTime;
        climbing = false;
        playerDamaged = false;
    }

    private void FixedUpdate()
    {
        float distance = Vector3.Distance(target.position, transform.position);

        switch (state)
        {
            case LilRobotState.IDLE:

                //Debug.DrawLine(transform.position, target.position, new Color(1f, 0f, 0f, 1f));
                rb.angularVelocity = new Vector3(rb.angularVelocity.x * deceleration, rb.angularVelocity.y, rb.angularVelocity.z * deceleration);
                rb.velocity = new Vector3(rb.velocity.x * deceleration, rb.velocity.y, rb.velocity.z * deceleration);
                if (rb.velocity.magnitude < 0.1f)
                {
                    rb.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ |
                        RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
                }
                if (distance < detectionDistance && detectPlayer())
                {
                    rb.constraints = RigidbodyConstraints.None;
                    state = LilRobotState.ATTACK;
                }
                break;
            case LilRobotState.ATTACK:
                //Debug.DrawLine(transform.position, target.position, new Color(1f, 0f, 0f, 1f));
                //Debug.DrawRay(obstacleDetector.position, obstacleDetector.forward * obstacleDetectorDistance, new Color(0f, 0f, 1f));

                if (agent.SetDestination(target.position) && agent.path.corners.Length > 1)
                {
                    Vector3 nextPosition = agent.path.corners[1];
                    //Move Lil
                    Vector3 dir = nextPosition - transform.position;
                    rb.AddForce(dir.normalized * maxSpeed * Time.deltaTime);
                    /** Debug **/
                    /*for (int i = 0; i < agent.path.corners.Length - 1; ++i)
                    {
                        Debug.DrawLine(agent.path.corners[i], agent.path.corners[i + 1], new Color(0f, 1f, 0f));
                    }*/

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
                if (!climbing && agent.remainingDistance <= agent.stoppingDistance)
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
        /** Update Debug Camera **/
        if (lilRobCamera != null)
        {
            lilRobCamera.position = transform.position - Vector3.forward * 2 + Vector3.up * 0.5f;
        }
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
}
