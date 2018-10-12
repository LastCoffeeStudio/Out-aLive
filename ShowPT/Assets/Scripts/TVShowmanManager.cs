using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TVShowmanManager : MonoBehaviour {

    public SubtitleAudioCollection soundsSubtitle;
    private GameObject[] televisions;
    private CtrlAudio ctrlAudio;

    void Start() {
        televisions = GameObject.FindGameObjectsWithTag("TV");
        ctrlAudio = GameObject.FindGameObjectWithTag("CtrlAudio").GetComponent<CtrlAudio>();
    }

    public void playMessageAllTVs(List<string> tvs, GenericEvent.EventType audiosShowmanType)
    {
        SubtitleAudio subtitleAudio = soundsSubtitle[(int) audiosShowmanType];
        AudioClip spriteAudio = subtitleAudio.audioClip;
        for (int i = 0; i < televisions.Length; i++)
        {
            if (tvs.Contains(televisions[i].name) && televisions[i].transform.GetChild(0).gameObject.activeSelf) {
                ctrlAudio.playOneSound(soundsSubtitle.audioGroup, spriteAudio, televisions[i].gameObject.transform.position, soundsSubtitle.volume, soundsSubtitle.spatialBlend, soundsSubtitle.priority);
                televisions[i].GetComponentInChildren<SpriteRenderer>()
                    .gameObject.GetComponent<Animator>().Play(spriteAudio.name);
            }
        }
        SubtitleManager.instance.playSubtitle(spriteAudio.length, subtitleAudio.keysString, SubtitleManager.SubtitleType.UPSUBTITLE);
    }
}
