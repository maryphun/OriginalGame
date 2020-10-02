using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuBack : MonoBehaviour
{
    [SerializeField] Image backImage, groundImage, knightImage;
    [SerializeField] float lightningTime = 10f;

    [SerializeField] private float timeCnt;

    // Start is called before the first frame update
    void Start()
    {
        timeCnt = lightningTime / 3.5f;
        Random.InitState(3456789);
    }

    // Update is called once per frame
    void Update()
    {
        timeCnt -= Time.deltaTime * (Random.Range(0.25f, 1.5f));
        if (timeCnt <= 0.0f)
        {
            timeCnt = lightningTime;
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
