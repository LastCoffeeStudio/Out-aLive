using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeForceExplodeShield : MonoBehaviour {

    public void forceExplode()
    {
        GetComponentInChildren<ShieldDroneEnemy>().explode();
    }
}
