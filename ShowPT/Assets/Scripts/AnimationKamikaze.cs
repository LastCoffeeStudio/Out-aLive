using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AnimationKamikaze : MonoBehaviour {

    private NavMeshAgent navAgent = null;
    private Animator animator = null;
    private float smoothAngle = 0.0f;

    // Use this for initialization
    void Start () {
        // Cache NavMeshAgent Reference
        navAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void Update () {

	    // Transform agents desired velocity into local space
	    Vector3 localDesiredVelocity = transform.InverseTransformVector(navAgent.desiredVelocity);

	    // Get angle in degrees we need to turn to reach the desired velocity direction
	    float angle = Mathf.Atan2(localDesiredVelocity.x, localDesiredVelocity.z) * Mathf.Rad2Deg;

	    // Smoothly interpolate towards the new angle
	    smoothAngle = Mathf.MoveTowardsAngle(smoothAngle, angle, 80.0f * Time.deltaTime);

	    // Speed is simply the amount of desired velocity projected onto our own forward vector
	    float speed = localDesiredVelocity.z;

	    // Set animator parameters
	   // animator.SetFloat("Angle", smoothAngle);
	    animator.SetFloat("Speed", speed, 0.1f, Time.deltaTime);
    }
}
