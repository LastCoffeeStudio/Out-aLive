using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericEvent : MonoBehaviour {

    public enum EventType
    {
        WELCOMEZONE = 0,
        FIRSTORRET = 1,
        FIRSTLIL = 2,
        MIDTIME = 3,
        DRONES = 4,
        BRIDGE = 5,
        PRESENTATIONBOSS = 6,
        BOSSFIGHT = 7,
        PLAYERDEAD = 8
    }

    public EventType type;

    public virtual void onEnableEvent() { }
}
