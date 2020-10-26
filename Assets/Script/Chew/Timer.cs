using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using MyBox;

public class Timer : MonoBehaviour
{
    // not included hours to day
    const int timeUnitConversion = 60; 
    [PositiveValueOnly]
    public float remainingTime;
    public TextMeshProUGUI timeText;
    public Color alertColor;
    [PositiveValueOnly]
    public int alertTime;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        float displayTime = remainingTime + 1;
        int hours = Mathf.FloorToInt(displayTime / timeUnitConversion / timeUnitConversion);
        int minutes = Mathf.FloorToInt((displayTime / timeUnitConversion) % timeUnitConversion);
        int seconds = Mathf.FloorToInt(displayTime % timeUnitConversion);

        timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        if (remainingTime > 0)
        {
            remainingTime -= Time.deltaTime;
        }
        if (remainingTime < 20)
        {
            timeText.color = alertColor;
        }
        
    }
}
