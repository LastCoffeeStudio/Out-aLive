using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereSnitchController : MonoBehaviour
{
    [Range(0.0f,1.0f)]
    public float speedApear;
    public float speedLava;
    private Renderer rend;
    private float prog;
    // Use this for initialization
    void Start()
    {
        rend = GetComponent<Renderer>();
        rend.material.shader = Shader.Find("Unlit/SphereSnitch");
        prog = 0.0f;


    }

    // Update is called once per frame
    void Update()
    {
        prog += speedApear * Time.deltaTime;
        rend.material.SetFloat("_Progress", prog);
        if (prog > 0.6f && prog < 2f)
        {
            speedApear = speedLava;
        }
    }
}
