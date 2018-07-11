using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CtrlCamerasWin : MonoBehaviour {
    
    [System.Serializable]
    public struct endCamera
    {
        public GameObject cameraObject;
        
        public GameObject[] path;
        public float minDistance;
        public float translationSpeed;
        public float rotationSpeed;

        [HideInInspector]
        public uint actual;
        [HideInInspector]
        public uint next;
        [HideInInspector]
        public Vector3 lastPosition;
        [HideInInspector]
        public Quaternion lastRotation;
        [HideInInspector]
        public float positionFactor;
        [HideInInspector]
        public float rotationFactor;
    }
    
    public endCamera[] endCameras;
    public float timeEndCamera;
    
    private uint activeEndCamera;
    private float timerEndCamera;

    // Use this for initialization
    void Start ()
    {
        if (endCameras.Length > 0)
        {
            timerEndCamera = 0f;
            activeEndCamera = (uint)Random.Range(0, endCameras.Length);
            endCameras[activeEndCamera].cameraObject.SetActive(true);
        }

        for (int i = 0; i < endCameras.Length; ++i)
        {
            endCameras[i].actual = (uint)Random.Range(0, endCameras[i].path.Length);
            endCameras[i].positionFactor = 0f;
            endCameras[i].rotationFactor = 0f;
            endCameras[i].cameraObject.transform.position = endCameras[i].path[endCameras[i].actual].transform.position;
            endCameras[i].cameraObject.transform.rotation = endCameras[i].path[endCameras[i].actual].transform.rotation;
            endCameras[i].lastPosition = endCameras[i].cameraObject.transform.position;
            endCameras[i].lastRotation = endCameras[i].cameraObject.transform.rotation;
            endCameras[i].next = takeNextPoint(endCameras[i]);
        }
    }

    private void FixedUpdate()
    {
        if (endCameras.Length > 0)
        {
            endCamera cam = endCameras[activeEndCamera];

            endCameras[activeEndCamera].positionFactor += Time.deltaTime * cam.translationSpeed;
            endCameras[activeEndCamera].rotationFactor += Time.deltaTime * cam.rotationSpeed;
            endCameras[activeEndCamera].cameraObject.transform.position = Vector3.Lerp(cam.lastPosition, cam.path[cam.next].transform.position, cam.positionFactor);
            endCameras[activeEndCamera].cameraObject.transform.rotation = Quaternion.Slerp(cam.lastRotation, cam.path[cam.next].transform.rotation, cam.rotationFactor);
        }
    }

    // Update is called once per frame
    void Update ()
    {
        updateActiveCamera();
        checkNextCamera();
    }

    private void updateActiveCamera()
    {
        endCamera cam = endCameras[activeEndCamera];
        if (cam.path.Length > 0 && Vector3.Distance(cam.cameraObject.transform.position, cam.path[cam.next].transform.position) < cam.minDistance)
        {
            endCameras[activeEndCamera].actual = cam.next;
            endCameras[activeEndCamera].lastPosition = cam.cameraObject.transform.position;
            endCameras[activeEndCamera].lastRotation = cam.cameraObject.transform.rotation;
            endCameras[activeEndCamera].positionFactor = 0f;
            endCameras[activeEndCamera].rotationFactor = 0f;
            endCameras[activeEndCamera].next = takeNextPoint(cam);
        }
    }

    private void checkNextCamera()
    {
        timerEndCamera -= Time.deltaTime;
        if (timerEndCamera <= 0f)
        {
            if (endCameras.Length > 0)
            {
                endCameras[activeEndCamera].cameraObject.SetActive(false);

                uint newActiveCamera = activeEndCamera;
                while (endCameras.Length >= 2 && newActiveCamera == activeEndCamera)
                {
                    newActiveCamera = (uint)Random.Range(0, endCameras.Length);
                }
                activeEndCamera = newActiveCamera;

                endCameras[activeEndCamera].cameraObject.SetActive(true);
            }

            timerEndCamera = timeEndCamera;
        }
    }

    private uint takeNextPoint(endCamera camera)
    {
        if (camera.actual == camera.path.Length - 1)
        {
            return 0;
        }
        return camera.actual + 1;
    }
}
