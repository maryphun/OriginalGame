using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MainMenuFront : MonoBehaviour
{
    [SerializeField] TMP_Text title;
    [SerializeField] Button start, exit, option;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(TextBlend(title, new Color(title.color.r, title.color.g, title.color.b, 0.0f), Color.white, 0.25f));
    }

    private IEnumerator TextBlend(TMP_Text txt, Color fromColor, Color targetColor, float speed)
    {
        txt.color = fromColor;
        float counter = 0.0f;

        while (counter < 1.0f)
        {
            counter = Mathf.Clamp(counter + Time.deltaTime * speed, 0.0f, 1.0f);
            txt.color = Color.Lerp(fromColor, targetColor, counter);
            yield return null;
        }
    }
}
