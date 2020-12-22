using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class MainMenuFront : MonoBehaviour
{
    [SerializeField] TMP_Text title;
    [SerializeField] Button start, exit, option;
    [SerializeField] Image transition;
    [SerializeField] MainMenuBack backmenu;
    
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

    public void doExitGame()
    {
        if (backmenu.isStarted) return;
        Application.Quit();
    }

    public void doStartGame()
    {
        if (backmenu.isStarted) return;

        JSAM.AudioManager.FadeMusicOut(3f);
        backmenu.isStarted = true;
        transition.DOFade(1.0f, 1.5f);
        StartCoroutine(ChangeScene("SampleScene", 2f));
    }

    public void doOption()
    {
        if (backmenu.isStarted) return;
        OptionManager.Instance().OpenMenu();
    }

    private IEnumerator ChangeScene(string scene, float time)
    {
        yield return new WaitForSeconds(time);
        SceneManager.LoadScene(scene);
    }
}
