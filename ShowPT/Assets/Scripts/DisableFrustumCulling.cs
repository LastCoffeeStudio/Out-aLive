using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableFrustumCulling : MonoBehaviour {

    private Camera weaponsCamera;

	// Use this for initialization
	void Start ()
    {
        weaponsCamera = GetComponent<Camera>();
    }

    void OnPreCull()
    {
        Vector3 originPos = transform.position;
        transform.position += -10f * transform.forward;
        weaponsCamera.cullingMatrix = weaponsCamera.projectionMatrix * weaponsCamera.worldToCameraMatrix;
        transform.position = originPos;
    }
}
