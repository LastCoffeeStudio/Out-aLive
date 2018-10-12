using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGameController : MonoBehaviour
{
    public float timeToEnd;
    public GameObject cinematicObject;
    public SubtitleAudio subAudio;
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
            SubtitleManager.instance.playSubtitle(20f, subAudio.keysString, SubtitleManager.SubtitleType.DOWNSUBTITLE);
            cinematicObject.SetActive(true);
            scoreController.hud.SetActive(false);
        }
        else
        {
            endTimer += Time.deltaTime;
        }
	}
}
