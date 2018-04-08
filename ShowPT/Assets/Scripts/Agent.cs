using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Agent : MonoBehaviour
{
    public Vector3 position;
    public Vector3 velocity;
    public Vector3 acceleration;
    public World world;
    public AgentConfig conf;
    

    private Vector3 wanderTarget;
    private GameObject debugWanderCube;
    private GameObject player;

    void Start ()
    {
        world = FindObjectOfType<World>();
        conf = FindObjectOfType<AgentConfig>();
        player = GameObject.FindGameObjectWithTag("Player");
        position = transform.position;
        velocity = new Vector3(Random.Range(-3, 3), Random.Range(-3, 3), Random.Range(-3, 3));
        if (world.debugWonder) debugWanderCube = GameObject.CreatePrimitive(PrimitiveType.Cube);
    }
	
	void Update ()
	{
        if(crash() == false) { 
	        float t = Time.deltaTime;

	        acceleration = combine();
	        acceleration = Vector3.ClampMagnitude(acceleration, conf.maxAcceleration);

	        velocity = velocity + acceleration * t;
	        velocity = Vector3.ClampMagnitude(velocity, conf.maxVelocity);

            position = position + velocity * t;

           // wrapArround(ref x, -world.bound, world.bound);

	        if (world.debugWonder == false)
	        {
	            transform.position = position;

	            if (velocity.magnitude > 0)
	            {
	                transform.LookAt(player.transform.position);
	            }
	        }
        }

    }
    void OnCollisionEnter(Collision collision)
          {
                if (collision.gameObject.tag.Equals("Agent") == false)
                  {
                      Destroy(gameObject);
                  }
         }

    bool crash()
    {
        bool crash = false;
        Ray ray = new Ray(this.position, velocity.normalized);
        crash = checkRayCrash(ray);
        if (crash == false)
        {
            ray = new Ray(this.position, new Vector3(1, 0, 0));
            crash = checkRayCrash(ray);
        }
        if (crash == false)
        {
            ray = new Ray(this.position, new Vector3(0, 1, 0));
            crash = checkRayCrash(ray);
        }
        if (crash == false)
        {
            ray = new Ray(this.position, new Vector3(0, 0, 1));
            crash = checkRayCrash(ray);
        }
        if (crash == false)
        {
            ray = new Ray(this.position, new Vector3(-1, 0, 0));
            crash = checkRayCrash(ray);
        }
        if (crash == false)
        {
            ray = new Ray(this.position, new Vector3(0, -1, 0));
            crash = checkRayCrash(ray);
        }
        if (crash == false)
        {
            ray = new Ray(this.position, new Vector3(0, 0, -1));
            crash = checkRayCrash(ray);
        }

        return crash;
    }

    bool checkRayCrash(Ray ray)
    {
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 0.01f))
        {
            if (hit.collider.gameObject.tag.Equals("Agent") == false)
            {
                Destroy(gameObject);
                return true;
            }
        }
        return false;
    }
    Vector3 cohesion()
    {
        Vector3 direction = new Vector3();

        var neighbours = world.getNeightbours(this, conf.radioCohesion);

        if (neighbours.Count == 0)
        {
            return direction;
        }
        int countAgents = 0;
        //Find the center of mass of all neighbors
        foreach (var agent in neighbours)
        {
            if (isInFieldOfVeiw(agent.position))
            {
                direction += agent.position;
                ++countAgents;
            }
        }

        direction /= countAgents;

        // a vector for our position x toward the com r
        direction = direction - this.position;

        direction = Vector3.Normalize(direction);
        return direction;
    }

    Vector3 separation()
    {

        Vector3 direction = new Vector3();

        var neighbours = world.getNeightbours(this, conf.radioSeparation);

        if (neighbours.Count == 0)
        {
            return direction;
        }

        //add the contribution neighbot towards me
        foreach (var agent in neighbours)
        {
            if (isInFieldOfVeiw(agent.position))
            {
                Vector3 towardsMe = this.position - agent.position;

                //if magnitude equals 0 both agents are in the same point
                if (towardsMe.magnitude > 0)
                {
                    //force contribution is inversly proportional to 
                    direction += (towardsMe.normalized / towardsMe.magnitude);

                }

                return direction.normalized;
            }
        }

        return Vector3.zero;
    }

    Vector3 alignment()
    {
        Vector3 direction = new Vector3();

        var neighbours = world.getNeightbours(this, conf.radioAligment);

        if (neighbours.Count == 0)
        {
            return direction;
        }

        foreach (var agent in neighbours)
        {
            if (isInFieldOfVeiw(agent.position))
            {
                //Match direction and speed == match velocity
                direction += agent.velocity;
            }
        }

        return direction.normalized;
    }

    protected virtual Vector3 combine()
    {
        Vector3 direction = conf.KCohesion*cohesion() + conf.KSeparation*separation() + conf.KAligment*alignment() + conf.KWonder*wander() 
            + conf.KAvoid* avoidObstacle() + conf.KPlayer*searchPlayer() + conf.KMinH*riseUp();
        return direction;
    }

    void wrapArround(ref Vector3 v, float min, float max)
    {
        v.x = wrapArroundFloat(v.x, min, max);
        v.y = wrapArroundFloat(v.y, min, max);
        v.z = wrapArroundFloat(v.z, min, max);
    }

    float wrapArroundFloat(float value, float min, float max)
    {
        if (value > max)
        {
            value = min;
        }
        else if (value < min)
        {
            value = max;
        }
        return value;
    }

    bool isInFieldOfVeiw(Vector3 stuff)
    {
        return (Vector3.Angle(this.velocity, stuff - this.position) <= conf.maxFieldOfViewAngle || -Vector3.Angle(this.velocity, stuff - this.position) >= -conf.maxFieldOfViewAngle);
    }


    protected Vector3 wander()
    {
        float jitter = conf.wanderJitter * Time.deltaTime;

        //add a small random vector to the target's position
        wanderTarget += new Vector3(RandomBinomial()*jitter, 0, RandomBinomial() * jitter);

        //project the vector bacj to unit circle
        wanderTarget = wanderTarget.normalized;

        //inclrease length to be the same of the radius of wander circle
        wanderTarget *= conf.wanderRadius;

        //position the target in front of the agent
        Vector3 targetInLocalSpace = wanderTarget + new Vector3(0, 0, conf.wanderDistance);

        //tranform the target from local space to world space
        Vector3 targetInWorldSpace = transform.TransformPoint(targetInLocalSpace);

        if (world.debugWonder) debugWanderCube.transform.position = targetInWorldSpace;

        targetInWorldSpace -= this.position;

        return targetInWorldSpace.normalized;

    }

    float RandomBinomial()
    {
        return Random.Range(0f, 1f) - Random.Range(0f, 1f);
    }

    Vector3 avoidObstacle()
    {
        bool existObstacle = false;
        Ray ray = new Ray(this.position, velocity.normalized);
        Vector3 direcctionFront = new Vector3();

        checkObstacles(ray, ref direcctionFront, ref existObstacle);
        Vector3 otherDirection = new Vector3();
        ray = new Ray(this.position, new Vector3(0, 0, 1));
        checkObstacles2(ray, ref otherDirection, ref existObstacle);
        ray = new Ray(this.position, new Vector3(0, 1, 0));
        checkObstacles2(ray, ref otherDirection, ref existObstacle);
        ray = new Ray(this.position, new Vector3(0, 1, 1));
        checkObstacles2(ray, ref otherDirection, ref existObstacle);
        ray = new Ray(this.position, new Vector3(1, 0, 0));
        checkObstacles2(ray, ref otherDirection, ref existObstacle);
        ray = new Ray(this.position, new Vector3(1, 0, 1));
        checkObstacles2(ray, ref otherDirection, ref existObstacle);
        ray = new Ray(this.position, new Vector3(1, 1, 0));
        checkObstacles2(ray, ref otherDirection, ref existObstacle);
        ray = new Ray(this.position, new Vector3(1, 1, 1));
        checkObstacles2(ray, ref otherDirection, ref existObstacle);
        ray = new Ray(this.position, new Vector3(0, 0, -1));
        checkObstacles2(ray, ref otherDirection, ref existObstacle);
        ray = new Ray(this.position, new Vector3(0, -1, 0));
        checkObstacles2(ray, ref otherDirection, ref existObstacle);
        ray = new Ray(this.position, new Vector3(0, -1, -1));
        checkObstacles2(ray, ref otherDirection, ref existObstacle);
        ray = new Ray(this.position, new Vector3(-1, 0, 0));
        checkObstacles2(ray, ref otherDirection, ref existObstacle);
        ray = new Ray(this.position, new Vector3(-1, 0, -1));
        checkObstacles2(ray, ref otherDirection, ref existObstacle);
        ray = new Ray(this.position, new Vector3(-1, -1, 0));
        checkObstacles2(ray, ref otherDirection, ref existObstacle);
        ray = new Ray(this.position, new Vector3(-1, -1, -1));
        checkObstacles2(ray, ref otherDirection, ref existObstacle);

        if (existObstacle == false)
        {
            return Vector3.zero;
        }

        otherDirection = otherDirection.normalized;
        direcctionFront = direcctionFront.normalized;
        otherDirection = otherDirection + direcctionFront;
        return otherDirection.normalized;
    }

    void checkObstacles(Ray ray, ref Vector3 r, ref bool existObstacle)
    {
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, conf.radioAvoid))
        {
            if (hit.collider.gameObject.tag.Equals("Agent") == false)
            {
                r += flee(hit.point);
                existObstacle = true;
            }
        }
    }

    void checkObstacles2(Ray ray, ref Vector3 direction, ref bool existObstacle)
    {
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, conf.radioAvoid))
        {
            if (hit.collider.gameObject.tag.Equals("Agent") == false)
            {
                Vector3 towardsMe = this.position - hit.point;

                //if magnitude equals 0 both agents are in the same point
                if (towardsMe.magnitude > 0)
                {
                    //force contribution is inversly proportional to 
                    direction += (towardsMe.normalized / towardsMe.magnitude);
                }
            }
        }
    }
    Vector3 flee(Vector3 target)
    {
        //Run the oposite direction from target
        Vector3 desiredVel = (this.position - target).normalized * conf.maxVelocity;

        //steer velocity
        return desiredVel - velocity;
    }

    Vector3 searchPlayer()
    {
        Vector3 postitionPlayer = player.transform.position;
        postitionPlayer.y += world.hightPlayer;

        if (Vector3.Distance(this.position, postitionPlayer) >  world.radiousPlayer)
        {
            return Vector3.Normalize(postitionPlayer - this.position);
        }

        return Vector3.zero;
    }

    Vector3 riseUp()
    {
        Vector3 postitionPlayer = player.transform.position;

        if ((postitionPlayer.y + world.minimunHight) > this.position.y)
        {
            return new Vector3(0, 1, 0);
        }

        return Vector3.zero;
    }
}
