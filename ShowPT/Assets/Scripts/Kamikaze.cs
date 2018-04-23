using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kamikaze : Enemy {

    Transform player;
    public float explosionDistance = 20f;
    public int explosionDamage = 3;

	[SerializeField]
	GameObject explosion;

    // Use this for initialization
    void Start() 
	{
		player = GameObject.FindGameObjectWithTag ("Player").GetComponent<Transform>();
	    ctrAudio = GameObject.FindGameObjectWithTag("CtrlAudio").GetComponent<CtrlAudio>();
    }

    // Update is called once per frame
    void Update()
    {
        hasExplode();
    }

    public override void getHit(int damage)
    {
        ctrAudio.playOneSound("Enemies", hitAudio, transform.position, 1.0f, 0.0f, 128);
        hitAudio = ctrAudio.hit;
        enemyHealth -= damage;
        Debug.Log(enemyHealth);
        if (enemyHealth <= 0)
        {
            explode();
        }
    }

    private void hasExplode()
    {
        if (player != null && Vector3.Distance(player.position, transform.position) <= explosionDistance)
        {
            //Explosion animation
            RaycastHit hitInfo;
            if (Physics.Raycast(transform.position, player.position - transform.position, out hitInfo, explosionDistance) && hitInfo.transform.tag == "Player")
            {
                player.GetComponent<PlayerHealth>().ChangeHealth(explosionDamage);
            }
			GameObject.Instantiate (explosion, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }

    public void explode()
    {
        if (player != null && Vector3.Distance(player.position, transform.position) <= explosionDistance)
        {
            //Explosion animation
            RaycastHit hitInfo;
            if (Physics.Raycast(transform.position, player.position - transform.position, out hitInfo, explosionDistance) && hitInfo.transform.tag == "Player")
            {
                player.GetComponent<PlayerHealth>().ChangeHealth(explosionDamage);
            }
            GameObject.Instantiate(explosion, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }

    public override void checkHealth()
    {
        if (enemyHealth <= 0f)
        {
            Destroy(gameObject);
            ScoreController.addDead(ScoreController.Enemy.KAMIKAZE);
        }
    }
}
