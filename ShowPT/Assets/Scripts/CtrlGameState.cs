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
    public GameObject continueButton;
    [SerializeField]
    GameUI gameUI;

    // Use this for initialization
    void Start ()
	{
        gameUI = GameObject.FindGameObjectWithTag("SceneUI").GetComponent<GameUI>();
        gameState = gameStates.ACTIVE;
        numSnitchKilled = 0;
        numSnithcObjectives = 1;
        Time.timeScale = 1;
        loseTitle.SetActive(false);
        winTitle.SetActive(false);
        continueButton.SetActive(false);
    }

    public static gameStates getGameState()
    {
        return gameState;
    }

    public void Update()
    {
		if (Input.GetKeyDown(KeyCode.P) && PlayerMovment.overrideControls == false)
        {
            if (gameState == gameStates.PAUSE)
            {
                setGameState(gameStates.ACTIVE);
            }
            else
            {
                setGameState(gameStates.PAUSE);
            }
        }
    }

    public void setGameState(gameStates newGameState)
    {
        gameState = newGameState;
        switch (newGameState)
        {
            case gameStates.ACTIVE:
                if (gameUI)
                {
                    gameUI.TogglePauseScreen(false);
                }
                break;

            case gameStates.PAUSE:
                if (gameUI)
                {
                    gameUI.TogglePauseScreen(true);
                }
                break;

            case gameStates.DEBUG:
                break;

            case gameStates.WIN:
                winTitle.SetActive(true);
                continueButton.SetActive(true);
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                break;

            case gameStates.DEATH:
                loseTitle.SetActive(true);
                continueButton.SetActive(true);
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
		setGameState(gameStates.ACTIVE);
		PlayerMovment.overrideControls = false;
        Main.instance.goMainMenu();
    }

    public void resumeGame()
    {
        setGameState(gameStates.ACTIVE);
    }
}
