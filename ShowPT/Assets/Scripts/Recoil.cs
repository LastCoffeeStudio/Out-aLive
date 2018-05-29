using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recoil : MonoBehaviour {

    [Header("Recoiil")]
    public float minRecoilAmountX;
    public float maxRecoilAmountX;
    public float minRecoilAmountY;
    public float maxRecoilAmountY;
    public float maxRecoilX;
    public float maxRecoilY;
    public float speedRecover;
    public bool recoilActive;
    public GameObject cameraHead;
    public GameObject playerHead;
    [Range(0f,1f)]
    public float percRecoilRecovered;

    [SerializeField]
    private float recoilAmountX = 0f;
    [SerializeField]
    private float recoilAmountY = 0f;
    private float interpolationValue = 0f;
    private float lastRecoilReducedX = 0f;
    private float lastRecoilReducedY = 0f;
	
	// Update is called once per frame
	void Update ()
    {
        if (interpolationValue < 1f)
        {
            interpolationValue += Time.deltaTime * speedRecover;
            Vector3 actualRecoil = new Vector3(recoilAmountX, recoilAmountY, 0f);
            Vector3 recoilAfter = Vector3.Lerp(actualRecoil, new Vector3(0f, 0f, 0f), interpolationValue);
            Vector3 recoilToReduce = recoilAfter - actualRecoil;

            //Update heads
            cameraHead.transform.Rotate(-recoilToReduce.x - lastRecoilReducedX, 0f, 0f);
            playerHead.transform.Rotate(0f, recoilToReduce.y - lastRecoilReducedY, 0f);
            lastRecoilReducedX += -recoilToReduce.x - lastRecoilReducedX;
            lastRecoilReducedY += recoilToReduce.y - lastRecoilReducedY;
        }
    }

    public void addRecoil()
    {
        if (recoilActive)
        {
            //Update recoil amounts
            Vector3 actualRecoil = new Vector3(recoilAmountX, recoilAmountY, 0f);
            Vector3 recoilToReduce = Vector3.Lerp(actualRecoil, new Vector3(0f, 0f, 0f), interpolationValue);
            recoilAmountX = recoilToReduce.x;
            recoilAmountY = recoilToReduce.y;
            lastRecoilReducedX = 0f;
            lastRecoilReducedY = 0f;
            interpolationValue = 0f;

            //Recoil X
            float lastRecoilAmountX = recoilAmountX;
            float recoilToAdd = Random.Range(minRecoilAmountX, maxRecoilAmountX);

            recoilAmountX += recoilToAdd * percRecoilRecovered;
            recoilAmountX = Mathf.Clamp(recoilAmountX, 0f, maxRecoilX);
            recoilToAdd = Mathf.Clamp(recoilToAdd + lastRecoilAmountX, -maxRecoilX, maxRecoilX);
            Vector3 eulerRotation = cameraHead.transform.localRotation.eulerAngles;
            cameraHead.transform.Rotate(-(recoilToAdd - lastRecoilAmountX), 0f, 0f);


            //Recoil Y
            float lastRecoilAmountY = recoilAmountY;
            recoilToAdd = Random.Range(minRecoilAmountY, maxRecoilAmountY);
            recoilToAdd *= Random.Range(0, 2) * 2 - 1;

            recoilAmountY += recoilToAdd * percRecoilRecovered;
            recoilAmountY = Mathf.Clamp(recoilAmountY, -maxRecoilY, maxRecoilY);
            recoilToAdd = Mathf.Clamp(recoilToAdd + lastRecoilAmountY, -maxRecoilY, maxRecoilY);
            eulerRotation = playerHead.transform.localRotation.eulerAngles;
            playerHead.transform.Rotate(0f, recoilToAdd - lastRecoilAmountY, 0f);
        }
    }
}
