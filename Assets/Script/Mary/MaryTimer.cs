using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using DG.Tweening;

public class MaryTimer : MonoBehaviour
{
    [SerializeField] private bool hideByDefault = true;
    [SerializeField] private TMP_Text timerText;
    [SerializeField] private UnityEvent timeEndCallback;
    [SerializeField] private float maxTime = 3600;
    [SerializeField] private float effectTime = 10f;    // if the timer value is lower than this, play special effect

    private float time;
    private bool isPaused;
    private string lastText;

    private void Awake()
    {
        if (hideByDefault)
        {
            ShowTimer(false, false, 0.0f);
        }
        else
        {
            time = 75;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!isPaused)
        {
            time = Mathf.Clamp(time - Time.deltaTime, 0f, maxTime);
        }

        string minute, second;
        minute = ((int)(time / 60)).ToString();
        second = ((int)(time % 60)).ToString("D2");

        timerText.text = minute + ":" + second;

        if (lastText != timerText.text)
        {
            if (time < effectTime)
            {
                Effect(0.5f);
            }
        }

        if (time <= 0f && !isPaused)
        {
            timeEndCallback.Invoke();
            SetPauseTimer(true);
        }
        lastText = timerText.text;
    }

    public void InitiateTime(float timeInSecond)
    {
        time = timeInSecond;
    }

    /// <summary>
    /// Can be negative too, return the result
    /// </summary>
    public float IncreaseTime(float value)
    {
        time += value;
        return time;
    }

    public void ShowTimer(bool boolean, bool timeGoesOn, float fadeTime)
    {
        if (boolean)
        {
            timerText.DOFade(1.0f, fadeTime);
        }
        else
        {
            timerText.DOFade(0.0f, fadeTime);
            if (timeGoesOn)
            {
                SetPauseTimer(true);
            }
        }
    }

    public void SetPauseTimer(bool pause)
    {
        isPaused = pause;
    }

    private void Effect(float fadeTime)
    {
        TMP_Text effectText = Instantiate(timerText, transform).GetComponent<TMP_Text>();
        effectText.GetComponent<RectTransform>().DOScale(5f, fadeTime);
        effectText.DOFade(0.0f, fadeTime);
        Destroy(effectText, fadeTime);
    }
}
