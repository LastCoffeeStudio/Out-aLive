using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CtrlDrones : MonoBehaviour
{
    //Distance betwen Target and player for drones start to search player.
    public float radioHomeEnter;
    //Distance betwen Target and player for drones comeback to target.
    public float radioHomeExit;
    public List<Drone> drones;
    public GameObject target;
    public SnitchDrone snitchDrone;
    private GameObject player;
    [HideInInspector]
    public bool playerInHome = false;
    private float OriginalRadioTargetConf;
    private DronesConfig dronesConfig;
    //***ONLY DOR DEBUG***//
    //
    //public bool debugWonder = false;
    //
    //********************//

    //In fact are target variables
    

    void Start ()
	{
	    drones = new List<Drone>();
	    dronesConfig = GetComponent<DronesConfig>();

        for (int i = 0; i < transform.childCount; ++i)
	    {
	        if (transform.GetChild(i).tag == "Drone")
	        {
	            transform.GetChild(i).GetComponent<Drone>().target = target;
	            transform.GetChild(i).GetComponent<Drone>().ctrlDrones = this;
	            transform.GetChild(i).GetComponent<Drone>().conf = dronesConfig;
	            drones.Add(transform.GetChild(i).GetComponent<Drone>());
            }
	    }

	    snitchDrone.ctrlDrones = this;
	    snitchDrone.targetTransform = target.transform;
	    player = GameObject.FindGameObjectWithTag("Player");
	    OriginalRadioTargetConf = dronesConfig.radiousTarget;

	}

    void Update()
    {
       //If the player is near of Target.
       if(!playerInHome && radioHomeEnter > Mathf.Abs(Vector3.Distance(target.transform.position, player.transform.position)))
       {
           playerInHome = true;
           updateTargetDrones(player);
           dronesConfig.radiousTarget = dronesConfig.radiousPlayer;
       }
       else if (playerInHome && radioHomeExit < Mathf.Abs(Vector3.Distance(target.transform.position, player.transform.position)))
       {
           playerInHome = false;
           updateTargetDrones(target);
           dronesConfig.radiousTarget = OriginalRadioTargetConf;
       }
    }

    private void updateTargetDrones(GameObject gameObj)
    {
        for (int i = 0; i < transform.childCount; ++i)
        {
            if (transform.GetChild(i).tag == "Drone")
            {
                transform.GetChild(i).GetComponent<Drone>().target = gameObj;
            }
        }
    }

    public List<Drone> getNeightbours(Drone agent, float radious)
    {

        List<Drone> neightbours = new List<Drone>();
        foreach (var otherAgent in drones)
        {
            if (otherAgent != null && otherAgent != agent)
            {
                if (Vector3.Distance(agent.position, otherAgent.position) <= radious)
                {
                    neightbours.Add(otherAgent);
                }
            }

        }

        return neightbours;
    }

    public List<Drone> getAllNeightbours()
    {

        List<Drone> neightbours = new List<Drone>();
        foreach (var otherAgent in drones)
        {
            if (otherAgent != null)
            {
               
                    neightbours.Add(otherAgent);
            }

        }

        return neightbours;
    }
}
