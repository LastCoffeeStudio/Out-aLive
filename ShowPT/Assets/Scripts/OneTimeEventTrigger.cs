using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneTimeEventTrigger : MonoBehaviour
{
    public GenericEvent.EventType[] eventsType;

    private List<GenericEvent> events;
    private bool triggered = false;
    private GameObject eventsContainer;

    private void Start()
    {
        events = new List<GenericEvent>();
        eventsContainer = GameObject.FindGameObjectWithTag("EventsContainer");
        if (eventsContainer != null)
        {
            getEvents();
        }
    }

    private void getEvents()
    {
        GenericEvent[] containerEvents = eventsContainer.GetComponents<GenericEvent>();
        for (int i = 0; i < eventsType.Length; ++i)
        {
            for (int j = 0; j < containerEvents.Length; ++j)
            {
                if (containerEvents[j].type == eventsType[i])
                {
                    events.Add(containerEvents[j]);
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!triggered && other.tag == "Player")
        {
            for (int i = 0; i < events.Count; ++i)
            {
                events[i].onEnableEvent();
            }

            triggered = true;
        }
    }
}
