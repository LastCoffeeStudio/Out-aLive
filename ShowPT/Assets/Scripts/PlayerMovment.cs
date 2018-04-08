using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovment : MonoBehaviour {

    public float moveSpeed = 5f;
    public float runSpeed = 10f;
    public float agacSpeed = 2f;
    public float lookSensivity;
    public bool swagLook = false;
    public float jumpForce;
    public float groundDistance = 1.05f;
    public bool flyControl = true;
    public float axisSensitivity = 3f;
    public bool snap = false;
    public Camera cam;
    [Range(0.2f, 1f)]
    public float runStart = 0.2f;
    public LayerMask layerMask;

    private float xMov = 0f;
    private float yMov = 0f;
    private float xRot = 0f;
    private float yRot = 0f;
    private float localSpeed = 0f;
    private Vector3 velocity = Vector3.zero;
    private Vector3 rotation = Vector3.zero;
    private Rigidbody rb;
    private bool isGrounded;
    private bool jumps = false;
    private bool jumping = false;

    public float spread = 5.0f;          
    public float maxSpread = 10.0f;
    public float minSpread = 5.0f;
    public float spreadPerSecond = 30.0f;
    public float decreasePerSecond = 35.0f;

    public float smoothAgac = 0.05f;

    bool drawCrosshair = true;

    float width = 2f;      //Crosshair width
    float height = 10f;     //Crosshair height

    private Texture2D tex;
    private GUIStyle lineStyle;

    public Animator animator;

    private Vector3 positionCameraAgac;
    private Vector3 positionCameraOr = new Vector3(0, 0.5f, 0.29f);

	//noiseValue must be modified whenever the player makes noise. Higher values mean enemies will hear you from further away.
	//At the start of the update() method, reset noiseValue to 0.
	public float noiseValue = 0f;

    private bool screenAbove = true;

    [SerializeField]
	GameUI gameUI;

    void Start () {
        rb = GetComponent<Rigidbody>();
        //animator = gameObject.GetComponentInChildren<Animator>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        tex = Texture2D.whiteTexture;
        lineStyle = new GUIStyle();
        lineStyle.normal.background = tex;

        positionCameraAgac = new Vector3(cam.transform.localPosition.x, 0, cam.transform.localPosition.z);

    }

    //Calcule 
    private void FixedUpdate()
    {
        movePlayer();
        rotateCamera();
    }

    void Update ()
    {
		noiseValue = 0f;
		if (CtrlPause.gamePaused == false) 
		{
			checkInput ();
		}
    }

    void OnGUI()
    {
        var centerPoint = new Vector2(Screen.width / 2, Screen.height / 2);

        if (drawCrosshair)
        {
            GUI.Box(new Rect(centerPoint.x - width / 2, centerPoint.y - (height + spread), width, height), "", lineStyle);
            GUI.Box(new Rect(centerPoint.x - width / 2, centerPoint.y + spread, width, height), "", lineStyle);
            GUI.Box(new Rect(centerPoint.x + spread, (centerPoint.y - width / 2), height, width), "", lineStyle);
            GUI.Box(new Rect(centerPoint.x - (height + spread), (centerPoint.y - width / 2), height, width), "", lineStyle);
        }
    }

    public void setScreenAbove(bool screenAbove)
    {
        this.screenAbove = screenAbove;
    }

    private void checkInput()
    {
        xMov = getAxis("Horizontal", snap);
        yMov = getAxis("Vertical", snap);

        /*getAxis("Horizontal");
        getAxis("Vertical");*/

        if(xMov != 0 || yMov != 0)
        {
            spread += spreadPerSecond * Time.deltaTime;
        }

        xRot = Input.GetAxis("Mouse X");
        yRot = Input.GetAxis("Mouse Y");

        if (Input.GetKey(KeyCode.Mouse0))
        {
            /*
            if (Cursor.lockState != CursorLockMode.Locked)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            */
        	spread += spreadPerSecond * Time.deltaTime;
        }
        else
        {
            spread -= decreasePerSecond * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.LeftShift) && yMov > runStart && !animator.GetBool("reloading") && !animator.GetBool("shooting"))
        {
            localSpeed = runSpeed;
        }
        else
        {
            localSpeed = moveSpeed;
        }

        if (Input.GetKey(KeyCode.C) && !jumping)
        {
            localSpeed = agacSpeed;
            cam.transform.localPosition = Vector3.Lerp(cam.transform.localPosition, positionCameraAgac, Time.deltaTime * smoothAgac);
        }
        else
        {
            cam.transform.localPosition = Vector3.Lerp(cam.transform.localPosition, positionCameraOr, Time.deltaTime * smoothAgac);
        }

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded && !jumping)
        {
            jumps = true;
        }

        spread = Mathf.Clamp(spread, minSpread, maxSpread);
    }

    private void movePlayer()
    {
        RaycastHit hitInfo;
        isGrounded = Physics.Raycast(transform.position, -Vector3.up, out hitInfo, groundDistance, layerMask);
        Debug.DrawRay(transform.position, -Vector3.up, new Color(1f, 0f, 0f), 0.1f, true);
        if (GameManager.debugMode)
        {
            Debug.DrawRay(transform.position, -Vector3.up*groundDistance, new Color(255f,0f,0f));
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
                animator.SetFloat("speed", localSpeed);
                rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);
            }
            else
            {
                animator.SetFloat("speed", 0f);
            }
        }

        if (isGrounded)
        {
            Vector3 jumpVector = Vector3.zero;

            if (jumping)
            {
                jumping = false;
            }
            if (jumps)
            {
                jumps = false;
                jumping = true;
                jumpVector = Vector3.up * jumpForce;
                rb.AddForce(jumpVector * Time.fixedDeltaTime, ForceMode.Impulse);
            }
        }
    }

    private void rotateCamera()
    {
        //Calculate Rotation
        rotation = new Vector3(0f, xRot, 0f) * lookSensivity;

        if (rotation != Vector3.zero)
        {
            rb.MoveRotation(rb.rotation * Quaternion.Euler(rotation));
        }

        rotation = new Vector3(-yRot, 0f, 0f) * lookSensivity;

        if (rotation != Vector3.zero)
        {
            cam.transform.Rotate(rotation);
        }
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

	
}
