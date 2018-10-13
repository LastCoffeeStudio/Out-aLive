using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenURL : MonoBehaviour
{

    public string url;

    public void OpenUrl()
    {
        Application.OpenURL(url);
    }
}
