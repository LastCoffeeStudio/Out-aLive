using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LilRobot : MonoBehaviour {

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
    public Transform head;
    public Transform agentTransform;
    public float recoverTime;
    public float maxSpeed;
    public float jumpForce;
    [Range(1,10)]
    public float headAngleFactor;
    [Range(0,1)]
    public float maxInclinationHead;
    [Range(1,10)]
    public float rotationSpeed;

    private Rigidbody rb;
    private NavMeshAgent agent;
    private NavMeshPath path;
    private LilRobotState state;
    private Vector3 headPosition;
    private Quaternion lastLocalRotation;
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
        headPosition = head.localPosition;
        lastLocalRotation = head.localRotation;
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
                rb.AddForce(Vector3.up * jumpForce);
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

    /*void OnCollisionEnter(Collision collision)
    {
        Vector3 position = collision.contacts[0].point;
        Vector3 normal = collision.contacts[0].normal;
        rb.AddForceAtPosition(-normal, position);
    }*/

    void updateObjects()
    {
        agentTransform.position = transform.position;
        agent.nextPosition = transform.position;
        head.position = transform.position;
        //head.localRotation = Quaternion.Inverse(transform.rotation);
        
        Vector3 vel = rb.velocity.normalized;
        float maxInclination = Mathf.Min(maxInclinationHead, rb.velocity.magnitude / headAngleFactor);
        Debug.Log(maxInclination);
        Vector3 dir = Vector3.Lerp(Vector3.up, new Vector3(vel.x, 0f, vel.z), maxInclination);
        head.localRotation = Quaternion.Slerp(head.localRotation, Quaternion.FromToRotation(Vector3.up, dir), Time.deltaTime * rotationSpeed);
        //head.localRotation = head.localRotation * Quaternion.FromToRotation(head.up, rb.velocity);
        //lastLocalRotation = head.localRotation;

        /** Update Debug Camera **/
        lilRobCamera.position = transform.position - Vector3.forward * 2 + Vector3.up * 0.5f;
    }
}
