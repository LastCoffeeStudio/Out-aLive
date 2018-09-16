using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyDecalLed : MonoBehaviour
{

    public float speedDesappear;
    public float secondsLife;

    private float size;
    // Update is called once per frame
    private void Start()
    {
        size = transform.localScale.x;
        transform.localScale = Vector3.zero;
        StartCoroutine(destryMe());
    }

    private IEnumerator destryMe()
    {
        float scale = speedDesappear * Time.deltaTime;
        while (transform.localScale.x < size)
        {
            transform.localScale += new Vector3(scale, scale, scale);
            yield return null;
        }
        yield return new WaitForSeconds(secondsLife);


        while (transform.localScale.x > 0.001)
        {
            transform.localScale -= new Vector3(scale, scale, scale);
            yield return null;
        }
        Destroy(gameObject);
    }
}
