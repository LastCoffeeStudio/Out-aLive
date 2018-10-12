using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Subtitle Audio Collection")]
public class SubtitleAudioCollection : ScriptableObject
{

    [SerializeField] public string audioGroup = string.Empty;
    [SerializeField] [Range(0.0f, 1.0f)] public float volume = 1.0f;
    [SerializeField] [Range(0.0f, 1.0f)] public float spatialBlend = 1.0f;
    [SerializeField] [Range(0, 256)] public int priority = 128;
    [SerializeField] public List<SubtitleAudioBank> subtitleAudioBanks = new List<SubtitleAudioBank>();

    public SubtitleAudio this[int i]
    {
        get
        {

            if (subtitleAudioBanks != null && subtitleAudioBanks.Count > i && subtitleAudioBanks[i].Clips.Count > 0)
            {
                List<SubtitleAudio> subtitleAudioList = subtitleAudioBanks[i].Clips;
                SubtitleAudio subtitleAudio = subtitleAudioList[Random.Range(0, subtitleAudioList.Count)];
                return subtitleAudio;
            }

            return null;
        }
    }

    public SubtitleAudio subtitleAudio
    {
        get
        {
            if (subtitleAudioBanks != null || subtitleAudioBanks.Count != 0 && subtitleAudioBanks[0].Clips.Count >= 0)
            {
                List<SubtitleAudio> subtitleAudioList = subtitleAudioBanks[0].Clips;
                SubtitleAudio subtitleAudio = subtitleAudioList[Random.Range(0, subtitleAudioList.Count)];
                return subtitleAudio;
            }
            return null;
        }
    }
}
