using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class CameraAgent : MonoBehaviour
{
    /*public Vector3 x;
    public Vector3 v;
    public Vector3 a;
    public World world;


    private Vector3 wanderTarget;
    private GameObject debugWanderCube;
    private GameObject player;
    public Text text;
    public float Ravoid;
    public float Rplayer;
    public float HightPlayer;

    public float Kavoid;
    public float Kw;
    public float Kplayer;
    public float KMinH;

    public float maxA;
    public float maxV;

    public float maxVnearPlayer;
    public float minimumHight;

    public float WanderJitter;
    public float WanderRadius;
    public float WanderDistance;

    void Start()
    {
        world = FindObjectOfType<World>();
        player = GameObject.FindGameObjectWithTag("Player");
        x = transform.position;
        v = new Vector3(Random.Range(-3, 3), Random.Range(-3, 3), Random.Range(-3, 3));
        if (world.debugWonder) debugWanderCube = GameObject.CreatePrimitive(PrimitiveType.Cube);
    }

    void Update()
    {
        if (crash() == false)
        {
            float t = Time.deltaTime;
            a = combine();
            a = Vector3.ClampMagnitude(a, maxA);

            v = v + a * t;
            if (Vector3.Magnitude(player.transform.position - x) < Rplayer+HightPlayer)
            {
                v = Vector3.ClampMagnitude(v, maxVnearPlayer);
            }
            else
            {
                v = Vector3.ClampMagnitude(v, maxV);
            }

            x = x + v * t;

            // wrapArround(ref x, -ctrlDrones.bound, ctrlDrones.bound);

            if (world.debugWonder == false)
            {
                transform.position = x;

                if (v.magnitude > 0)
                {
                    Vector3 te = player.transform.position + v;

                    transform.LookAt(player.transform.position+v);
                }
            }
        }

    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag.Equals("Agent") == false)
        {
            text.text = (Int32.Parse(text.text) - 1).ToString();
            Destroy(gameObject);
        }
    }

    bool crash()
    {
        bool crash = false;
        Ray ray = new Ray(this.x, v.normalized);
        crash = checkRayCrash(ray);
        if (crash == false)
        {
            ray = new Ray(this.x, new Vector3(1, 0, 0));
            crash = checkRayCrash(ray);
        }
        if (crash == false)
        {
            ray = new Ray(this.x, new Vector3(0, 1, 0));
            crash = checkRayCrash(ray);
        }
        if (crash == false)
        {
            ray = new Ray(this.x, new Vector3(0, 0, 1));
            crash = checkRayCrash(ray);
        }
        if (crash == false)
        {
            ray = new Ray(this.x, new Vector3(-1, 0, 0));
            crash = checkRayCrash(ray);
        }
        if (crash == false)
        {
            ray = new Ray(this.x, new Vector3(0, -1, 0));
            crash = checkRayCrash(ray);
        }
        if (crash == false)
        {
            ray = new Ray(this.x, new Vector3(0, 0, -1));
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
                text.text = (Int32.Parse(text.text) - 1).ToString();
                Destroy(gameObject);
                return true;
            }
        }
        return false;
    }

    protected virtual Vector3 combine()
    {
        Vector3 r =  Kw * wander() + Kavoid * avoidObstacle() + Kplayer * searchPlayer() + KMinH * riseUp();
        return r;
    }
    

    protected Vector3 wander()
    {
        float jitter = WanderJitter * Time.deltaTime;

        //add a small random vector to the target's position
        wanderTarget += new Vector3(RandomBinomial() * jitter, 0, RandomBinomial() * jitter);

        //project the vector bacj to unit circle
        wanderTarget = wanderTarget.normalized;

        //inclrease length to be the same of the radius of wander circle
        wanderTarget *= WanderRadius;

        //position the target in front of the agent
        Vector3 targetInLocalSpace = wanderTarget + new Vector3(0, 0, WanderDistance);

        //tranform the target from local space to ctrlDrones space
        Vector3 targetInWorldSpace = transform.TransformPoint(targetInLocalSpace);

        if (world.debugWonder) debugWanderCube.transform.position = targetInWorldSpace;

        targetInWorldSpace -= this.x;

        return targetInWorldSpace.normalized;

    }

    float RandomBinomial()
    {
        return Random.Range(0f, 1f) - Random.Range(0f, 1f);
    }

    Vector3 avoidObstacle()
    {
        bool existObstacle = false;
        Vector3 W = new Vector3();
        Ray ray = new Ray(this.x, v.normalized);
        checkObstacles(ray, ref W, ref existObstacle);
        Vector3 r = new Vector3();
        ray = new Ray(this.x, new Vector3(0, 0, 1));
        checkObstacles2(ray, ref r, ref existObstacle);
        ray = new Ray(this.x, new Vector3(0, 1, 0));
        checkObstacles2(ray, ref r, ref existObstacle);
        ray = new Ray(this.x, new Vector3(0, 1, 1));
        checkObstacles2(ray, ref r, ref existObstacle);
        ray = new Ray(this.x, new Vector3(1, 0, 0));
        checkObstacles2(ray, ref r, ref existObstacle);
        ray = new Ray(this.x, new Vector3(1, 0, 1));
        checkObstacles2(ray, ref r, ref existObstacle);
        ray = new Ray(this.x, new Vector3(1, 1, 0));
        checkObstacles2(ray, ref r, ref existObstacle);
        ray = new Ray(this.x, new Vector3(1, 1, 1));
        checkObstacles2(ray, ref r, ref existObstacle);
        ray = new Ray(this.x, new Vector3(0, 0, -1));
        checkObstacles2(ray, ref r, ref existObstacle);
        ray = new Ray(this.x, new Vector3(0, -1, 0));
        checkObstacles2(ray, ref r, ref existObstacle);
        ray = new Ray(this.x, new Vector3(0, -1, -1));
        checkObstacles2(ray, ref r, ref existObstacle);
        ray = new Ray(this.x, new Vector3(-1, 0, 0));
        checkObstacles2(ray, ref r, ref existObstacle);
        ray = new Ray(this.x, new Vector3(-1, 0, -1));
        checkObstacles2(ray, ref r, ref existObstacle);
        ray = new Ray(this.x, new Vector3(-1, -1, 0));
        checkObstacles2(ray, ref r, ref existObstacle);
        ray = new Ray(this.x, new Vector3(-1, -1, -1));
        checkObstacles2(ray, ref r, ref existObstacle);

        if (existObstacle == false)
        {
            return Vector3.zero;
        }

        r = r.normalized;
        W = W.normalized;
        r = r + W;
        return r.normalized;
    }

    void checkObstacles(Ray ray, ref Vector3 r, ref bool existObstacle)
    {
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Ravoid))
        {
            if (hit.collider.gameObject.tag.Equals("Agent") == false)
            {
                r += flee(hit.point);
                existObstacle = true;
            }
        }
    }

    void checkObstacles2(Ray ray, ref Vector3 r, ref bool existObstacle)
    {
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Ravoid))
        {
            if (hit.collider.gameObject.tag.Equals("Agent") == false)
            {
                Vector3 towardsMe = this.x - hit.point;

                //if magnitude equals 0 both agents are in the same point
                if (towardsMe.magnitude > 0)
                {
                    //force contribution is inversly proportional to 
                    r += (towardsMe.normalized / towardsMe.magnitude);
                }
            }
        }
    }
    Vector3 flee(Vector3 target)
    {
        //Run the oposite direction from target
        Vector3 desiredVel = (this.x - target).normalized * maxV;

        //steer velocity
        return desiredVel - v;
    }

    Vector3 searchPlayer()
    {
        Vector3 postitionPlayer = player.transform.position;
        postitionPlayer.y += HightPlayer;

        if (Vector3.Distance(this.x, postitionPlayer) > Rplayer)
        {
            return Vector3.Normalize(postitionPlayer - this.x);
        }
        else
        {
            return -Vector3.Normalize(postitionPlayer - this.x);
        }
        return Vector3.zero;
    }

    Vector3 riseUp()
    {
        Vector3 postitionPlayer = player.transform.position;

        if ((postitionPlayer.y + minimumHight) > this.x.y)
        {
            return new Vector3(0, 1, 0);
        }

        return Vector3.zero;
    }*/
}
