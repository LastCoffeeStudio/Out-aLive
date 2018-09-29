using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeAppearTintColor: MonoBehaviour
{
    private Renderer renderer;
    private float alpha = 0f;
    public float progresionLot = 1.4f;
    private CtrlShieldDrones ctrlShieldDrones;

	// Use this for initialization
	void Start ()
	{
	    ctrlShieldDrones = gameObject.transform.root.GetComponent<CtrlShieldDrones>();
	    ctrlShieldDrones.appearFade += playFade;
        renderer = GetComponent<Renderer>();
	    renderer.material.SetFloat("_Mode", 2f);
        setAlpha();
    }

    public void playFade()
    {
        StartCoroutine(increaseAlphaToOne());
    }

    private void setAlpha()
    {
        Color color = renderer.material.GetColor("_TintColor");
        color.a = alpha;
        renderer.material.SetColor("_TintColor", color);
    }

    IEnumerator increaseAlphaToOne()
    {
        while (alpha < 0.196f)
        {
            alpha += progresionLot * Time.deltaTime;
            setAlpha();
            yield return null;
        }
        alpha = 0.196f;
        renderer.material.SetFloat("_Mode", 0f);
        setAlpha();
    }
}
