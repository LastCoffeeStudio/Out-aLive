using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class IntroCinematicController : MonoBehaviour {

    public Main ctrlMain;

    private VideoPlayer videoPlayer;

    private void Start()
    {
        GameObject.FindGameObjectWithTag("CtrlAudio").GetComponent<BGM>().stopTheMusic();
        videoPlayer = GetComponent<VideoPlayer>();
        videoPlayer.loopPointReached += playGame;
        ctrlMain = GameObject.Find("CtrlMain").GetComponent<Main>();
		Cursor.visible = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("ButtonA"))
        {
            ctrlMain.activateSceneGame();
        }
    }

    private void playGame(VideoPlayer vp)
    {
        ctrlMain.activateSceneGame();
    }
}
