using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPoolItem
{
    public GameObject gameObj = null;
    public Transform transf = null;
    public AudioSource audioSource = null;
    public float importance = float.MaxValue;
    public bool isPlaying = false;
    public IEnumerator coroutine = null;
    public ulong ID = 0;
}
