using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Crosshair : MonoBehaviour
{
    public enum CrosshairType
    {
        CROSS,
        CIRCLE
    }

    [Header("Crosshair properties")]
    public CrosshairType type;
    public Image[] lines;
    public float startingSpread;
    public float minSpread;
    public float maxSpread;
    public float walkFactorSpread;
    public float runFactorSpread;
    [Range(0,1)]
    public float crouchFactorSpread;
    public float jumpFactorSpread;
    public float increaseSpeed;
    public float decreaseSpeed;
    public bool isFixed;

    private float spread = 0f;
    private Vector3[] initialSpreads;
    private Camera playerCamera;
    private PlayerMovment playerStats;

    private void Start()
    {
        playerCamera = GameObject.FindGameObjectWithTag("CameraPlayer").GetComponent<Camera>();
        playerStats = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovment>();

        initialSpreads = new Vector3[lines.Length];
        for (int i = 0; i < lines.Length; ++i)
        {
            initialSpreads[i] = lines[i].rectTransform.localPosition;
        }
    }

    private void Update()
    {
        if (!isFixed)
        {
            updateSpread();
        }
        updateCrosshair();
    }

    private void updateSpread()
    {
        float mainSpread = startingSpread;
        if (playerStats.crouched)
        {
            mainSpread -= startingSpread * crouchFactorSpread;
        }

        switch (playerStats.state)
        {
            case PlayerMovment.playerState.WALKING:
                if (spread > minSpread * walkFactorSpread)
                {
                    spread = Mathf.Lerp(spread, mainSpread * walkFactorSpread, decreaseSpeed * Time.deltaTime);
                }
                else
                {
                    spread = Mathf.Lerp(spread, mainSpread * walkFactorSpread, increaseSpeed * Time.deltaTime);
                }
                break;
            case PlayerMovment.playerState.RUNNING:
                spread = Mathf.Lerp(spread, mainSpread * runFactorSpread, increaseSpeed * Time.deltaTime);
                break;
            case PlayerMovment.playerState.JUMPING:
                spread = Mathf.Lerp(spread, mainSpread * jumpFactorSpread, increaseSpeed * Time.deltaTime);
                break;
            case PlayerMovment.playerState.IDLE:
                spread = Mathf.Lerp(spread, mainSpread, decreaseSpeed * Time.deltaTime);
                break;
        }
        spread = Mathf.Clamp(spread, minSpread, maxSpread);
    }

    private void updateCrosshair()
    {
        lines[0].rectTransform.localPosition = initialSpreads[0] + new Vector3(0, spread, 0);
        lines[1].rectTransform.localPosition = initialSpreads[1] + new Vector3(-spread, 0, 0);
        lines[2].rectTransform.localPosition = initialSpreads[2] + new Vector3(0, -spread, 0);
        lines[3].rectTransform.localPosition = initialSpreads[3] + new Vector3(spread, 0, 0);
    }

    /*Returns a ray from the camera inside the area of the crosshair*/
    public Ray getRayCrosshairArea()
    {

        Debug.Log(playerCamera);
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        Vector2 centerPoint = playerCamera.ViewportToScreenPoint(new Vector3(0.5f, 0.5f, 0f));
        Vector2 scaledSize = Vector2.Scale(lines[0].rectTransform.rect.size, lines[0].rectTransform.lossyScale);
        Vector3 position = centerPoint;

        if (!isFixed)
        {
            switch (type)
            {
                case CrosshairType.CROSS:

                    //With pivot at center!
                    position = centerPoint + Random.insideUnitCircle * (((float)Screen.height / (float)Screen.currentResolution.height) * lines[0].rectTransform.localPosition.y - scaledSize.y / 2f);
                    break;
                case CrosshairType.CIRCLE:

                    //With pivot at center!
                    position = centerPoint + Random.insideUnitCircle * (((float)Screen.height / (float)Screen.currentResolution.height) * lines[0].rectTransform.localPosition.y + scaledSize.x / 2f);
                    break;
            }
        }

        ray = playerCamera.ScreenPointToRay(position);

        return ray;
    }

    public void increaseSpread(float addedSpread)
    {
        spread += addedSpread;
    }
}