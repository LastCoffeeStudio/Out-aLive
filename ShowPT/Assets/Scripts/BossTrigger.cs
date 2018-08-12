using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTrigger : MonoBehaviour {

    public GameObject boss;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            boss.GetComponent<Animator>().enabled = true;
            boss.GetComponent<BossController>().enabled = true;
        }
    }
}
