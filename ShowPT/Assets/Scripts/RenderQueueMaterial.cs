using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderQueueMaterial : MonoBehaviour {

    [Range(-1,5000)]
    public int priority;

    private List<Renderer> renderers;

    private void Start()
    {
        renderers = new List<Renderer>();
        Renderer parentRenderer = gameObject.GetComponent<Renderer>();

        if (parentRenderer != null)
        {
            renderers.Add(parentRenderer);
        }

        for (int i = 0; i < transform.childCount; ++i)
        {
            Renderer rend = transform.GetChild(i).GetComponent<Renderer>();

            if (rend != null)
            {
                renderers.Add(rend);
            }
        }

        for (int i = 0; i < renderers.Count; ++i)
        {
            Material[] mats = renderers[i].materials;

            for (int j = 0; j < mats.Length; ++j)
            {
                Debug.Log("Changes: " + i + " " + j);
                mats[j].renderQueue = priority;
            }
        }
    }

    private void Update()
    {
        
    }
}
