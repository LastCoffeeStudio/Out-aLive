using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventsController : MonoBehaviour {

	public struct EventData
    {

    }

    public static EventsController eventsControllerInstance;

    private void Start()
    {
        if (eventsControllerInstance == null)
        {
            eventsControllerInstance = this;
        }
        else if (eventsControllerInstance != this)
        {
            Destroy(gameObject);
        }
    }
}
