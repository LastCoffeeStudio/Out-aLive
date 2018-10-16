using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SubtitleManager : MonoBehaviour
{
    public static SubtitleManager instance;
    private bool subtitlesAtive = true;
    private Coroutine actualCoroutine;
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

    private Text upSubtitle;
    private Text downSubtitle;
    private Text selectText;
    private int iter;
    

    public void playSubtitle(float timeInSecs, SubtitleItem[] subtitles, SubtitleType subtitleType)
    {
        if (subtitlesAtive)
        {
            int timeSec = ((int) timeInSecs + 1);
            switch (subtitleType)
            {
                case SubtitleType.UPSUBTITLE:
                    selectText = LocalizationManager.instance.upSubtitle;
                    break;
                case SubtitleType.DOWNSUBTITLE:
                    selectText = LocalizationManager.instance.downSubtitle;
                    break;
            }
            actualCoroutine = StartCoroutine(playSubtitleAsync(timeSec, subtitles));
        }
    }

    IEnumerator playSubtitleAsync(int timeInSecs, SubtitleItem[] subtitles)
    {
        selectText.transform.parent.gameObject.SetActive(true);
        iter = 0;
        while (iter < subtitles.Length-1)
        {
            selectText.text = LocalizationManager.instance.getLocalizedValue(subtitles[iter].keySubtitle);
            yield return new WaitForSeconds(subtitles[iter+1].timeSec - subtitles[iter].timeSec);
            ++iter;
        }
        selectText.text = LocalizationManager.instance.getLocalizedValue(subtitles[iter].keySubtitle);
        yield return new WaitForSeconds(timeInSecs - subtitles[iter].timeSec);
        selectText.transform.parent.gameObject.SetActive(false);
    }

    public void stopSubtitle()
    {
        LocalizationManager.instance.upSubtitle.transform.parent.gameObject.SetActive(false);
        LocalizationManager.instance.downSubtitle.transform.parent.gameObject.SetActive(false);
        StopCoroutine(actualCoroutine);
    }

    public void subtitlesActive(bool state)
    {
        subtitlesAtive = state;
    }
}
