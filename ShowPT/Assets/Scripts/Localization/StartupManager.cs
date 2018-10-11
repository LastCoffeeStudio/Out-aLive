using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartupManager : MonoBehaviour
{
    
    // Use this for initialization
    private IEnumerator Start()
    {
        while (!LocalizationManager.instance.getIsReady())
        {
            yield return null;
        }

        SceneManager.LoadScene("Menu Test");
    }

}