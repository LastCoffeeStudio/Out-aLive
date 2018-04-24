using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GlobalTimer : MonoBehaviour
{
    public bool timersActive;
    public float totalTimeLeft;
    public Text[] timers;

    [SerializeField]
    private CtrlGameState ctrlGame;
    private int lastTimeUpdate;
    private int snitchesLeft;
    

    // Use this for initialization
    void Start ()
    {
        ctrlGame = GameObject.FindGameObjectWithTag("CtrlGame").GetComponent<CtrlGameState>();
        timersActive = false;
        lastTimeUpdate = (int)totalTimeLeft;
        snitchesLeft = GameObject.FindGameObjectsWithTag("Snitch").Length;
    }
	
	// Update is called once per frame
	void Update ()
    {
		if (timersActive)
        {
            updateTimers();
        }
	}

    private void updateTimers()
    {
        if (totalTimeLeft > 0f)
        {
            totalTimeLeft -= Time.deltaTime;
            if ((int)totalTimeLeft < lastTimeUpdate)
            {
                lastTimeUpdate = (int)totalTimeLeft;
                for (int i = 0; i < timers.Length; ++i)
                {
                    timers[i].text = ((int)totalTimeLeft).ToString();
                }
            }
        }
        else
        {
            ctrlGame.setGameState(CtrlGameState.gameStates.DEATH);
        }
    }

    public void activateTimers()
    {
        timersActive = true;
        for (int i = 0; i < timers.Length; ++i)
        {
            timers[i].gameObject.SetActive(true);
            timers[i].text = ((int)totalTimeLeft).ToString();
        }
    }

    public void updateSnitches()
    {
        --snitchesLeft;
        Debug.Log(snitchesLeft);
        if (snitchesLeft <= 0)
        {
            ctrlGame.setGameState(CtrlGameState.gameStates.WIN);
        }
    }
}
