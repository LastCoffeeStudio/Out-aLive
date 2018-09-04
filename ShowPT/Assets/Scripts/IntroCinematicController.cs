using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class IntroCinematicController : MonoBehaviour {

    public Main ctrlMain;

    private VideoPlayer videoPlayer;

    private void Start()
    {
        videoPlayer = GetComponent<VideoPlayer>();
        videoPlayer.loopPointReached += playGame;
    }

    private void playGame(VideoPlayer vp)
    {
        ctrlMain.activateSceneGame();
    }
}
