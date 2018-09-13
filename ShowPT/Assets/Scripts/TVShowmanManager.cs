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

	void Update () {
		if(Input.GetKeyDown(KeyCode.O))
        {
            playMessageAllTVs();
        }
	}

    private void playMessageAllTVs()
    {
        AudioClip spriteAudio = sounds[0];
        
        for (int i = 0; i < televisions.Length; i++)
        {
            ctrlAudio.playOneSound(sounds.audioGroup, sounds[0], televisions[i].gameObject.transform.position, sounds.volume, sounds.spatialBlend, sounds.priority);
            televisions[i].GetComponentInChildren<SpriteRenderer>()
                .gameObject.GetComponent<Animator>().Play(sounds[0].name);
        }
    }
}
