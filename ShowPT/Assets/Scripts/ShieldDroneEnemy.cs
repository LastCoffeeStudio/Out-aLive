using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldDroneEnemy : Enemy {

    public Transform player;

    private GameObject smoke1;
    private GameObject smoke2;
    private ParticleSystem particleSmoke1;
    private ParticleSystem particleSmoke2;
    private bool exploted = false;
    private GameObject effectHit;
    private GameObject propShield;
    // Use this for initialization
    private void Start ()
    {
        active = false;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        smoke1 = transform.GetChild(0).gameObject;
        smoke2 = transform.GetChild(1).gameObject;
        particleSmoke1 = smoke1.GetComponent<ParticleSystem>();
        particleSmoke2 = smoke2.GetComponent<ParticleSystem>();
        //ctrAudio = GameObject.FindGameObjectWithTag("CtrlAudio").GetComponent<CtrlAudio>()
        for (int i = 0; i < transform.childCount;++i)
        {
            if (transform.GetChild(i).name == "prop_sheld")
            {
                propShield = transform.GetChild(i).gameObject;
            }
            else if (transform.GetChild(i).name == "effectHit")
            {
                effectHit = transform.GetChild(i).gameObject;
                effectHit.SetActive(false);
            }
        }
    }

    // Update is called once per frame
    private void Update()
    {
        if (player != null)
        {
            checkPlayerDistance();
        }
        
    }

    public override float getHit(int damage)
    {
        enemyHealth -= damage;
        checkHealth();
        return enemyHealth;
    }

    private void checkPlayerDistance()
    {
       
    }

    public void explode()
    {
        if (exploted == false)
        {
            exploted = true;
            generateDeathEffect();
        }
    }

    public override void checkHealth()
    {
        if (enemyHealth > 1)
        {
            effectHit.SetActive(true);
            StartCoroutine(playEffectHit());
        }
        else if(enemyHealth <= 0f)
        {
            ScoreController.addDead(ScoreController.Enemy.DRON);
            smoke1.SetActive(true);
            particleSmoke1.Play();
            smoke2.SetActive(true);
            particleSmoke2.Play();
            GetComponentInChildren<RotateShieldDead>().enabled = true;
            transform.parent.parent.Rotate(Random.Range(30, 60), Random.Range(30, 60), Random.Range(30, 60));
            transform.parent.GetComponent<Animator>().enabled = true;
            transform.parent.GetComponent<Animator>().Play("ShieldDead");
            GetComponent<SphereCollider>().isTrigger = true;
            Rigidbody rbody = gameObject.AddComponent<Rigidbody>();
            rbody.useGravity = false;
        }
    }

    IEnumerator playEffectHit()
    {
        yield return new WaitForSeconds(1);
        effectHit.SetActive(false);
        if (enemyHealth < 2)
        {
            propShield.SetActive(false);
        }
    }
}
