using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossArmController : MonoBehaviour {

    public GameObject boss;
    public int id;
    public float forceQuantity;
    [HideInInspector]
    public bool vulnerable;

    private BossController bossCtrl;

    private void Start()
    {
        bossCtrl = boss.GetComponent<BossController>();
    }

    public void getHit(int damage)
    {
        if (vulnerable)
        {
            bossCtrl.getHitArm(id, damage);
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Player")
        {
            Vector3 forceDir = (collider.transform.position - transform.position).normalized;
            forceDir.y = 0.5f;
            collider.GetComponent<Rigidbody>().AddForceAtPosition(Vector3.up, collider.transform.position);
            collider.GetComponent<Rigidbody>().AddForce(forceDir * forceQuantity, ForceMode.Impulse);

            collider.GetComponent<PlayerHealth>().ChangeHealth(-bossCtrl.getBossDamage(id));
            Debug.Log(bossCtrl.getBossDamage(id));
            //Enable particle effects here if any
        }

        if (collider.tag == "Enemy" || collider.tag == "Agent" || collider.tag == "Snitch")
        {
            collider.gameObject.GetComponent<Enemy>().getHit(int.MaxValue);
            //Enable particle effects here if any
        }

        if (collider.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            bossCtrl.stopArm(id);
            //Enable particle effects here if any
        }
    }
}
