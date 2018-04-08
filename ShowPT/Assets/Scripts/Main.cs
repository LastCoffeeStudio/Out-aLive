using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Main : MonoBehaviour
{
    public static Main instance = null;
    public Slider loadBar;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }

    // Use this for initialization
    private void Start() {}

    private void Update() {}

    public void playGame(string name)
    {
        StartCoroutine(loadLevel(name));
    }

    public void loadSceneByName(string name)
    {
        SceneManager.LoadSceneAsync(name);
    }

    IEnumerator loadLevel(string name)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(name);
        while (!operation.isDone)
        {
            loadBar.value = operation.progress;
            yield return null;
        }
    }

    public void quitGame()
    {
        Application.Quit();
    }

}
