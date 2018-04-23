using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;
using UnityEngine.SceneManagement;


public class CtrlAudio : MonoBehaviour
{
 
    public AudioMixer audioMixer = null;
    public int numSounds = 10;

    private Dictionary<string, TrackInfo> tracks = new Dictionary<string, TrackInfo>();
    private List<AudioPoolItem> pool = new List<AudioPoolItem>();
    private Dictionary<ulong, AudioPoolItem> activePool = new Dictionary<ulong, AudioPoolItem>();
    private ulong nextId = 0;
    private Transform listenerPos = null;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        if (!audioMixer) return;

        AudioMixerGroup[] mixerGroups = audioMixer.FindMatchingGroups(string.Empty);
        
        for(int i = 0; i < mixerGroups.Length; ++i)
        {
            TrackInfo trackInfo = new TrackInfo();
            trackInfo.name = mixerGroups[i].name;
            trackInfo.group = mixerGroups[i];
            trackInfo.trackFader = null;
            tracks[mixerGroups[i].name] = trackInfo;
        }

        for (int i = 0; i < numSounds; i++)
        {
            // Create GameObject and assigned AudioSource and Parent
            GameObject go = new GameObject("Pool Item");
            AudioSource audioSource = go.AddComponent<AudioSource>();
            go.transform.parent = transform;

            // Create and configure Pool Item
            AudioPoolItem poolItem = new AudioPoolItem();
            poolItem.gameObj = go;
            poolItem.audioSource = audioSource;
            poolItem.transf = go.transform;
            poolItem.isPlaying = false;
            go.SetActive(false);
            pool.Add(poolItem);

        }

    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        listenerPos = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public float getTrackVolume(string track)
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

    public AudioMixerGroup getAudioGroupFromTrackName(string name)
    {
        TrackInfo trackInfo;
        if (tracks.TryGetValue(name, out trackInfo))
        {
            return trackInfo.group;
        }

        return null;
    }

    public void setTrackVolume(string track, float volume, float fadeTime = 0.0f)
    {
        if (audioMixer)
        {
            TrackInfo trackInfo;
            if (tracks.TryGetValue(track, out trackInfo))
            {
                if (trackInfo.trackFader != null)
                {
                    StopCoroutine(trackInfo.trackFader);
                }

                if (fadeTime == 0.0f)
                {
                    audioMixer.SetFloat(track, volume);
                }
                else
                {
                    trackInfo.trackFader = setTrackVolumeInternal(track, volume, fadeTime);
                    StartCoroutine(trackInfo.trackFader);
                }
            }
        }
    }

    protected IEnumerator setTrackVolumeInternal(string track, float volume, float fadeTime)
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

    protected ulong configPoolObject(int poolIndex, string track, AudioClip clip, Vector3 position, float volume, float spatialBlend, float importance)
    {
        if (poolIndex >= 0 && poolIndex < pool.Count)
        {
            AudioPoolItem poolItem = pool[poolIndex];

            nextId++;
            
            AudioSource source = poolItem.audioSource;
            source.clip = clip;
            source.volume = volume;
            source.spatialBlend = spatialBlend;

            
            source.outputAudioMixerGroup = tracks[track].group;
            
            source.transform.position = position;
            
            poolItem.isPlaying = true;
            poolItem.importance = importance;
            poolItem.ID = nextId;
            poolItem.gameObj.SetActive(true);
            source.Play();
            poolItem.coroutine = stopSoundDelayed(nextId, source.clip.length);
            StartCoroutine(poolItem.coroutine);
            
            activePool[nextId] = poolItem;
            
            return nextId;
        }

        return 0;
    }

    protected IEnumerator stopSoundDelayed(ulong id, float duration)
    {
        yield return new WaitForSeconds(duration);
        AudioPoolItem activeSound;
        
        if (activePool.TryGetValue(id, out activeSound))
        {
            activeSound.audioSource.Stop();
            activeSound.audioSource.clip = null;
            activeSound.gameObj.SetActive(false);
            activePool.Remove(id);
            activeSound.isPlaying = false;
        }
    }

    public ulong playOneSound(string track, AudioClip clip, Vector3 position, float volume, float spatialBlend, int priority = 128)
    {
        if (tracks.ContainsKey(track) && clip != null && volume.Equals(0.0f) == false)
        {

            float importance = (listenerPos.position - position).sqrMagnitude / Mathf.Max(1, priority);

            int leastImportantIndex = -1;
            float leastImportanceValue = float.MaxValue;
            
            for (int i = 0; i < pool.Count; i++)
            {
                AudioPoolItem poolItem = pool[i];

                if (!poolItem.isPlaying)
                {
                    return configPoolObject(i, track, clip, position, volume, spatialBlend, importance);
                }
                else if (poolItem.importance > leastImportanceValue)
                {
                    leastImportanceValue = poolItem.importance;
                    leastImportantIndex = i;
                }
            }

            if (leastImportanceValue > importance)
            {
                return configPoolObject(leastImportantIndex, track, clip, position, volume, spatialBlend, importance);
            }

            //In case we want all sounds here (TO DO) create new AudioPoolItem and call configPoolObject
            return 0;
        }

        return 0;
    }
    public IEnumerator playOneSound(string track, AudioClip clip, Vector3 position, float volume, float spatialBlend, float delay, int priority = 128)
    {
        yield return new WaitForSeconds(delay);
        playOneSound(track, clip, position, volume, spatialBlend, priority);
    }
}
