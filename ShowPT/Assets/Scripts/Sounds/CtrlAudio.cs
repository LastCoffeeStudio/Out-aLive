using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;


public class CtrlAudio : MonoBehaviour
{
 
    public AudioMixer audioMixer = null;
    Dictionary<string, TrackInfo> tracks = new Dictionary<string, TrackInfo>();
    
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        if (!audioMixer) return;

        AudioMixerGroup[] mixerGroups = audioMixer.FindMatchingGroups(string.Empty);
        
        for(int i = 0; i < mixerGroups.Length; ++i)
        {
            TrackInfo trackInfo = new TrackInfo();
            trackInfo.Name = mixerGroups[i].name;
            trackInfo.Group = mixerGroups[i];
            trackInfo.TrackFader = null;
            tracks[mixerGroups[i].name] = trackInfo;
        }


    }
    
    public float GetTrackVolume(string track)
    {
        TrackInfo trackInfo;
        if (tracks.TryGetValue(track, out trackInfo))
        {
            float volume;
            audioMixer.GetFloat(track, out volume);
            return volume;
        }
        return float.MinValue;
    }

    public AudioMixerGroup GetAudioGroupFromTrackName(string name)
    {
        TrackInfo trackInfo;
        if (tracks.TryGetValue(name, out trackInfo))
        {
            return trackInfo.Group;
        }

        return null;
    }

    public void SetTrackVolume(string track, float volume, float fadeTime = 0.0f)
    {
        if (audioMixer)
        {
            TrackInfo trackInfo;
            if (tracks.TryGetValue(track, out trackInfo))
            {
                if (trackInfo.TrackFader != null)
                {
                    StopCoroutine(trackInfo.TrackFader);
                }

                if (fadeTime == 0.0f)
                {
                    audioMixer.SetFloat(track, volume);
                }
                else
                {
                    trackInfo.TrackFader = SetTrackVolumeInternal(track, volume, fadeTime);
                    StartCoroutine(trackInfo.TrackFader);
                }
            }
        }
    }

    protected IEnumerator SetTrackVolumeInternal(string track, float volume, float fadeTime)
    {
        float startVolume = 0.0f;
        float timer = 0.0f;
        audioMixer.GetFloat(track, out startVolume);

        while (timer < fadeTime)
        {
            timer += Time.unscaledDeltaTime;
            audioMixer.SetFloat(track, Mathf.Lerp(startVolume, volume, timer / fadeTime));
            yield return null;
        }

        audioMixer.SetFloat(track, volume);
    }


}
