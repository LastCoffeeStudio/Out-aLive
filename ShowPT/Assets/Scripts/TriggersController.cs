using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggersController : MonoBehaviour {

    public GlobalTimer globalTimer;
    
    private void OnTriggerEnter(Collider other)
    {
        if (gameObject.tag == "TimerTrigger" && other.tag == "Player")
        {
            Debug.Assert(globalTimer != null);
            globalTimer.activateTimers();
        }
    }
}
