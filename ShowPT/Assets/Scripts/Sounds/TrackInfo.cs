using System.Collections;
using UnityEngine.Audio;

public class TrackInfo  {

    public string name = string.Empty;
    public AudioMixerGroup group = null;
    public IEnumerator trackFader = null;
}
