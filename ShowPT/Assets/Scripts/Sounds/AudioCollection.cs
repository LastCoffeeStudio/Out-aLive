using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Audio Collection")]
public class AudioCollection : ScriptableObject
{

    [SerializeField] public string audioGroup = string.Empty;
    [SerializeField] [Range(0.0f, 1.0f)] public float volume = 1.0f;
    [SerializeField] [Range(0.0f, 1.0f)] public float spatialBlend = 1.0f;
    [SerializeField] [Range(0, 256)] public int priority = 128;
    [SerializeField] public List<ClipBank> audioClipBanks = new List<ClipBank>();

    public AudioClip this[int i]
    {
        get
        {

            if (audioClipBanks != null && audioClipBanks.Count > i && audioClipBanks[i].Clips.Count > 0)
            {
                List<AudioClip> clipList = audioClipBanks[i].Clips;
                AudioClip clip = clipList[Random.Range(0, clipList.Count)];
                return clip;
            }

            return null;
        }
    }

    public AudioClip audioClip
    {
        get
        {
            if (audioClipBanks != null || audioClipBanks.Count != 0 && audioClipBanks[0].Clips.Count >= 0)
            {
                List<AudioClip> clipList = audioClipBanks[0].Clips;
                AudioClip clip = clipList[Random.Range(0, clipList.Count)];
                return clip;
            }
            return null;
        }
    }
}
