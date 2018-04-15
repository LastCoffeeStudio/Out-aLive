using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CtrlDrones : MonoBehaviour
{


    public List<Drone> drones;
    public GameObject target;
    public SnitchDrone snitchDrone;

    //***ONLY DOR DEBUG***//
    //
    //public bool debugWonder = false;
    //
    //********************//

    //In fact are target variables
    

    void Start ()
	{
	    drones = new List<Drone>();
	    for (int i = 0; i < transform.childCount; ++i)
	    {
	        if (transform.GetChild(i).tag == "Drone")
	        {
	            transform.GetChild(i).GetComponent<Drone>().target = target;
	            transform.GetChild(i).GetComponent<Drone>().ctrlDrones = this;
	            transform.GetChild(i).GetComponent<Drone>().conf = GetComponent<DronesConfig>();
	            drones.Add(transform.GetChild(i).GetComponent<Drone>());
            }
	    }

	    snitchDrone.ctrlDrones = this;


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
