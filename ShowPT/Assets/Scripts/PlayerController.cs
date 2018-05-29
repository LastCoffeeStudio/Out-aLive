using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
public class PlayerController : MonoBehaviour {

    public float maxSpeed;
    public Rigidbody characterRigidbody = GameObject.Find("PlayerNew").GetComponent<Rigidbody>();

	private void Start ()
    {
        characterRigidbody = gameObject.GetComponent<Rigidbody>();
	}
	
	private void Update ()
    {
        handleInput();
	}

    private void handleInput()
    {
        if (Input.GetKey(KeyCode.W))
        {
            transform.Translate(new Vector3(0, 0, 1));
        }
        else if (Input.GetKey(KeyCode.A))
        {
            transform.Translate(new Vector3(-1, 0, 0));
        }
        else if (Input.GetKey(KeyCode.S))
        {
            transform.Translate(new Vector3(0, 0, -1));
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Translate(new Vector3(1, 0, 0));
        }
    }
}
