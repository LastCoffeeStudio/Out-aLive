using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIMisileSnitch : MonoBehaviour
{
    [HideInInspector]
    public AISnitch aiSnitch;
    private Transform originalTransform;

    // Use this for initialization
    void Start ()
    {
        originalTransform = transform;
    }
	
	// Update is called once per frame
	void Update ()
	{
	    
	}

    public void blast()
    {
        GetComponent<Animation>().Play(AnimationPlayMode.Stop);
    }

    public void finishBlast()
    {
        GetComponent<Animation>().Stop();
        transform.position = originalTransform.position;
        transform.rotation = originalTransform.rotation;
        aiSnitch.misileFree(gameObject);
    }
}
