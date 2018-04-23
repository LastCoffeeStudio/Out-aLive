using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class SnitchDrone : MonoBehaviour
{

    public Vector3 position;
    public Vector3 velocity;
    public Vector3 acceleration;
    public Transform playerTransform;
    public Transform targetTransform;
    public CtrlDrones ctrlDrones;

    public float radioAvoid;
    public float KAvoid;
    public float KWall;
    public float maxAcceleration;
    public float maxVelocity;
    public float maxFieldOfViewAngle = 180;

    void Start()
    {
        position = transform.position;
        velocity = new Vector3(Random.Range(-3, 3), Random.Range(-3, 3), Random.Range(-3, 3));
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (crash() == false)
        {
            float t = Time.deltaTime;

            acceleration = combine();
            acceleration = Vector3.ClampMagnitude(acceleration, maxAcceleration);

            velocity = velocity + acceleration * t;
            velocity = Vector3.ClampMagnitude(velocity, maxVelocity);

            position = position + velocity * t;

            transform.position = position;

            if (velocity.magnitude > 0)
            {
                //transform.LookAt(target.transform.position);
            }

        }

    }

    void OnCollisionEnter(Collision collision)
    {
        /*if (collision.gameObject.tag.Equals("Drone") == false)
        {
            Destroy(gameObject);
        }*/
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
            if (hit.collider.gameObject.tag.Equals("Drone") == false)
            {
                //Destroy(gameObject);
                return true;
            }
        }

        return false;
    }

    protected virtual Vector3 combine()
    {
        Vector3 direction = KAvoid * avoidObstacle() + KWall*vectorWall();
        return direction;
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
        if (Physics.Raycast(ray, out hit, radioAvoid))
        {
            if (hit.collider.gameObject.tag.Equals("Drone") == false)
            {
                r += flee(hit.point);
                existObstacle = true;
            }
        }
    }

    void checkObstacles2(Ray ray, ref Vector3 direction, ref bool existObstacle)
    {
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, radioAvoid))
        {
            if (hit.collider.gameObject.tag.Equals("Drone") == false)
            {
                Vector3 towardsMe = this.position - hit.point;

                //if magnitude equals 0 both agents are in the same point
                if (towardsMe.magnitude > 0)
                {
                    //force contribution is inversly proportional to 
                    direction += (towardsMe.normalized / towardsMe.magnitude);
                    existObstacle = true;
                }
            }
        }
    }

    Vector3 flee(Vector3 target)
    {
        //Run the oposite direction from target
        Vector3 desiredVel = (this.position - target).normalized * maxVelocity;

        //steer velocity
        return desiredVel - velocity;
    }

    Vector3 vectorWall()
    {
        Vector3 direction = new Vector3();
        Vector3 directionPlayerDrones = new Vector3();
        Vector3 centroidDrones = new Vector3();
        var neighbours = ctrlDrones.getAllNeightbours();

        //Si no hay drones ponemos la posicion del Target.
        if (neighbours.Count == 0)
        {
            return (targetTransform.position - transform.position).normalized;
        }

        foreach (var drone in neighbours)
        {
            centroidDrones += drone.transform.position;
            directionPlayerDrones = drone.position - playerTransform.position;
            directionPlayerDrones += drone.position - transform.position;
        }

        directionPlayerDrones = directionPlayerDrones.normalized;
        centroidDrones /= neighbours.Count;

        //Buscamos el centro entre la posicion del Target 
        if (ctrlDrones.playerInHome == false)
        {
            centroidDrones = (centroidDrones + targetTransform.position) / 2.0f;
        }
        direction = (centroidDrones + directionPlayerDrones * 10) - transform.position;

        return direction.normalized;
    }
}
