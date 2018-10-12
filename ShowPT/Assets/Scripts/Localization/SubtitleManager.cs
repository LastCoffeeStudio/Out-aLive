using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SubtitleManager : MonoBehaviour
{
    public static SubtitleManager instance;
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

    public enum SubtitleType
    {
        UPSUBTITLE,
        DOWNSUBTITLE
    }
    private Text UpSubtitle;
    private Text DownSubtitle;
    private Text selectText;

    private void Start()
    {
        if (LocalizationManager.instance.getLenguage() == "AR.json")
        {
            UpSubtitle.font = LocalizationManager.instance.arFont;
            DownSubtitle.font = LocalizationManager.instance.arFont;
        }
    }

    private void OnEnable()
    {
        UpSubtitle = transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Text>();
        DownSubtitle = transform.GetChild(0).GetChild(0).GetChild(1).GetComponent<Text>();
    }

    public void playSubtitle(int timeInSecs, SubtitleItem[] subtitles, SubtitleType subtitleType)
    {
        switch (subtitleType)
        {
            case SubtitleType.UPSUBTITLE:
                selectText = UpSubtitle;
                break;
            case SubtitleType.DOWNSUBTITLE:
                selectText = DownSubtitle;
                break;
        }
        StartCoroutine(playSubtitleAsync(timeInSecs, subtitles));
    }

    IEnumerator playSubtitleAsync(int timeInSecs, SubtitleItem[] subtitles)
    {
        selectText.gameObject.SetActive(true);
        int iter = 0;
        while (iter < subtitles.Length-1)
        {
            selectText.text = LocalizationManager.instance.getLocalizedValue(subtitles[iter].keySubtitle);
            yield return new WaitForSeconds(subtitles[iter+1].timeSec - subtitles[iter].timeSec);
            ++iter;
        }
        selectText.text = LocalizationManager.instance.getLocalizedValue(subtitles[iter].keySubtitle);
        yield return new WaitForSeconds(timeInSecs - subtitles[iter].timeSec);
        selectText.gameObject.SetActive(false);
    }
}
