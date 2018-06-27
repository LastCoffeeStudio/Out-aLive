using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
    public int numSnithcObjectives;
    public static int numSnitchKilled;

    public static gameStates gameState;
    public GameObject winTitle;
    public GameObject loseTitle;

    // Use this for initialization
    void Start ()
	{
        gameState = gameStates.ACTIVE;
        numSnitchKilled = 0;
        numSnithcObjectives = 1;
        Time.timeScale = 1;
        //GameObject.FindGameObjectsWithTag("LoseTitle")[0].SetActive(false);
        //GameObject.FindGameObjectsWithTag("WinTitle")[0].SetActive(false);
    }

    public static gameStates getGameState()
    {
        return gameState;
    }

    public void Update()
    {
        if (Input.GetKey(KeyCode.V) || numSnitchKilled >= numSnithcObjectives)
        {
            setGameState(gameStates.WIN);
            Time.timeScale = 0;
        }
        if (Input.GetKey(KeyCode.B))
        {
            setGameState(gameStates.DEATH);
            Time.timeScale = 0;
        }
    }

    public void setGameState(gameStates newGameState)
    {
        gameState = newGameState;
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
                //winState.text = "YOU WIN!";
                //medal.enabled = true;
                //gameObject.GetComponent<CtrlCamerasWin>().enabled = true;
                winTitle.SetActive(true);
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                //score.SetActive(true);
                break;
            case gameStates.DEATH:
                print("YOU DEATH!!!!");
                loseTitle.SetActive(true);
                //winState.text = "YOU DIED!";
                //medal.enabled = false;
				Time.timeScale = 0;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                break;
            case gameStates.EXIT:
                break;
        }
    }
    
    public void goToMainMenu()
    {
        Main.instance.goMainMenu();
    }
}
