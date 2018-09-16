using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyDecalLed : MonoBehaviour
{

    public float speedDesappear;
    public float secondsLife;
    // Update is called once per frame
    private void Start()
    {
        StartCoroutine(destryMe());
    }

    private IEnumerator destryMe()
    {
        yield return new WaitForSeconds(secondsLife);
        float scale = speedDesappear * Time.deltaTime;

        while (transform.localScale.x > 0.001)
        {
            transform.localScale -= new Vector3(scale, scale, scale);
            yield return null;
        }
        Destroy(gameObject);
    }
}
