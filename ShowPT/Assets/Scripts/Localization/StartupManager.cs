using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartupManager : MonoBehaviour
{
    public BGM bgm;
    
    // Use this for initialization
    private IEnumerator Start()
    {
        bgm.playMeSomething(0);
        while (!LocalizationManager.instance.getIsReady())
        {
            yield return null;
        }

        SceneManager.LoadScene("Menu Test");
    }

}