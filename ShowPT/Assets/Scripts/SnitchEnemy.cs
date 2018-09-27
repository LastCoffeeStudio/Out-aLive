using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnitchEnemy : Enemy {
    
    public Transform player;

	[SerializeField]
	GameObject bridge;

    [Header("Sounds")]
    public AudioCollection tvCollection;
    public AudioClip snichSound;
    private ulong idSnichSound;
    public List<string> tvs;
    private TVShowmanManager tVShowmanManager;

    // Use this for initialization
    private void Start()
    {
        tVShowmanManager = GameObject.FindGameObjectWithTag("TVShowmanManager").GetComponent<TVShowmanManager>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        ctrAudio = GameObject.FindGameObjectWithTag("CtrlAudio").GetComponent<CtrlAudio>();
        idSnichSound = ctrAudio.playOneSound("Enemies", snichSound, transform.position, 0.3f, 1f, 90, true, gameObject, 30, 0f,
            AudioRolloffMode.Linear);

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
        checkHealth();
        return enemyHealth;
    }

    private void checkPlayerDistance()
    {

    }

    

    public override void checkHealth()
    {
        if (enemyHealth <= 0f)
        {
            ctrAudio.stopSound(idSnichSound);
            ScoreController.addDead(ScoreController.Enemy.DRON);
			bridge.SetActive (true);
            tVShowmanManager.playMessageAllTVs(tvs, GenericEvent.EventType.BRIDGE);
            //ctrAudio.playOneSound(tvCollection.audioGroup, tvCollection[(int)GenericEvent.EventType.BRIDGE], Vector3.zero, 0.05f, 0f, tvCollection.priority);
            generateDeathEffect ();
        }
    }
    
}
