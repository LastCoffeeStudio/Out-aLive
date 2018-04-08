using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CtrlCamerasWin : MonoBehaviour {

    public GameObject[] endCameras;
    public float timeEndCamera;

    private uint activeEndCamera;
    private float timerEndCamera;

    // Use this for initialization
    void Start ()
    {
        enabled = false;
        if (endCameras.Length < 0)
        {
            timerEndCamera = 0f;
            activeEndCamera = (uint)Random.Range(0, endCameras.Length);
        }
    }
	
	// Update is called once per frame
	void Update () {
	    timerEndCamera -= Time.deltaTime;
	    if (timerEndCamera <= 0f)
	    {
	        if (endCameras.Length > 0)
	        {
	            endCameras[activeEndCamera].SetActive(false);
	            activeEndCamera = (uint)Random.Range(0, endCameras.Length);
	            endCameras[activeEndCamera].SetActive(true);
	        }

	        timerEndCamera = timeEndCamera;
	    }
    }
}
