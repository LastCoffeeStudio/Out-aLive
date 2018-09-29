using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeAppearColor: MonoBehaviour
{
    private Renderer renderer;
    private float alpha = 0f;
    public float progresionLot = 0.3f;
    private CtrlShieldDrones ctrlShieldDrones;

	// Use this for initialization
	void Start ()
	{
	    ctrlShieldDrones = gameObject.transform.root.GetComponent<CtrlShieldDrones>();
	    ctrlShieldDrones.appearFade += playFade;
        renderer = GetComponent<Renderer>();
        setAlpha();
    }

    public void playFade()
    {
        StartCoroutine(increaseAlphaToOne());
    }

    private void setAlpha()
    {
        Color color = renderer.material.GetColor("_Color");
        color.a = alpha;
        renderer.material.SetColor("_Color", color);
    }

    IEnumerator increaseAlphaToOne()
    {
        while (alpha < 1f)
        {
            alpha += progresionLot * Time.deltaTime;
            setAlpha();
            yield return null;
        }
        renderer.material.SetFloat("_Mode", 0f);
        renderer.material.SetFloat("_Mode", 0);
        renderer.material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
        renderer.material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
        renderer.material.SetInt("_ZWrite", 1);
        renderer.material.DisableKeyword("_ALPHATEST_ON");
        renderer.material.DisableKeyword("_ALPHABLEND_ON");
        renderer.material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        renderer.material.renderQueue = -1;
    }
}
