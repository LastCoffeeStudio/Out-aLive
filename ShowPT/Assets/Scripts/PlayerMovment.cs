using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovment : MonoBehaviour
{
    public enum playerState
    {
        IDLE,
        WALKING,
        RUNNING,
        JUMPING
    }

	public static bool overrideControls = false;

    [Header("Speed Settings")]
    public float moveSpeed = 5f;
    public float runSpeed = 10f;
    public float agacSpeed = 2f;
    private float aimingSpeed = 2f;
    [HideInInspector]
    public float localSpeed = 0f;
    public float lookSensivity;
    public float cameraSmooth;
    public bool swagLook = false;
    public float jumpForce;
    public float groundDistance = 1.05f;
    public bool flyControl = true;
    public float axisSensitivity = 3f;
    public bool snap = false;
    public playerState state;
    public Camera cam;
    public float interactMaxDistance;
    [Range(0.2f, 1f)]
    public float runStart = 0.2f;
    public LayerMask layerMask;

    private float xMov = 0f;
    private float yMov = 0f;
    private float xRot = 0f;
    private float yRot = 0f;
    private Vector3 velocity = Vector3.zero;
    private Vector3 rotation = Vector3.zero;
    private Rigidbody rb;
    private bool isGrounded;
    private bool jumps = false;
    private bool jumping = false;

    public HudController hudController;

    [Header("Sounds")]
    public AudioCollection steps;
    public AudioCollection jumpsCollection;
    public AudioCollection fallCollection;
    private bool playFallSound = true;
    private float lastYposition;
    public Texture2D splatmap;
    private int indexAudioCol = 1;
    private Color red;
    private Color blue;
    private Color green;
    private Color purple;
    private CtrlAudio ctrlAudio;
    private float lastMovx = 0f;
    private bool increas = true;

    [Header("Crouch Settings")]
    [Range(0.1f, 0.9f)]
    public float crouchAmount;
    [HideInInspector]
    public bool crouched = false;
    public float timeToCrouch = 1f;
    private float crouchTime = 0f;
    private float actualCrouchAmount = 0f;
    private CapsuleCollider playerCollider;

    private Texture2D tex;
    private GUIStyle lineStyle;

    [HideInInspector]
    public Animator animator;
    private Vector3 originalCameraPos;
    private Vector3 originalCapsuleCenter;
    private float originalCapsuleHeight;

    private float zoom;
    private float zoomSpeed;
    private float originalZoom;

    //noiseValue must be modified whenever the player makes noise. Higher values mean enemies will hear you from further away.
    //At the start of the update() method, reset noiseValue to 0.
    [HideInInspector]
    public float noiseValue = 0f;
    //private bool screenAbove = true;

    [SerializeField]
    private GameUI gameUI;

    [Header("Walk Settings")]
    public AnimationCurve cameraMovementX;
    public AnimationCurve cameraMovementY;
    public float recoverTime;
    [Range(0f,1f)]
    public float movementMagnitude;
    private float cameraIndex;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerCollider = GetComponent<CapsuleCollider>();

        originalCapsuleCenter = playerCollider.center;
        originalCapsuleHeight = playerCollider.height;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        tex = Texture2D.whiteTexture;
        lineStyle = new GUIStyle();
        lineStyle.normal.background = tex;

        originalCameraPos = cam.transform.localPosition;
        ctrlAudio = GameObject.FindGameObjectWithTag("CtrlAudio").GetComponent<CtrlAudio>();

        state = playerState.IDLE;

        zoom = 50f;
        zoomSpeed = 10f;
        originalZoom = 60f;

        blue = splatmap.GetPixel(1, 1);
        red = splatmap.GetPixel(1, splatmap.width - 2);
        purple = splatmap.GetPixel(splatmap.width - 2, splatmap.width - 2);
        green = splatmap.GetPixel(splatmap.width - 2, 1);
    }

    public void PlayStep()
    {
        checkSelectedSound();
        ctrlAudio.playOneSound("Player", steps[indexAudioCol], transform.position, steps.volume, steps.spatialBlend, steps.priority);
    }

    public void checkSelectedSound()
    {
        //rate between terrain and splatmap;
        float rate = splatmap.width / 240f;

        //Get x and z for splatmap
        int x = (int)((transform.position.x + 100) * rate);
        int z = (int)((transform.position.z + 100) * rate);
        Color color = splatmap.GetPixel(x, z);
        if (color == green)
        {
            indexAudioCol = 2;
        }
        else if (color == purple)
        {
            indexAudioCol = 0;
        }
        else if (color == red)
        {
            indexAudioCol = 3;
        }
        else if (color == blue)
        {
            indexAudioCol = 1;
        }
    }

    //Calcule 
    private void FixedUpdate()
    {
		if (overrideControls == false) 
		{
			updatePlayer();
			updateCamera ();
		}
    }

    void Update()
    {
        float dt = Time.deltaTime;
        if (dt == 0)
        {
            dt = 10;
        }
        noiseValue = 0f;
        if (CtrlGameState.gameState == CtrlGameState.gameStates.ACTIVE)
        {
			if (overrideControls == false) 
			{
				checkInteract();
				checkInput ();
			}
        }
    }

    private void checkInteract()
    {
        RaycastHit hitInfo;

        int layer_mask = LayerMask.GetMask("InteractableObjects");

        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hitInfo, interactMaxDistance, layer_mask))
        {
            InteractableObject interactableObject = hitInfo.collider.gameObject.GetComponentInParent<InteractableObject>();

            if ((InteractableObjectsManager.interactableObjectsManagerinstance.ObjectActive == null ||
                !InteractableObjectsManager.equalsObjectActive(interactableObject)) && !interactableObject.getState().Equals(InteractableObject.InteractableObjectState.DISABLE))
            {
                if (InteractableObjectsManager.interactableObjectsManagerinstance.ObjectActive != null)
                {
                    InteractableObjectsManager.interactableObjectsManagerinstance.ObjectActive.enabled = false;
                }
                interactableObject.enabled = true;
                InteractableObjectsManager.interactableObjectsManagerinstance.ObjectActive = interactableObject;
				if (interactableObject.masterObject == InteractableObject.ObjectMaster.SAME_OBJECT) 
				{
					InteractableObjectsManager.showInteractableObject (hitInfo.collider.gameObject.transform.name);
				} 
				else 
				{
					InteractableObjectsManager.showInteractableObject (hitInfo.collider.gameObject.transform.parent.name);
				}
            }
        } else
        {
            if (InteractableObjectsManager.interactableObjectsManagerinstance.ObjectActive != null)
            {
                InteractableObjectsManager.interactableObjectsManagerinstance.ObjectActive.enabled = false;
                InteractableObjectsManager.interactableObjectsManagerinstance.ObjectActive = null;
                InteractableObjectsManager.hideInteractableObject();
            }
        }
    }

    /*public void setScreenAbove(bool screenAbove)
    {
        this.screenAbove = screenAbove;
    }*/

    private void checkInput()
    {
        if (Input.GetAxis("JoyLeftX") != 0)
        {
            xMov = Input.GetAxis("Horizontal");
        }
        else
        {
            xMov = getAxis("Horizontal", snap);
        }

        if (Input.GetAxis("Vertical") != 0)
        {
            yMov = Input.GetAxis("Vertical");
        }
        else
        {
            yMov = getAxis("Vertical", snap);
        }

        if (Input.GetAxis("JoyRightX") != 0)
        {
            xRot = Input.GetAxis("JoyRightX");
        }
        else
        {
            xRot = Input.GetAxis("Mouse X");
        }
        if (Input.GetAxis("JoyRightY") != 0)
        {
            yRot = Input.GetAxis("JoyRightY");
        }
        else
        {
            yRot = Input.GetAxis("Mouse Y");
        }

        if ((Input.GetKey(KeyCode.LeftShift) || Input.GetButton("ButtonL3")) && yMov > runStart && (animator == null || (!animator.GetBool("reloading") && !animator.GetBool("shooting") && !animator.GetBool("aiming"))) && (!jumping || localSpeed == runSpeed))
        {
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, originalZoom, Time.deltaTime * zoomSpeed);
            localSpeed = runSpeed;
        }
        else if (animator != null && animator.GetBool("aiming"))
        {
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, zoom, Time.deltaTime * zoomSpeed);
            localSpeed = aimingSpeed;
        }
        else 
        {
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, originalZoom, Time.deltaTime * zoomSpeed);
            localSpeed = moveSpeed;
        }
        if ((Input.GetButtonDown("ButtonB") || Input.GetKeyDown(KeyCode.C)) && !jumping)
        {
            updateCrouchState(!crouched);
        }
        if (crouched && !jumping)
        {
            hudController.setMenDown(true);
            localSpeed = agacSpeed;
        }
        else
        {
            hudController.setMenDown(false);
        }

        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("ButtonA")) && isGrounded && !jumping)
        {
            jumps = true;
            lastYposition = transform.position.y;
            checkSelectedSound();
            ctrlAudio.playOneSound(jumpsCollection.audioGroup, jumpsCollection[indexAudioCol], transform.position, jumpsCollection.volume, jumpsCollection.spatialBlend, jumpsCollection.priority);
            if (crouched)
            {
                updateCrouchState(false);
            }
        }
    }

    private void updateCrouchState(bool crouch)
    {
        crouched = crouch;
        crouchTime = timeToCrouch - crouchTime;
        crouchTime = Mathf.Clamp(crouchTime, 0f, timeToCrouch);
    }

    private void updatePlayer()
    {
        RaycastHit hitInfo;
        isGrounded = Physics.Raycast(transform.position, -Vector3.up, out hitInfo, groundDistance, layerMask);
        Debug.DrawRay(transform.position, -Vector3.up, new Color(1f, 0f, 0f), 0.1f, true);
        if (CtrlDebug.debugMode)
        {
            Debug.DrawRay(transform.position, -Vector3.up * groundDistance, new Color(255f, 0f, 0f));
        }

        if (flyControl || isGrounded)
        {
            Vector3 movHorizontal = transform.right * xMov;
            Vector3 movVertical = transform.forward * yMov;
            Vector3 mov = movHorizontal + movVertical;

            if (mov.magnitude > 1f)
            {
                mov = mov.normalized;
            }
            velocity = mov * localSpeed;

            if (velocity != Vector3.zero)
            {
                if (animator != null)
                {
                    animator.SetFloat("speed", localSpeed);
                }

                RaycastHit hit;
                bool hitbool = Physics.Raycast(transform.position, velocity, out hit, 1f);
                if (hitbool == false || (hit.transform.gameObject.layer !=  LayerMask.NameToLayer("Wall")
                                     && hit.transform.gameObject.layer != LayerMask.NameToLayer("LedsWall")
                                     && hit.transform.root.name != "prop_room"
                                     && hit.transform.root.name != "Scene"))
                {
                        Debug.DrawRay(transform.position, velocity, new Color(1f, 0f, 1f), 0.1f, true);
                    rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);
                }
                else
                {
                    Debug.DrawRay(transform.position, velocity, new Color(0f, 01f, 1f), 0.1f, true);
                }

                if (!jumping)
                {
                    if (localSpeed >= runSpeed)
                    {
                        state = playerState.RUNNING;
                    }
                    else
                    {
                        state = playerState.WALKING;
                    }
                }
            }
            else
            {
                if (animator != null)
                {
                    animator.SetFloat("speed", 0f);
                }

                if (!jumping)
                {
                    state = playerState.IDLE;
                }
            }
        }

        if (isGrounded)
        {
            Vector3 jumpVector = Vector3.zero;

            if (jumping)
            {
                playFallSound = true;
                jumping = false;
            }
            if (jumps)
            {
                jumps = false;
                jumping = true;
                jumpVector = Vector3.up * jumpForce;
                rb.AddForce(jumpVector * Time.fixedDeltaTime, ForceMode.Impulse);
                state = playerState.JUMPING;
            }
        }

        if (jumping)
        {
            if (lastYposition > transform.position.y)
            {
                RaycastHit hit;
                Debug.DrawRay(transform.position, Vector3.down, new Color(0.3f, 0.5f, 0.3f), 0.1f, true);
                bool hitbool = Physics.Raycast(transform.position, Vector3.down, out hit, 1.85f);
                if (playFallSound == true  && hitbool && (hit.transform.gameObject.layer == LayerMask.NameToLayer("Wall")
                                                                              || hit.transform.root.name == "Showroom"))
                {
                    playFallSound = false;
                    checkSelectedSound();
                    ctrlAudio.playOneSound(fallCollection.audioGroup, fallCollection[indexAudioCol], transform.position, fallCollection.volume, fallCollection.spatialBlend, fallCollection.priority);
                }
            }
            else
            {
                lastYposition = transform.position.y;
            }
          
        }
    }

    private void updateCamera()
    {
        //Calculate Rotation
        rotation = new Vector3(0f, xRot, 0f) * lookSensivity;

        if (rotation != Vector3.zero)
        {
            rb.rotation = Quaternion.Slerp(rb.rotation, rb.rotation * Quaternion.Euler(rotation), cameraSmooth * Time.deltaTime);
        }

        rotation = new Vector3(-yRot, 0f, 0f) * lookSensivity;

        if (rotation != Vector3.zero)
        {
            Quaternion finalCamRotation = ClampRotationAroundXAxis(cam.transform.localRotation * Quaternion.Euler(rotation));
            cam.transform.localRotation = Quaternion.Slerp(cam.transform.localRotation, finalCamRotation, cameraSmooth * Time.deltaTime);
        }

        //Update Camera
        if (crouched && !jumping)
        {
            if (crouchTime < timeToCrouch)
            {
                if (actualCrouchAmount != crouchAmount) actualCrouchAmount = Mathf.Lerp(0f, crouchAmount, crouchTime / timeToCrouch);
                crouchTime += Time.deltaTime;
            }
            playerCollider.center = new Vector3(originalCapsuleCenter.x, originalCapsuleCenter.y - (originalCapsuleHeight * crouchAmount) / 2f, originalCapsuleCenter.z);
            playerCollider.height = originalCapsuleHeight - originalCapsuleHeight * crouchAmount;
        }
        else
        {
            if (crouchTime < timeToCrouch)
            {
                if(actualCrouchAmount != 0) actualCrouchAmount = Mathf.Lerp(crouchAmount, 0f, crouchTime / timeToCrouch);
                crouchTime += Time.deltaTime;
            }
            playerCollider.center = originalCapsuleCenter;
            playerCollider.height = originalCapsuleHeight;
        }
        
        //Update CameraPivot
        if (!jumping && velocity.magnitude > 0.5f)
        {
            float movementFactor = 4f;

            if (localSpeed == moveSpeed || localSpeed == agacSpeed)
            {
                movementFactor = 3f;
            }
            cameraIndex += Time.deltaTime * velocity.magnitude / movementFactor;

            Keyframe k = cameraMovementX[cameraMovementX.length - 1];
            while (cameraIndex > k.time)
            {
                cameraIndex -= k.time;
            }
        }
        else
        {
            if (cameraIndex != 0f)
            {
                Keyframe k = cameraMovementX[cameraMovementX.length - 1];
                
                float nextCameraIndex = cameraIndex + Time.deltaTime * 2f;
                if (cameraIndex >= k.time || (cameraIndex < k.time / 2f && nextCameraIndex > k.time / 2f))
                {
                    cameraIndex = 0f;
                }
                else
                {
                    cameraIndex = nextCameraIndex;
                }
            }
        }

        if (jumping == false && increas == true && lastMovx > cameraMovementX.Evaluate(cameraIndex) )
        {
            increas = false;
            PlayStep();
        }
        else if(jumping == false && increas == false && lastMovx < cameraMovementX.Evaluate(cameraIndex))
        {
            increas = true;
            PlayStep();
        }
        lastMovx = cameraMovementX.Evaluate(cameraIndex);
        float movementX = cameraMovementX.Evaluate(cameraIndex) * movementMagnitude;
        float movementY = cameraMovementY.Evaluate(cameraIndex) * movementMagnitude;
        cam.transform.localPosition = originalCameraPos + new Vector3(movementX, originalCapsuleHeight * -actualCrouchAmount + movementY, 0f);
    }

    /*If axis is a valid Axis, returns a value between -1 and 1*/
    /*If axis is not a valid Axis, the return value is 0*/
    private float getAxis(string axis, bool snap)
    {
        float displacement = 0f;
        KeyCode axis1 = KeyCode.None;
        KeyCode axis2 = KeyCode.None;
        switch (axis)
        {
            case "Horizontal":
                displacement = xMov;
                axis1 = KeyCode.A;
                axis2 = KeyCode.D;
                break;

            case "Vertical":
                displacement = yMov;
                axis1 = KeyCode.S;
                axis2 = KeyCode.W;
                break;
        }

        if ((Input.GetKey(axis1) && Input.GetKey(axis2)) || (!Input.GetKey(axis1) && !Input.GetKey(axis2)))
        {
            if (displacement > 0)
            {
                displacement -= axisSensitivity * Time.deltaTime;
                if (displacement < 0)
                {
                    displacement = 0f;
                }
            }
            else if (displacement < 0)
            {
                displacement += axisSensitivity * Time.deltaTime;
                if (displacement > 0)
                {
                    displacement = 0f;
                }
            }
            if (snap)
            {
                displacement = 0f;
            }
        }
        else if (Input.GetKey(axis1))
        {
            displacement -= axisSensitivity * Time.deltaTime;
            if (snap && displacement > 0f)
            {
                displacement = 0f;
            }
            if (displacement < -1f)
            {
                displacement = -1f;
            }
        }
        else if (Input.GetKey(axis2))
        {
            displacement += axisSensitivity * Time.deltaTime;
            if (snap && displacement < 0f)
            {
                displacement = 0f;
            }
            if (displacement > 1f)
            {
                displacement = 1f;
            }
        }

        return displacement;
    }

    Quaternion ClampRotationAroundXAxis(Quaternion q)
    {
        q.x /= q.w;
        q.y /= q.w;
        q.z /= q.w;
        q.w = 1.0f;

        float angleX = 2.0f * Mathf.Rad2Deg * Mathf.Atan(q.x);

        angleX = Mathf.Clamp(angleX, -90f, 90f);

        q.x = Mathf.Tan(0.5f * Mathf.Deg2Rad * angleX);

        return q;
    }
}
