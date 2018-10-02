using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public GameObject cameraPivot;

    Vector3 originalCameraPosition;
    List<Shake> activeShakes;

    private void Start()
    {
        originalCameraPosition = cameraPivot.transform.localPosition;
        activeShakes = new List<Shake>();
    }

    // Update is called once per frame
    void Update ()
    {
		if (PlayerMovment.overrideControls == false) 
		{
			/*if (Input.GetKeyDown (KeyCode.H)) 
			{
				startShake (10f, 0.1f, 0.2f, 10, 0.5f);
			}*/

			Vector3 cameraPivotPosition = new Vector3 ();

			for (int i = 0; i < activeShakes.Count; ++i) 
			{
				cameraPivotPosition += activeShakes [i].shakeCamera ();

				if (activeShakes [i].state == Shake.ShakeState.END) 
				{
					activeShakes.RemoveAt (i);
					if (activeShakes.Count == 0) 
					{
						cameraPivot.transform.localPosition = originalCameraPosition;
					}
				}
			}

			cameraPivot.transform.localPosition = cameraPivotPosition;
		}
    }

    public void startShake(float shakeTime, float timeToFadeIn, float timeToFadeOut, float speed, float magnitude)
    {
        Shake shake = new Shake(shakeTime, timeToFadeIn, timeToFadeOut, speed, magnitude, cameraPivot.transform.localPosition.z);
        activeShakes.Add(shake);
    }
}
