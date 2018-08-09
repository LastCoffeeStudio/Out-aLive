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

	void Update()
	{
		timer += Time.deltaTime;

		if (timer >= lifetime) 
		{
			doIt ();
		}
	}

	void doIt()
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
	            objectToDealWith = transform.parent.gameObject;
	            break;
            default:
                objectToDealWith = transform.parent.parent.parent.parent.gameObject;
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
}
