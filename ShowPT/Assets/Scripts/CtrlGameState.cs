using UnityEngine;
using UnityEngine.UI;

public class CtrlGameState : MonoBehaviour
{
    public PlayerMovment playerMovement;
    public GameObject score;
    public enum gameStates
    {
        ACTIVE,
        PAUSE,
        DEBUG,
        WIN,
        DEATH,
        EXIT
    }

    public Text winState;
    public Image medal;

    public gameStates gameState;
    
	// Use this for initialization
	void Start ()
	{
        gameState = gameStates.ACTIVE;
    }

    public gameStates getGameState()
    {
        return gameState;
    }

    public void setGameState(gameStates newGameState)
    {
        switch (newGameState)
        {
            case gameStates.ACTIVE:
            break;
            case gameStates.PAUSE:
                break;
            case gameStates.DEBUG:
                break;
            case gameStates.WIN:
                print("YOU WIIIIINNN!!!!");
                winState.text = "YOU WIN!";
                medal.enabled = true;
                gameObject.GetComponent<CtrlCamerasWin>().enabled = true;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                CtrlPause.gamePaused = true;
                score.SetActive(true);
                break;
            case gameStates.DEATH:
                print("YOU DEATH!!!!");
                winState.text = "YOU DIED!";
                medal.enabled = false;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                CtrlPause.gamePaused = true;
                score.SetActive(true);
                break;
            case gameStates.EXIT:
                break;
        }
    }
}
