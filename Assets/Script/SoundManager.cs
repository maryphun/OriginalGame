using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.IO;

[System.Serializable]
public class SoundVolume
{
    public float bgm = 1.0f;
    public float se = 1.0f;

    public bool mute = false;

    public void Reset()
    {
        bgm = 1.0f;
        se = 1.0f;
        mute = false;
    }
}

public class SoundManager : Singleton<SoundManager>
{

    public class Handle
    {
        public float volume = 0.5f;
        public float fadeSpeed = 1.0f;
        public long frame = 0;

        public void FadeIn()
        {
            Instance().StartCoroutine(fadeIn());
        }

        public void FadeOut()
        {
            Instance().StartCoroutine(fadeOut());
        }

        public void ResetParams()
        {
            volume = 1.0f;
            fadeSpeed = 1.0f;
            frame = 0;
        }

        private IEnumerator fadeIn()
        {
            while (volume < 1.0f)
            {
                volume += fadeSpeed * Time.deltaTime;
                yield return null;
            }
            volume = 1.0f;
        }

        private IEnumerator fadeOut()
        {
            while (volume > 0.0f)
            {
                volume -= fadeSpeed * Time.deltaTime;
                yield return null;
            }
            volume = 0.0f;
        }
    }

    [SerializeField]
    private SoundVolume _volume = new SoundVolume();

    // Get and Set
    public SoundVolume Volume
    {
        get { return _volume; }
        set { _volume = value; }
    }

    [SerializeField]
    private AudioClip[] seClips;
    [SerializeField]
    private AudioClip[] bgmClips;

    private Dictionary<string, int> seIndexes = new Dictionary<string, int>();
    private Dictionary<string, int> bgmIndexes = new Dictionary<string, int>();

    private const int cNumChannel = 16;

    private AudioSource bgmSource;
    private Handle bgmHandle = new Handle();

    private AudioSource[] seSources = new AudioSource[cNumChannel];
    private Dictionary<Handle, AudioSource> seHandles = new Dictionary<Handle, AudioSource>();

    private long frameCounter;

    public void Initialize()
    {
        bgmSource = gameObject.AddComponent<AudioSource>();
        bgmSource.loop = true;

        for (int i = 0; i < seSources.Length; i++)
        {
            seSources[i] = gameObject.AddComponent<AudioSource>();
            seHandles[new Handle()] = seSources[i];
        }

        seClips = Resources.LoadAll<AudioClip>("Audio/SE");
        bgmClips = Resources.LoadAll<AudioClip>("Audio/BGM");

        for (int i = 0; i < seClips.Length; ++i)
        {
            seIndexes[seClips[i].name] = i;
        }

        for (int i = 0; i < bgmClips.Length; ++i)
        {
            bgmIndexes[bgmClips[i].name] = i;
        }
    }

    void Update()
    {
        // Mute audio base on master mute boolen
        bgmSource.mute = _volume.mute;
        foreach (var source in seSources)
        {
            source.mute = _volume.mute;
        }

        // 
        bgmSource.volume = _volume.bgm * bgmHandle.volume;
        foreach (var pair in seHandles)
        {
            pair.Value.volume = _volume.se * pair.Key.volume;
        }

        frameCounter++;
    }

    public int GetSeIndex(string name)
    {
        return seIndexes[name];
    }

    public int GetBgmIndex(string name)
    {
        return bgmIndexes[name];
    }

    public Handle PlayBgm(string name)
    {
        int index = bgmIndexes[name];
        return PlayBgm(index, 1.0f);
    }

    public Handle PlayBgm(string name, float volume)
    {
        int index = bgmIndexes[name];
        return PlayBgm(index, volume);
    }

    public Handle PlayBgm(int index, float volume)
    {
        if(index < 0 || index >= bgmClips.Length)
        {
            return null;
        }

        if (bgmSource.clip == bgmClips[index])
        {
            return bgmHandle;
        }

        bgmSource.Stop();
        bgmSource.clip = bgmClips[index];
        bgmSource.Play();

        bgmHandle.ResetParams();
        bgmHandle.frame = frameCounter;
        bgmHandle.volume = volume;

        return bgmHandle;
    }

    public void StopBgm()
    {
        bgmSource.Stop();
        bgmSource.clip = null;
    }

    public bool IsBgmPlaying { get { return bgmSource.isPlaying; } }

    public bool IsSePlaying(string name)
    {
        return seSources[GetSeIndex(name)].isPlaying;
    }

    public Handle PlaySe(string name)
    {
        return PlaySe(GetSeIndex(name), 1.0f);
    }

    public Handle PlaySe(string name, float volume)
    {
        return PlaySe(GetSeIndex(name), volume);
    }

    public Handle PlaySe(int index, float volume)
    {
        if (0 > index || seClips.Length <= index)
        {
            return null;
        }

        foreach (var k in seHandles)
        {
            AudioSource source = k.Value;
            Handle handle = k.Key;
            if (source.clip == seClips[index] &&
                 handle.frame == frameCounter)
            {
                return handle;
            }
        }

        foreach (var k in seHandles)
        {
            AudioSource source = k.Value;
            Handle handle = k.Key;
            if (!source.isPlaying)
            {
                handle.ResetParams();
                source.clip = seClips[index];
                source.Play();
                handle.frame = frameCounter;
                handle.volume = volume;
                return handle;
            }
        }

        return null;
    }

    public void StopSe()
    {
        foreach (AudioSource source in seSources)
        {
            source.Stop();
            source.clip = null;
            source.volume = 1.0f;
        }
    }

    public void StopSe(string name)
    {
        for (int i = 0; i < seSources.Length; i++)
        {
            if (seSources[i].clip.name == name)
            {
                seSources[i].Stop();
                seSources[i].clip = null;
                seSources[i].volume = 1.0f;
                return;
            }
        }
    }

    public void SetSeLoop(int index, bool flag)
    {
        if (index < 0 || index >= seClips.Length)
        {
            Debug.Log("Index " + index + " is not exist");
            return;
        }

        seSources[index].loop = flag;
    }

    //public void SetSeVolume(string name, float volume)
    //{
    //    foreach (AudioSource source in seSources)
    //    {
    //        if (source.clip.name == name)
    //        {
    //            source.volume = volume;
    //            return;
    //        }
    //    }
    //}

    //public void SetBGMVolume(float volume)
    //{
    //    if (!bgmSource)
    //    {
    //        bgmSource.volume = volume;
    //    }
    //}

    void OnApplicationQuit()
    {
        StopSe();
        StopBgm();
    }
}