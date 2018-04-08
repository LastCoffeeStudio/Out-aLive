using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentConfig : MonoBehaviour
{

    public float radioCohesion;
    public float radioSeparation;
    public float radioAligment;
    public float radioAvoid;

    public float KCohesion;
    public float KSeparation;
    public float KAligment;
    public float KWonder;
    public float KAvoid;
    public float KPlayer;
    public float KMinH;

    public float maxAcceleration;
    public float maxVelocity;
    public float maxFieldOfViewAngle = 180;

    public float wanderJitter;
    public float wanderRadius;
    public float wanderDistance;
}
