using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIShieldDrone : MonoBehaviour
{
    [HideInInspector]
    public Vector3 position;
    [HideInInspector]
    public Vector2 velocity;
    [HideInInspector]
    public Vector2 acceleration;
    [HideInInspector]
    public CtrlShieldDrones ctrlShieldDrones;
    [HideInInspector]
    public GameObject player;
    [HideInInspector]
    public Transform shieldTransform;

    // Use this for initialization
    void Start()
    {
        shieldTransform = transform.GetChild(0);
    }

    // Update is called once per frame
    void Update()
    {

        float t = Time.deltaTime;
        acceleration = combine();

        acceleration = Vector2.ClampMagnitude(acceleration, ctrlShieldDrones.maxAcceleration);

        velocity = velocity + acceleration * t;
        velocity = Vector2.ClampMagnitude(velocity, ctrlShieldDrones.maxVelocity);
        
        transform.Rotate(-velocity.x, velocity.y, -transform.rotation.eulerAngles.z);
    }

    protected Vector2 combine()
    {
        Vector2 direction;
        if (ctrlShieldDrones.playerInHome)
        {
            direction = ctrlShieldDrones.KSeparation * separation(ctrlShieldDrones.radioSeparation) + ctrlShieldDrones.KPlayer * searchTarget();
        }
        else
        {
            return Vector2.zero;
            //direction = ctrlShieldDrones.KSeparation * separation(5f);
        }
        return direction;
    }

    private Vector2 separation(float radio)
    {
        Vector3 direction = new Vector3();

        var neighbours = ctrlShieldDrones.getNeightbours(this, radio);

        if (neighbours.Count == 0)
        {
            return Vector2.zero;
        }

        foreach (var agent in neighbours)
        {
           
            Vector3 distance = shieldTransform.position - agent.shieldTransform.position;

            //if magnitude equals 0 both agents are in the same point
            if (distance.magnitude > 0)
            {
                Vector2 rotation = calculateRotationToPosiotion(agent.shieldTransform.position);
                rotation = -rotation;
                direction.x  += rotation.x / distance.magnitude;
                direction.y += rotation.y / distance.magnitude;
            }

            return direction.normalized;
        }
        return Vector2.zero;
    }


    private Vector2 searchTarget()
    {
        return calculateRotationToPosiotion(player.transform.position);
    }

    private Vector2 calculateRotationToPosiotion(Vector3 position)
    {
        float rotationY = orientation2D(transform.position.x, transform.position.z, shieldTransform.position.x,
            shieldTransform.position.z, position.x, position.z);
        float rotationX = orientation2D(transform.position.y, transform.position.z, shieldTransform.position.y,
            shieldTransform.position.z, position.y, position.z);
        if (transform.position.z > position.z) rotationX *= -1;
        return new Vector2(rotationX, rotationY).normalized;
    }

    private int orientation2D(float centerA, float centerB, float pointA, float pointB, float targetA, float targetB)
    {
        double result = ((pointA - centerA) * (targetB - centerB)) - ((pointB - centerB) * (targetA - centerA));

        if (result > 0) return -1;
        else if (result < 0) return 1;
        return 0;
    }
}
