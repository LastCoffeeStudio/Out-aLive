using DigitalRuby.LightningBolt;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricShot : MonoBehaviour {

    public float lifeTime;
    public int damage;
    public GameObject lightToEnemy;
    public GameObject decalLed;

    private List<Vector3> positionList;
    private List<GameObject> projectilesList;

    private void Start()
    {
        positionList = new List<Vector3>();
        projectilesList = new List<GameObject>();
        Destroy(gameObject, lifeTime);
    }

    private void Update()
    {
        for(int i = 0; i < positionList.Count; ++i)
        {
            projectilesList[i].GetComponent<LightningBoltScript>().EndPosition = positionList[i];
            projectilesList[i].GetComponent<LightningBoltScript>().Trigger();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy" || other.tag == "Agent" || other.tag == "Snitch")
        {
            projectilesList.Add(Instantiate(lightToEnemy, transform.position, Quaternion.identity, gameObject.transform));
            positionList.Add(other.gameObject.transform.position);

            other.gameObject.GetComponent<Enemy>().getHit(damage);
            other.gameObject.GetComponent<Enemy>().setStatusParalyzed();
        }
        if (other.tag == "BossArm")
        {
            projectilesList.Add(Instantiate(lightToEnemy, transform.position, Quaternion.identity, gameObject.transform));
            positionList.Add(other.gameObject.transform.position);

            other.gameObject.GetComponent<BossArmController>().getHit(damage);
        }
        if (other.gameObject.layer == LayerMask.NameToLayer("PhysicsObjects"))
        {
            Vector4 dataToPass = new Vector4(transform.position.x, transform.position.y, transform.position.z, damage);
            other.gameObject.SendMessage("shotBehavior", dataToPass);
        }
    }
}
