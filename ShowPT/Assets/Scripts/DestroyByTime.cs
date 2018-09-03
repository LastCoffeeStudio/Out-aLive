using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class DestroyByTime : MonoBehaviour {

	GameObject objectToDealWith;

	[SerializeField]
    float lifetime = 2f;
	float timer = 0f;

	enum destroyWhat
	{
		MYSELF,
		FATHER,
        SHIELD
	}
	[SerializeField]
	destroyWhat whatToDestroy = destroyWhat.MYSELF;

	enum destroyOptions
	{
		DESTROY,
		DISABLE
	}
	[SerializeField]
	destroyOptions destroyOrDisable = destroyOptions.DESTROY;

	[SerializeField]
	bool fade = true;
	[SerializeField]
	float fadeTime = 3f;

	Renderer[] listOfChildren;

	void Start()
	{
		listOfChildren = GetComponentsInChildren<Renderer>();
	}


    private void OnEnable()
    {
        switch (whatToDestroy)
        {
            case destroyWhat.SHIELD:
                GameObject gameObj = transform.parent.gameObject;
                transform.parent = null;
                Destroy(gameObj);
                break;
            default:
                break;
        }
    }

    void Update()
	{
	    
        timer += Time.deltaTime;

		if (timer >= lifetime) 
		{
		   
            if (fade == false) 
			{
				destroyMe ();
			} 
			else 
			{
				fadeAway ();
			}
		}
	}

	void destroyMe()
	{
		GameObject objectToDealWith;

	    switch (whatToDestroy)
	    {
            case destroyWhat.MYSELF:
                objectToDealWith = gameObject;
            break;
	        case destroyWhat.FATHER:
	            objectToDealWith = transform.parent.gameObject;
	            break;
	        case destroyWhat.SHIELD:
	            objectToDealWith = gameObject;
	            break;
            default:
                objectToDealWith = transform.parent.gameObject;
                break;
        }

		if (destroyOrDisable == destroyOptions.DESTROY) 
		{
			Destroy (objectToDealWith);
		} 
		else 
		{
			objectToDealWith.SetActive (false);
		}
	}

	void fadeAway()
	{
		if (timer < lifetime + fadeTime) 
		{
			float fadingCounter = timer - lifetime;
			float newAlpha = Mathf.Lerp (1, 0, fadingCounter / fadeTime);

			foreach (Renderer renderer in listOfChildren) 
			{
				foreach (Material material in renderer.materials)
				{
					if (material.HasProperty ("_Color")) 
					{
						Color newColor = new Color (material.color.r, material.color.g, material.color.b, newAlpha);
						material.SetColor ("_Color", newColor);
					}
				}
			}
		} 
		else 
		{
			destroyMe ();
		}
	}
}
