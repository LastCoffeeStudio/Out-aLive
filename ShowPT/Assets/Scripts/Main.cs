using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Main : MonoBehaviour
{
    public static Main instance = null;
    public Slider loadBar;
    AsyncOperation async;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }

    // Use this for initialization
    private void Start()
    {
        StartCoroutine(loadLevel(1));
    }

    private void Update()
    {
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
