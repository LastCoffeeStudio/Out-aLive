using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AISnitch : MonoBehaviour {

    
    public float speed = 5f;
    public float radioToAttack = 20f;
    public float timeBetweenAttacks = 5f;

    private GameObject player;
    private Transform front;
    private Queue<GameObject> misilesQueue;
    private float timeForNextAttack;
    // Use this for initialization
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        front = transform.GetChild(0);
        timeForNextAttack = timeBetweenAttacks;
        for (int i = 1; i < 5; ++i)
        {
            transform.GetChild(i).GetComponent<AIMisileSnitch>().aiSnitch = this;
            misilesQueue.Enqueue(transform.GetChild(i).transform.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {

        //Move snitch
        float rotationY = orientation2D(transform.position.x, transform.position.z, front.position.x,
            front.position.z, player.transform.position.x, player.transform.position.z);
        transform.Rotate(0f, rotationY * speed * Time.deltaTime, 0f);

        if (Vector3.Distance(player.transform.position, transform.position) < radioToAttack)
        {
            timeForNextAttack -= Time.deltaTime;
            if (timeForNextAttack < 0 && misilesQueue.Count > 0)
            {
                timeForNextAttack = timeBetweenAttacks;
                GameObject misile = misilesQueue.Dequeue();
                misile.GetComponent<AIMisileSnitch>().blast();
            }
        }
    }

    public void misileFree(GameObject misile)
    {
        misilesQueue.Enqueue(misile);
    }

    private int orientation2D(float centerA, float centerB, float pointA, float pointB, float targetA, float targetB)
    {
        double result = ((pointA - centerA) * (targetB - centerB)) - ((pointB - centerB) * (targetA - centerA));

        if (result > 0) return -1;
        else if (result < 0) return 1;
        return 0;
    }
}
