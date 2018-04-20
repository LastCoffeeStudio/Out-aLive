using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LilRobot : Enemy {

    enum LilRobotState
    {
        IDLE,
        ATTACK,
        JUMP,
        RECOVER
    }

    public Transform lilRobCamera;
    public Transform target;
    public float detectionDistance;
    public float jumpDistance;
    public Transform agentTransform;
    public float recoverTime;
    public float maxSpeed;
    public float jumpForceVertical;
    public float jumpForceHorizontal;
    [Range(1,10)]
    public float rotationSpeed;

    private Rigidbody rb;
    private NavMeshAgent agent;
    private NavMeshPath path;
    private LilRobotState state;
    private float recoverResetTime;

    // Use this for initialization
    void Start ()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody>();
        agent = agentTransform.GetComponent<NavMeshAgent>();
        agent.updatePosition = false;
        agent.updateRotation = false;
        path = new NavMeshPath();
        state = LilRobotState.IDLE;
        recoverResetTime = recoverTime;
    }

    private void FixedUpdate()
    {
        /**Manual Control**/
        /*float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");*/
        /****/

        float distance = Vector3.Distance(target.position, transform.position);

        switch (state)
        {
            case LilRobotState.IDLE:
                /**Manual Control**/
                /*rb.AddForce(Vector3.right * x * 10);
                rb.AddForce(Vector3.forward * y * 10);*/
                /****/
                //rb.AddForce(transform.InverseTransformDirection(rb.velocity));

                if (distance < detectionDistance)
                {
                    state = LilRobotState.ATTACK;
                }
                break;
            case LilRobotState.ATTACK:
                agent.CalculatePath(target.position, path);
                Vector3 dir = path.corners[1] - transform.position;
                //rb.AddForce(dir.normalized * 0.3f);
                rb.AddForce(dir.normalized * maxSpeed * Time.deltaTime);
                if (distance > detectionDistance + 5f)
                {
                    state = LilRobotState.IDLE;
                }
                else if (distance < jumpDistance)
                {
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
                    //rb.AddForce(transform.InverseTransformDirection(rb.velocity));
                    recoverTime -= Time.deltaTime;
                }
                else
                {
                    recoverTime = recoverResetTime;
                    state = LilRobotState.IDLE;
                }
                break;
            default:
                break;
        }

       
    }

    // Update is called once per frame
    void Update ()
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

    public override void getHit()
    {
        --enemyHealth;
        Debug.Log(enemyHealth);
        //Execute properly Animation
        checkHealth();
    }
}
