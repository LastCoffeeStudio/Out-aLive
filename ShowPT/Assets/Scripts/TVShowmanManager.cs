using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TVShowmanManager : MonoBehaviour {

    public AudioCollection sounds;
    private GameObject[] televisions;
    private CtrlAudio ctrlAudio;

    void Start() {
        televisions = GameObject.FindGameObjectsWithTag("TV");
        ctrlAudio = GameObject.FindGameObjectWithTag("CtrlAudio").GetComponent<CtrlAudio>();
    }

    public void playMessageAllTVs(List<string> tvs, AudiosShowmanType audiosShowmanType)
    {
        AudioClip spriteAudio = sounds[(int)audiosShowmanType];

        for (int i = 0; i < televisions.Length; i++)
        {
            if (tvs.Contains(televisions[i].name)) {
                ctrlAudio.playOneSound(sounds.audioGroup, spriteAudio, televisions[i].gameObject.transform.position, sounds.volume, sounds.spatialBlend, sounds.priority);
                televisions[i].GetComponentInChildren<SpriteRenderer>()
                    .gameObject.GetComponent<Animator>().Play(spriteAudio.name);
            }
        }
    }
}
