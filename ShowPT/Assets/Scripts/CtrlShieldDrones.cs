using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CtrlShieldDrones : MonoBehaviour
{

    //Distance betwen Target and player for drones start to search player.
    //public float radioHomeEnter;
    //Distance betwen Target and player for drones comeback to target.
    //public float radioHomeExit;
    [Header("Shield Behaivor, movment and speed")]
    public float radioSeparation;
    public float KSeparation;
    public float KPlayer;
    public float maxAcceleration;
    public float maxVelocity;

    [NonSerialized]
    public bool playerInHome = false;
    private List<AIShieldDrone> shieldDrones;
    private GameObject player;
    private GameObject Snitch;
	private Material SnitchMaterial;
    private GameObject sphere;
    private int dronesAlive;
    private List<GameObject> shieldsObjects;
	private List<Material> shieldsMaterial;

    [Header("Animation Event")]
    public AnimationCurve curve;
    public float timeDown = 5f;
    public float angleSpeed = 50f;
	public float speedFade =1f;
    [NonSerialized]
    public bool rotateShieldsAnimation = true;

    void Start()
    {
        dronesAlive = 0;
        shieldDrones = new List<AIShieldDrone>();
        shieldsObjects = new List<GameObject>();
        player = GameObject.FindGameObjectWithTag("Player");
		SnitchMaterial = Snitch.GetComponent<Renderer>().material;
		Color colorSnitch = SnitchMaterial.color;
		colorSnitch.a = 0f;
		SnitchMaterial.SetColor("_Color", colorSnitch);
        for (int i = 0; i < transform.childCount; ++i)
        {
            if (transform.GetChild(i).tag == "Drone")
            {
                transform.GetChild(i).GetComponent<AIShieldDrone>().player = player;
                transform.GetChild(i).GetComponent<AIShieldDrone>().ctrlShieldDrones = this;
                shieldDrones.Add(transform.GetChild(i).GetComponent<AIShieldDrone>());
                shieldsObjects.Add(transform.GetChild(i).gameObject);
				shieldsMaterial.Add(transform.GetChild(i).GetComponent<Renderer>().material);
				Color color = shieldsMaterial [i].color;
				color.a = 0f;
				shieldsMaterial[i].SetColor("_Color", color);
                ++dronesAlive;
            }
            else if (transform.GetChild(i).name == "Snitch")
            {
                Snitch = transform.GetChild(i).gameObject;
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
        Snitch.transform.parent = null;
        transform.Translate(0f, 30f, 0f);
		sphere.SetActive(false);
    }

    public void startEventDrones()
    {
        StartCoroutine(downShieldDrones());
		StartCoroutine(fadeDrones());
        StartCoroutine(rotateShieldDrones());
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
            if (sphere.active == true)
            {
                sphere.GetComponent<SphereSnitchController>().disolveBarrier(Snitch);
            }
        }
    }

	IEnumerator fadeDrones()
	{
		float alpha = 0f;
		while (alpha < 1f)
		{
			for (int i = 0; i < shieldsObjects.Count; i++)
			{
				alpha += speedFade * Time.deltaTime;
				Color color = shieldsMaterial [i].color;
				color.a = alpha;
				shieldsMaterial[i].SetColor("_Color", color);

			}
			Color colorSnitch = SnitchMaterial.color;
			colorSnitch.a = alpha;
			SnitchMaterial.SetColor("_Color", colorSnitch);
			yield return null;
		}
	}
    
    IEnumerator downShieldDrones()
    {
        float time = 0f;
        Vector3 startDownPosition= transform.position;
        Vector3 endDownPosition = Snitch.transform.position;

        //Drones Join in sphere
        Vector3 startChildsPosition = shieldsObjects[0].transform.GetChild(0).transform.localPosition;
        Vector3 endstChildsPosition = startChildsPosition;
        endstChildsPosition.z = 3f;
        while (time <= timeDown)
        {
            time += Time.deltaTime;
            float t = time / timeDown;
            transform.position = Vector3.Lerp(startDownPosition, endDownPosition, curve.Evaluate(t));
            for (int i = 0; i < shieldsObjects.Count; i++)
            {
                shieldsObjects[i].transform.GetChild(0).transform.localPosition = Vector3.Lerp(startChildsPosition, endstChildsPosition, curve.Evaluate(t));
            }
            yield return null;
        }
        Snitch.transform.parent = transform;
        yield return new WaitForSeconds(1);
        sphere.SetActive(true);
        sphere.GetComponent<SphereSnitchController>().apearBarrier();
    }

    IEnumerator rotateShieldDrones()
    {
        
        while (rotateShieldsAnimation)
        {
            for (int i = 0; i < shieldsObjects.Count; i++)
            {
                shieldsObjects[i].transform.Rotate(0f, angleSpeed * Time.deltaTime, 0f);
            }
            yield return null;
        }
    }

    public void finishRotationAnimation()
    {
        rotateShieldsAnimation = false;
        playerInHome = true;
    }
}
