using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventTrigger : MonoBehaviour {

    [SerializeField]
    private GenericEvent[] events;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            for (int i = 0; i < events.Length; ++i)
            {
                events[i].onEnableEvent();
            }
        }
    }
}
