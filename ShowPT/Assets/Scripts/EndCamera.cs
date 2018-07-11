using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndCamera : MonoBehaviour {

    public GameObject[] path;
    public float minDistance;
    public float translationSpeed;
    public float rotationSpeed;

    private uint actual;
    private uint next;
    private Vector3 lastPosition;
    private Quaternion lastRotation;
    private float positionFactor;
    private float rotationFactor;

    // Use this for initialization
    void Start ()
    {
        actual = (uint)Random.Range(0, path.Length);
        positionFactor = 0f;
        rotationFactor = 0f;
        transform.position = path[actual].transform.position;
        transform.rotation = path[actual].transform.rotation;
        lastPosition = transform.position;
        lastRotation = transform.rotation;
        next = takeNextPoint();
	}

    private void FixedUpdate()
    {
        positionFactor += Time.deltaTime * translationSpeed;
        rotationFactor += Time.deltaTime * rotationSpeed;
        transform.position = Vector3.Lerp(lastPosition, path[next].transform.position, positionFactor);
        transform.rotation = Quaternion.Slerp(lastRotation, path[next].transform.rotation, rotationFactor);
    }

    // Update is called once per frame
    void Update () {
		if (path.Length > 0 && Vector3.Distance(transform.position,path[next].transform.position) < minDistance)
        {
            actual = next;
            lastPosition = transform.position;
            lastRotation = transform.rotation;
            positionFactor = 0f;
            rotationFactor = 0f;
            next = takeNextPoint();
        }
	}

    private uint takeNextPoint()
    {
        if (actual == path.Length - 1)
        {
            return 0;
        }
        return actual + 1;
    }
}
