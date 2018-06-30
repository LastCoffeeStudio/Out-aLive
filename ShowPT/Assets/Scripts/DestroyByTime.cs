using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyByTime : MonoBehaviour {

	GameObject objectToDealWith;

	[SerializeField]
    float lifetime = 2f;
	float timer = 0f;

	enum destroyWhat
	{
		MYSELF,
		FATHER
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

		if (whatToDestroy == destroyWhat.FATHER) 
		{
			objectToDealWith = transform.parent.gameObject;
		} 
		else 
		{
			objectToDealWith = gameObject;
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
