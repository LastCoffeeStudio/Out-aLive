using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourceMachineActivationController : MonoBehaviour
{
    public Animator resourceMachineAnim;
    public float minDistance;

    private GameObject player;

    void Start ()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        if (!resourceMachineAnim.GetBool("MachineOn") && Vector3.Distance(resourceMachineAnim.transform.position, player.transform.position) <= minDistance)
        {
            resourceMachineAnim.SetBool("MachineOn", true);
        }
    }
}
