using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericEvent : MonoBehaviour {

    public enum EventType
    {
        LILEVENT,
        TURRETEVENT
    }

    public EventType type;

    public virtual void onEnableEvent() { }
}
