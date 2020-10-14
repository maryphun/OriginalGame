using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class ButtonHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] Color originalColor = Color.white, targetColor = Color.red;

    public void OnPointerEnter(PointerEventData eventData)
    {
        var txt = GetComponentInChildren<TMP_Text>();
        if (txt != null)
        {
            txt.color = targetColor;
        }

        JSAM.AudioManager.PlaySound(JSAM.Sounds.ButtonHover);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        var txt = GetComponentInChildren<TMP_Text>();
        txt.color = originalColor;

        JSAM.AudioManager.PlaySound(JSAM.Sounds.ButtonHover);
    }
}
