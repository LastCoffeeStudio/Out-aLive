using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Main : MonoBehaviour
{
    AsyncOperation async;

    private void Start()
    {
        StartCoroutine(loadLevel("Game"));
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
            
    IEnumerator loadLevel(string name)
    {
        if (name == "")
            yield break;

        async = SceneManager.LoadSceneAsync(name);
        async.allowSceneActivation = false;
        async.priority = 1;
        yield return async;

    }

    public void quitGame()
    {
        Application.Quit();
    }

}
