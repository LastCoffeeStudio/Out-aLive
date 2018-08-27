using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPropCtrl : MonoBehaviour
{
    private Transform targetPosition; // we have to add in the Inspector our target

    private void Start()
    {
        targetPosition = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (targetPosition != null && PlayerMovment.overrideControls == false)
        {
            transform.LookAt(targetPosition);
            transform.Rotate( new Vector3(-90.0f, 0.0f, 0.0f));
        }
    }
}