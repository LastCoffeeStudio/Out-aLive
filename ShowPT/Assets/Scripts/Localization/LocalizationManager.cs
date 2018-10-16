using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class LocalizationManager : MonoBehaviour
{

    public static LocalizationManager instance;
    public Text upSubtitle;
    public Text downSubtitle;

    private Dictionary<string, string> localizedText;
    private bool isReady = false;
    private string missingTextString = "Localized text not found";

    private string lenguage;
    // Use this for initialization
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    public void loadLocalizedText(string fileName)
    {
        localizedText = new Dictionary<string, string>();
        string filePath = Path.Combine(Application.streamingAssetsPath, fileName);

        if (File.Exists(filePath))
        {
            string dataAsJson = File.ReadAllText(filePath);
            LocalizationData loadedData = JsonUtility.FromJson<LocalizationData>(dataAsJson);

            for (int i = 0; i < loadedData.items.Length; ++i)
            {
                if (fileName != "AR.json")
                {
                    localizedText.Add(loadedData.items[i].key, loadedData.items[i].value);
                }
                else
                {
                    localizedText.Add(loadedData.items[i].key,reverse(loadedData.items[i].value)); 
                }
            }
            Debug.Log("Data loaded, dictionary contains: " + localizedText.Count + " entries");
            lenguage = fileName;
        }
        else
        {
            Debug.LogError("Cannot find file!");
        }

        isReady = true;
    }

    public string getLocalizedValue(string key)
    {
      
        if (localizedText.ContainsKey(key))
        {
           return localizedText[key];
        }

        return missingTextString;

    }

    public bool getIsReady()
    {
        return isReady;
    }

    public string getLenguage()
    {
        return lenguage;
    }

    public static string reverse(string s)
    {
        char[] charArray = s.ToCharArray();
        Array.Reverse(charArray);
        return new string(charArray);
    }
}
