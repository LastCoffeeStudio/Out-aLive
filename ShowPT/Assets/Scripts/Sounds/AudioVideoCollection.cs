using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Audio Video Collection")]
public class AudioVideoCollection : ScriptableObject
{

    [SerializeField] public string audioGroup = string.Empty;
    [SerializeField] [Range(0.0f, 1.0f)] public float volume = 1.0f;
    [SerializeField] [Range(0.0f, 1.0f)] public float spatialBlend = 1.0f;
    [SerializeField] [Range(0, 256)] public int priority = 128;
    [SerializeField] public List<ClipBank> audioClipBanks = new List<ClipBank>();
    [SerializeField] public List<SpriteBank> scriptBanks = new List<SpriteBank>();

    public SpriteAudio this[int i]
    {
        get
        {

            if (audioClipBanks != null && audioClipBanks.Count > i && audioClipBanks[i].Clips.Count > 0
                && scriptBanks != null && scriptBanks.Count > i && scriptBanks[i].Sprites.Count > 0)
            {
                List<AudioClip> clipList = audioClipBanks[i].Clips;
                List<Sprite> spriteList = scriptBanks[i].Sprites;
                int pointer =  Random.Range(0, clipList.Count);
                SpriteAudio spriteAudio = new SpriteAudio();
                spriteAudio.audioClip = clipList[pointer];
                spriteAudio.sprite = spriteList[pointer];
                return spriteAudio;
            }

            return null;
        }
    }

    public SpriteAudio audioClip
    {
        get
        {
            if (audioClipBanks != null && audioClipBanks.Count > 0 && audioClipBanks[0].Clips.Count > 0
                && scriptBanks != null && scriptBanks.Count > 0 && scriptBanks[0].Sprites.Count > 0)
            {
                List<AudioClip> clipList = audioClipBanks[0].Clips;
                List<Sprite> spriteList = scriptBanks[0].Sprites;
                int pointer = Random.Range(0, clipList.Count);
                SpriteAudio spriteAudio = new SpriteAudio();
                spriteAudio.audioClip = clipList[pointer];
                spriteAudio.sprite = spriteList[pointer];
                return spriteAudio;
            }
            return null;
        }
    }
}
