using UnityEngine;

[CreateAssetMenu(fileName = "New Subtitle Audio")]
public class SubtitleAudio : ScriptableObject
{
    public AudioClip audioClip;
    public SubtitleItem[] keysString;
}
