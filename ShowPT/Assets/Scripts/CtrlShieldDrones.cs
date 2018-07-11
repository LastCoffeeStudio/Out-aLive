using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CtrlShieldDrones : MonoBehaviour {

    //Distance betwen Target and player for drones start to search player.
    public float radioHomeEnter;
    //Distance betwen Target and player for drones comeback to target.
    public float radioHomeExit;
    public float radioSeparation;
    public float KSeparation;
    public float KPlayer;
    public float maxAcceleration;
    public float maxVelocity;
    
    public bool playerInHome = false;
    private List<AIShieldDrone> shieldDrones;
    private GameObject player;
    private GameObject Snitch;

    private int dronesAlive;

    // Use this for initialization
    void Start ()
    {
        dronesAlive = 0;
        shieldDrones = new List<AIShieldDrone>();
        player = GameObject.FindGameObjectWithTag("Player");
        
        for (int i = 0; i < transform.childCount; ++i)
        {
            if (transform.GetChild(i).tag == "Drone")
            {
                transform.GetChild(i).GetComponent<AIShieldDrone>().player = player;
                transform.GetChild(i).GetComponent<AIShieldDrone>().ctrlShieldDrones = this;
                shieldDrones.Add(transform.GetChild(i).GetComponent<AIShieldDrone>());
                ++dronesAlive;
            }else if (transform.GetChild(i).name == "Snitch")
            {
                Snitch = transform.GetChild(i).gameObject;
            }
        }
    }

    private void Update()
    {
        if (playerInHome == false)
        {
            if (Vector3.Distance(player.transform.position, transform.position) <= radioHomeEnter)
            {
                playerInHome = true;
            }
        }
        else
        {
            if (Vector3.Distance(player.transform.position, transform.position) >= radioHomeExit)
            {
                playerInHome = false;
            }
        }
        
    }

    public List<AIShieldDrone> getNeightbours(AIShieldDrone agent, float radious)
    {

        List<AIShieldDrone> neightbours = new List<AIShieldDrone>();
        foreach (var otherAgent in shieldDrones)
        {
            if (otherAgent != null && otherAgent != agent)
            {
                if (Vector3.Distance(agent.shieldTransform.position, otherAgent.shieldTransform.position) <= radious)
                {
                    neightbours.Add(otherAgent);
                }
            }

        }

        return neightbours;
    }

    public void dronKilled()
    {
        --dronesAlive;
        if (dronesAlive < 1)
        {
            Snitch.GetComponent<SphereCollider>().enabled = true;
        }
    }
}
