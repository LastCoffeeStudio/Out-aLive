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
    private bool alive = true;
    // Use this for initialization
    void Start()
    {
        rend = GetComponent<Renderer>();
        rend.material.shader = Shader.Find("Unlit/SphereSnitch");
        prog = 0.1f;


    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void apearBarrier()
    {
        StartCoroutine(_apearBarrier());
    }

    public void disolveBarrier(GameObject snitch)
    {
        StartCoroutine(_disolveBarrier(snitch));
    }


    private IEnumerator _apearBarrier()
    {
        while (prog < 0.6f)
        {
            prog += speedApear * Time.deltaTime;
            rend.material.SetFloat("_Progress", prog);
            yield return null;
        }
        StartCoroutine(moveLava());
        transform.parent.GetComponent<CtrlShieldDrones>().finishRotationAnimation();
    }

    private IEnumerator moveLava()
    {
        while (alive)
        {
            prog += speedLava * Time.deltaTime;
            rend.material.SetFloat("_Progress", prog);
            yield return null;
        }
    }

    private IEnumerator _disolveBarrier(GameObject snitch)
    {
        prog = 0.7f;
        alive = false;
        while (prog > 0.1f)
        {
            prog -= speedApear * Time.deltaTime;
            rend.material.SetFloat("_Progress", prog);
            yield return null;
        }
        snitch.GetComponent<SphereCollider>().enabled = true;
        gameObject.SetActive(false);
    }
}
