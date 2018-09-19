using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Main : MonoBehaviour
{
    public enum Actions
    {
        PLAY,
        MENU,
        QUIT
    }

    public static Main instance = null;
    AsyncOperation async;

    public bool shouldLoadSceneOne = false;

    [SerializeField]
    private GameObject[] UIToDisable;
    private Dictionary<Actions, UnityAction> actionsDictionary;

    [Header("Sounds")]
    public AudioClip selectSound;
    public AudioClip highlightedSound;
    private CtrlAudio ctrlAudio;
    private BGM bgm;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
        ctrlAudio = GameObject.FindGameObjectWithTag("CtrlAudio").GetComponent<CtrlAudio>();
        actionsDictionary = new Dictionary<Actions, UnityAction>();
        initializeActions();
    }

    private void Start()
    {
        bgm = GameObject.FindGameObjectWithTag("CtrlAudio").GetComponent<BGM>();
        bgm.playMeSomething(0);
    }

    void OnEnable()
    {
        //Debug.Log("OnEnable called");
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // called second
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if(SceneManager.GetActiveScene().buildIndex == 0)
        {
            disableGameObjects();
            shouldLoadSceneOne = true;
            Time.timeScale = 1;
        }
    }

    private void Update()
    {
        if (shouldLoadSceneOne)
        {
            shouldLoadSceneOne = false;
            StartCoroutine(loadLevel(1));
        }

        if (async != null && async.isDone)
        {
            //Debug.Log("done loading");
        }
    }

    IEnumerator loadLevel(int numScene)
    {
        async = SceneManager.LoadSceneAsync(numScene);
        async.allowSceneActivation = false;
        yield return async;
    }

    private void disableGameObjects()
    {
        UIToDisable = GameObject.FindGameObjectsWithTag("MainUI");

        for (int i = 0; i < UIToDisable.Length; ++i)
        {
            UIToDisable[i].SetActive(false);
        }
    }

    private void initializeActions()
    {
        actionsDictionary.Add(Actions.PLAY, playGame);
        actionsDictionary.Add(Actions.MENU, goMainMenu);
        actionsDictionary.Add(Actions.QUIT, quitGame);
    }

    public void playGame()
    {
        if (async != null)
        {
            //async.allowSceneActivation = true;
        }
    }

    public void goMainMenu()
    {
		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }

    public void quitGame()
    {
        Application.Quit();
    }

    public UnityAction getUnityAction(Actions action)
    {
        UnityAction UAction = null;
        actionsDictionary.TryGetValue(action, out UAction);
        return UAction;
    }

    public void activateSceneGame()
    {
        async.allowSceneActivation = true;
    }
}
