using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField]
    private AudioSource audioSource;
    private Coroutine coroutine;
    public AudioClip clip;
    public float volume = 0.5f;


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
        
    }

    public void SetMasterVolume(float value)
    {
        AudioListener.volume = value;
    }

    public void PlaySE(AudioClip clip,float volume)
    {
        audioSource.PlayOneShot(clip,volume);
    }

    public void PlayBGM(AudioClip clip, float volume)
    {
        if (coroutine == null)
        {
            coroutine = StartCoroutine(SoundLoop(clip,volume));
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
