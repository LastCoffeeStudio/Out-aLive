using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour {

    public int maxDamage;
    public int minDamage;
    public float timeToExplode;
    public float expansionDiameter;
    public float expansionSpeed;

    public float timeToDie;

    private float time;

    private void OnEnable()
    {
        time = 0f;
        transform.parent = null;
    }

    // Update is called once per frame
    void Update ()
    {
        if (time >= timeToExplode)
        {
            transform.localScale = Vector3.Slerp(new Vector3(0f, 0f, 0f), new Vector3(expansionDiameter, expansionDiameter, expansionDiameter), (time - timeToExplode) * expansionSpeed);
        }
        if (time >= timeToDie)
        {
            Destroy(gameObject);
        }
        time += Time.deltaTime;
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Enemy" || col.tag == "Agent" || col.tag == "Snitch")
        {
            float distance = Vector3.Distance(col.gameObject.transform.position, transform.position);
            int finalDamage = calculateDamage(distance);
            col.gameObject.GetComponent<Enemy>().getHit(finalDamage);
        }
        if (col.gameObject.layer == LayerMask.NameToLayer("PhysicsObjects"))
        {
            float distance = Vector3.Distance(col.gameObject.transform.position, transform.position);
            int finalDamage = calculateDamage(distance);
            Vector4 dataToPass = new Vector4(transform.position.x, transform.position.y, transform.position.z, finalDamage);
            col.gameObject.SendMessage("shotBehavior", dataToPass);
        }
    }

    private int calculateDamage(float distance)
    {
        Debug.Log("Data");
        Debug.Log(Mathf.Clamp01(distance / (expansionDiameter / 2f)));
        Debug.Log((int)(minDamage + (maxDamage - minDamage) * Mathf.Clamp01(distance / (expansionDiameter / 2f))));
        Debug.Log("End");
        return (int)(minDamage + (maxDamage - minDamage) * (1 - Mathf.Clamp01(distance / (expansionDiameter / 2f))));
    }
}
