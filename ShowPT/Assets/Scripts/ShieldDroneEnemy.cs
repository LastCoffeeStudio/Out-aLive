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
    private Renderer rendShield;
    private float incrementColorGreen;
    private CtrlShieldDrones ctrlShieldDrones;

    // Use this for initialization
    private void Start ()
    {
        active = false;
        ctrAudio = GameObject.FindGameObjectWithTag("CtrlAudio").GetComponent<CtrlAudio>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        smoke1 = transform.GetChild(0).gameObject;
        smoke2 = transform.GetChild(1).gameObject;
        particleSmoke1 = smoke1.GetComponent<ParticleSystem>();
        particleSmoke2 = smoke2.GetComponent<ParticleSystem>();
        ctrlShieldDrones = transform.root.GetComponent<CtrlShieldDrones>();
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

        //SetUp Color
        rendShield = propShield.GetComponent<Renderer>();
        //Total Change Green is 50
        float totalChange = 50f / 255f;
        incrementColorGreen = totalChange / (enemyHealth - 1f);
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
        ctrAudio.playOneSound("Enemies", hitAudio, transform.position, 0.5f, 0.0f, 128);
        enemyHealth -= damage;
        Color color = rendShield.material.GetColor("_TintColor");
        color.g += incrementColorGreen;
        rendShield.material.SetColor("_TintColor",color);
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
            ctrlShieldDrones.dronKilled();
            propShield.SetActive(false);
            ScoreController.addDead(ScoreController.Enemy.DRON);
            smoke1.SetActive(true);
            particleSmoke1.Play();
            smoke2.SetActive(true);
            particleSmoke2.Play();
            AudioSource audio = GetComponentInChildren<AudioSource>();
            audio.volume = 1f;
            audio.maxDistance = 100F;
            GetComponentInChildren<RotateShieldDead>().enabled = true;
            transform.parent.parent.Rotate(Random.Range(30, 60), Random.Range(30, 60), Random.Range(30, 60));
            transform.parent.GetComponent<Animator>().enabled = true;
            transform.parent.GetComponent<Animator>().Play("ShieldDead");
            GetComponent<SphereCollider>().isTrigger = true;
            Rigidbody rbody = gameObject.GetComponent<Rigidbody>();
            if (rbody == null)
            {
                rbody = gameObject.AddComponent<Rigidbody>();
            }
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
