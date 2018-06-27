using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Main : MonoBehaviour
{
    public static Main instance = null;
    public Slider loadBar;
    AsyncOperation async;

    public bool shouldLoadSceneOne = false;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);

        
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
            Debug.Log("done loading");
        }
    }

    public void playGame(string name)
    {
        if (async != null)
        {
            async.allowSceneActivation = true;
        }
    }

    public void goMainMenu()
    {
		Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }

    IEnumerator loadLevel(int numScene)
    {
        async = SceneManager.LoadSceneAsync(numScene);
        loadBar.value = async.progress;
        async.allowSceneActivation = false;
        yield return async;
    }

    public void quitGame()
    {
        Application.Quit();
    }

}
