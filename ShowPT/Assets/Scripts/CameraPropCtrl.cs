using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPropCtrl : MonoBehaviour
{
    public Transform targetPosition; // we have to add in the Inspector our target

    void Update()
    {
        if (targetPosition != null)
        {
            transform.LookAt(targetPosition);
        }
    }
}