using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class EndingCinematicController : MonoBehaviour {

    public GameObject ctrlGame;

    private GameObject cameraPlayer;
    private VideoPlayer videoPlayer;
    private CtrlGameState ctrlGameState;

    private void Start()
    {
        GameObject.FindGameObjectWithTag("CtrlAudio").GetComponent<BGM>().stopTheMusic();
        ctrlGameState = ctrlGame.GetComponent<CtrlGameState>();
        cameraPlayer = GameObject.FindGameObjectWithTag("CameraPlayer");
        videoPlayer = GetComponent<VideoPlayer>();
        videoPlayer.loopPointReached += endGame;
        Cursor.visible = false;
    }

    private void Update() {}

    private void endGame(VideoPlayer vp)
    {
        ctrlGameState.setGameState(CtrlGameState.gameStates.WIN);
        cameraPlayer.SetActive(false);
        ctrlGame.GetComponent<CtrlCamerasWin>().enabled = true;
    }
}
