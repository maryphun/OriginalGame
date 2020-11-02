using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuBack : MonoBehaviour
{
    [SerializeField] Image backImage, groundImage, knightImage;
    [SerializeField] float lightningTime = 10f;

    [SerializeField] private float timeCnt;

    [HideInInspector] public bool isStarted;

    // Start is called before the first frame update
    void Start()
    {
        timeCnt = lightningTime / 3.5f;
        Random.InitState((int)Time.time);

        // sound manager singleton init
        var bgm = JSAM.AudioManager.PlayMusic(JSAM.Music.MainMenuBGM);
        bgm.volume = 0.85f;

        var rain = JSAM.AudioManager.PlaySoundLoop(JSAM.Sounds.Rain);
        //rain.volume = 0.35f;

        isStarted = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isStarted)
        timeCnt -= Time.deltaTime * (Random.Range(0.25f, 1.5f));
        if (timeCnt <= 0.0f)
        {
            timeCnt = lightningTime;
            JSAM.AudioManager.PlaySound(JSAM.Sounds.Thunder);
            StartCoroutine(Lightning(backImage));
        }
    }

    private IEnumerator Lightning(Image img)
    {
        Color originalColor = img.color;
        img.color = Color.white;
        float counter = 0.0f;

        while (img.color != originalColor)
        {
            counter += Time.deltaTime * 2f;
            img.color = Color.Lerp(Color.white, originalColor, counter);
            yield return null;
        }
    }
}
