using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CtrlShieldDrones : MonoBehaviour {

    //Distance betwen Target and player for drones start to search player.
    public float radioHomeEnter;
    //Distance betwen Target and player for drones comeback to target.
    public float radioHomeExit;
    [Header("Shield Behaivor, movment and speed")]
    public float radioSeparation;
    public float KSeparation;
    public float KPlayer;
    public float maxAcceleration;
    public float maxVelocity;
    
    public bool playerInHome = false;
    private List<AIShieldDrone> shieldDrones;
    private GameObject player;
    private GameObject Snitch;
    private GameObject sphere;
    private int dronesAlive;
    private List<GameObject> shieldsObjects;
    // Use this for initialization
    void Start ()
    {
        dronesAlive = 0;
        shieldDrones = new List<AIShieldDrone>();
        shieldsObjects = new List<GameObject>();
        player = GameObject.FindGameObjectWithTag("Player");
        
        for (int i = 0; i < transform.childCount; ++i)
        {
            if (transform.GetChild(i).tag == "Drone")
            {
                transform.GetChild(i).GetComponent<AIShieldDrone>().player = player;
                transform.GetChild(i).GetComponent<AIShieldDrone>().ctrlShieldDrones = this;
                shieldDrones.Add(transform.GetChild(i).GetComponent<AIShieldDrone>());
                shieldsObjects.Add(transform.GetChild(i).gameObject);
                ++dronesAlive;
            }else if (transform.GetChild(i).name == "Snitch")
            {
                Snitch = transform.GetChild(i).gameObject;
                Snitch.transform.parent = null;
            }
            else if (transform.GetChild(i).name == "Sphere")
            {
                sphere = transform.GetChild(i).gameObject;
            }
        }
        for (int i = 0; i < shieldsObjects.Count; i++)
        {
            shieldsObjects[i].transform.GetChild(0).Translate(0f, 0f, 10f);
        }
        transform.Translate(0f, 30f, 0f);
    }

    bool trigerFinish = false;
    private void Update()
    {
        if (trigerFinish == false)
        {
            
            StartCoroutine(closeDoorsSmooth());
            trigerFinish = true;
        }
        if (playerInHome == false)
        {
            if (Vector3.Distance(player.transform.position, transform.position) <= radioHomeEnter)
            {
                //playerInHome = true;
            }

          
        }
        else
        {
            if (Vector3.Distance(player.transform.position, transform.position) >= radioHomeExit)
            {
                //playerInHome = false;
            }
        }
        
    }

    public List<AIShieldDrone> getNeightbours(AIShieldDrone agent, float radious)
    {

        List<AIShieldDrone> neightbours = new List<AIShieldDrone>();
        foreach (var otherAgent in shieldDrones)
        {
            if (otherAgent != null && otherAgent != agent)
            {
                if (Vector3.Distance(agent.shieldTransform.position, otherAgent.shieldTransform.position) <= radious)
                {
                    neightbours.Add(otherAgent);
                }
            }

        }

        return neightbours;
    }

    public void dronKilled()
    {
        --dronesAlive;
        if (dronesAlive < 1)
        {
            sphere.SetActive(false);
            Snitch.GetComponent<SphereCollider>().enabled = true;
        }
    }

    public AnimationCurve curve;
    public float timeClosing = 5f;
    IEnumerator closeDoorsSmooth()
    {
        float time = 0f;
        float speed = -3f;
        float angleSpeed = 30f;
        Vector3 startRotation = transform.position;
        Vector3 endRotation = Snitch.transform.position;
        Vector3 startPosition = shieldsObjects[0].transform.GetChild(0).transform.localPosition;
        Vector3 endPosition = startPosition;
        endPosition.z = 3f;
        while (time <= timeClosing)
        {
            time += Time.deltaTime;
            float t = time / timeClosing;
            transform.position = Vector3.Lerp(startRotation, endRotation, curve.Evaluate(t));
            for (int i = 0; i < shieldsObjects.Count; i++)
            {
                shieldsObjects[i].transform.Rotate(0f, angleSpeed*Time.deltaTime, 0f);
                shieldsObjects[i].transform.GetChild(0).transform.localPosition = Vector3.Lerp(startPosition, endPosition, curve.Evaluate(t));
            }
            yield return null;
        }
    }
}
