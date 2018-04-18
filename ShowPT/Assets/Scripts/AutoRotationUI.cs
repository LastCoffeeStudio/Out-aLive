using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoRotationUI : MonoBehaviour
{

    public float speed = 20f;

	// Update is called once per frame
	void Update () {

	   gameObject.transform.Rotate(Vector3.back, speed*Time.deltaTime);
    }
}
