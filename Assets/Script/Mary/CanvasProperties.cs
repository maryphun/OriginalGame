using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CanvasProperties : MonoBehaviour
{
    [Range(0.0f, 1.0f), SerializeField]
    private float aValue = 1;

    [Header("Canvas Component Setup"), SerializeField]
    private CanvasGroup trans;
    public TextMeshProUGUI[] textmesh;
    public Image[] progressor;

    private float originalAlpha;
    private float targetAlpha;
    private float lerpRate;
    private float lerp;
    private IEnumerator coroutine;

    void Start()
    {
        trans.alpha = aValue;
    }

    private void Update()
    {
        //if the alpha lerp is not done yet
        if (trans.alpha != targetAlpha)
        {
            lerp += lerpRate;
            trans.alpha = Mathf.Lerp(originalAlpha, targetAlpha, lerp);
        }
    }

    public void SetAlpha(float alpha, float incrementSpeed)
    {
        originalAlpha = trans.alpha;
        targetAlpha = alpha;
        lerpRate = incrementSpeed;
        lerp = 0.0f;
        enabled = true;
    }

    public void SetAlpha(float alpha, float incrementSpeed, float stayTime)
    {
        originalAlpha = trans.alpha;
        targetAlpha = alpha;
        lerpRate = incrementSpeed;
        lerp = 0.0f;
        coroutine = DelayFadeOut(stayTime);
        StopAllCoroutines();
        StartCoroutine(coroutine);
        enabled = true;
    }

    public float GetAlpha()
    {
        return trans.alpha;
    }

    public void SetTextMesh(int index ,string text)
    {
        textmesh[index].SetText(text);
    }
    
    public void SetTextColor(int index, Color newcolor)
    {
        textmesh[index].color = newcolor;
    }

    public void SetProgressor(int index, float targetFillAmount)
    {
        progressor[index].fillAmount = targetFillAmount;
    }

    public float GetProgressor(int index)
    {
        return progressor[index].fillAmount;
    }

    public void SetProgressorColor(int index, Gradient gradientcolor, float point)
    {
        progressor[index].color = gradientcolor.Evaluate(point);
    }

    public void SetProgressorColor(int index, Color newcolor)
    {
        progressor[index].color = newcolor;
    }

    public void SetScale(Vector3 scale)
    {
        var target = transform.GetChild(0).GetComponent<RectTransform>();
        if (target != null)
        {
            target.localScale = scale;
        }
        else
        {
            Debug.LogWarning("Script trying to access something null, this shouldn't happen!");
        }
    }

    public void SetScaleX(float newX)
    {
        var target = transform.GetChild(0).GetComponent<RectTransform>();
        if (target != null)
        {
            target.localScale = new Vector3(newX, target.localScale.y, target.localScale.z);
        }
        else
        {
            Debug.LogWarning("Script trying to access something null, this shouldn't happen!");
        }
    }

    private IEnumerator DelayFadeOut(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        //reset
        originalAlpha = trans.alpha;
        targetAlpha = 0.0f;
        lerp = 0.0f;
    }
}
