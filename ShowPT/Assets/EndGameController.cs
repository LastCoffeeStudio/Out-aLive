using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGameController : MonoBehaviour
{
    public float timeToEnd;
    public GameObject cinematicObject;

    private GameObject player;
    private ScoreController scoreController;
    private float endTimer;

	void Start ()
    {
        endTimer = 0;
        player = GameObject.FindGameObjectWithTag("Player");
        scoreController = GameObject.FindGameObjectWithTag("SceneUI").GetComponent<ScoreController>();

    }
	
	void Update ()
    {
		if (endTimer >= timeToEnd)
        {
            player.GetComponent<PlayerHealth>().disablePlayer();
            cinematicObject.SetActive(true);
            scoreController.hud.SetActive(false);
        }
        else
        {
            endTimer += Time.deltaTime;
        }
	}
}
