using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class OptionsController : MonoBehaviour {

    public AudioMixer mixer;
    public Text volumeValue;
    public Dropdown resolutionDropdown;
    public Dropdown qualityDropdown;
    public Toggle fullscreenToggle;
    public Slider volumeSlider;

    private Resolution[] resolutions;

    private void Start()
    {
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();
        int resolutionValue = 0;
        for (int i = 0; i < resolutions.Length; ++i)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            if (!options.Contains(option))
            {
                options.Add(option);
            }

            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {
                resolutionValue = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = resolutionValue;
        resolutionDropdown.RefreshShownValue();

        qualityDropdown.value = QualitySettings.GetQualityLevel();

        fullscreenToggle.isOn = Screen.fullScreen;
        float volume = 0f;
        mixer.GetFloat("masterVolume", out volume);

        if (volume >= -80f && volume <= 0f)
        {
            volumeSlider.value = volume;
        }
        volumeValue.text = ((int)(((volumeSlider.value / 80f) + 1f) * 100)).ToString();
    }

    public void setVolume(float value)
    {
        mixer.SetFloat("Master", value);
        volumeValue.text = ((int)(((value / 80f) + 1f) * 100)).ToString();
    }

    public void setQuality(int value)
    {
        QualitySettings.SetQualityLevel(value);
    }

    public void setFullscreen(bool fullscreen)
    {
        Screen.fullScreen = fullscreen;
    }

    public void setResolution(int value)
    {
        Resolution resolution = resolutions[value];
        Screen.SetResolution(resolution.width,resolution.height,Screen.fullScreen,60);
    }

    public void setSubtitles(bool value)
    {
        SubtitleManager.instance.subtitlesActive(value);
    }
}
