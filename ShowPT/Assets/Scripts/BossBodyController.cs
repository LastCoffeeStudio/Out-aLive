using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBodyController : MonoBehaviour {

    public GameObject boss;
    public float forceQuantity;

    private BossController bossCtrl;

    private void Start()
    {
        bossCtrl = boss.GetComponent<BossController>();
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Player")
        {
            Vector3 forceDir = (collider.transform.position - transform.position).normalized;
            forceDir.y = 0.5f;
            collider.GetComponent<Rigidbody>().AddForceAtPosition(Vector3.up, collider.transform.position);
            collider.GetComponent<Rigidbody>().AddForce(forceDir * forceQuantity, ForceMode.Impulse);

            collider.GetComponent<PlayerHealth>().ChangeHealth(-bossCtrl.getBossDamage());
        }

        if (collider.tag == "Enemy" || collider.tag == "Agent" || collider.tag == "Snitch")
        {
            collider.gameObject.GetComponent<Enemy>().getHit(int.MaxValue);
        }
    }
}
