using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyByTime : MonoBehaviour {

    public float lifetime = 2f;
    void Start()
    {
        Destroy(gameObject, lifetime);
    }
}
