using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPools : MonoBehaviour {

    [HideInInspector]
    public List<GameObject> activeProjectiles;

    // Use this for initialization
    void Start ()
    {
        activeProjectiles = new List<GameObject>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        cleanUnusedProjectiles();
	}

    protected void cleanUnusedProjectiles()
    {
        Debug.Log(activeProjectiles.Count);
        for (int i = 0; i < activeProjectiles.Count;)
        {
            if (activeProjectiles[i].GetComponent<Projectile>().toDelete)
            {
                GameObject projectile = activeProjectiles[i];
                activeProjectiles.RemoveAt(i);
                Destroy(projectile);
            }
            else
            {
                ++i;
            }
        }
    }
}
