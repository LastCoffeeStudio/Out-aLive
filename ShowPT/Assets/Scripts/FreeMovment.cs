using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeMovment : MonoBehaviour {

    public float speed = 7.0f;
    public float minX = -360.0f;
    public float maxX = 360.0f;

    public float minY = -45.0f;
    public float maxY = 45.0f;

    public float sensX = 100.0f;
    public float sensY = 100.0f;

    float rotationY = 0.0f;
    float rotationX = 0.0f;

    private float runSpeed;
    // Use this for initialization
    void Start () {
        
    }
	
	// Update is called once per frame
	void Update ()
	{
	    runSpeed = speed;

        if (Input.GetKey(KeyCode.LeftShift))
	    {
	        runSpeed *= 3.0f;
	    }
        if (Input.GetKey(KeyCode.D))
	    {
	        transform.position += transform.right * runSpeed * Time.deltaTime;
	    }
	    if (Input.GetKey(KeyCode.A))
	    {
	        transform.position -= transform.right * runSpeed * Time.deltaTime;
	    }
	    if (Input.GetKey(KeyCode.W))
	    {
	        transform.position += transform.forward * runSpeed * Time.deltaTime;
	    }
	    if (Input.GetKey(KeyCode.S))
	    {
	        transform.position -= transform.forward * runSpeed * Time.deltaTime;
	    }
	    rotationX += Input.GetAxis("Mouse X") * sensX * Time.deltaTime;
	    rotationY += Input.GetAxis("Mouse Y") * sensY * Time.deltaTime;
	    rotationY = Mathf.Clamp(rotationY, minY, maxY);
	    transform.localEulerAngles = new Vector3(-rotationY, rotationX, 0);
    }
}
