using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AudioManager : MonoBehaviour
{
    [SerializeField]
    private AudioSource audioSource;
    private Coroutine coroutine;
    public AudioClip clip;

    [Range(0.0f,1.0f)]
    public float masterVolume = 0.5f;

    [Range(0.0f, 1.0f)]
    public float soundEffectVolume = 0.5f;

    [Range(0.0f, 1.0f)]
    public float BGMVolume = 0.5f;


    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

    }
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        //clip = audioSource.clip;
        //PlayBGM(clip);
    }

    // Update is called once per frame
    void Update()
    {
        SetMasterVolume(masterVolume);
    }

    public void SetMasterVolume(float value)
    {
        AudioListener.volume = value;
    }

    public void PlaySE(AudioClip clip)
    {
        audioSource.PlayOneShot(clip, soundEffectVolume);
    }

    public void PlayBGM(AudioClip clip)
    {
        if (coroutine == null)
        {
            coroutine = StartCoroutine(SoundLoop(clip, BGMVolume));
        }
    }

    public void StopBGM()
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
            audioSource.Stop();
            coroutine = null;
        }
    }

    IEnumerator SoundLoop(AudioClip clip, float volume)
    {
        while (true)
        {
            if (audioSource.isPlaying == false)
            {
                audioSource.PlayOneShot(clip,volume);
            }
            yield return null;
        }
    }
}
