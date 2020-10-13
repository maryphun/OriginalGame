using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class HitPoint : MonoBehaviour
{
    public int currentHp = 0;
    [SerializeField] private Image heart;
    [SerializeField] private Image[] breakFX;
    [SerializeField] private GameObject gameOverCanvas;

    private List<RectTransform> hearts;
    //private PlayerInput input;
    private int maxHp = 27;

    // Start is called before the first frame update
    private void Awake()
    {
        hearts = new List<RectTransform>();

        //input = new PlayerInput();
        //input.Enable();
        //input.Debug.Z.performed += _ => ChangeHp(-1);
        //input.Debug.X.performed += _ => ChangeHp(0);
        //input.Debug.C.performed += _ => ChangeHp(1);
    }

    private void UpdateHP()
    {
        // Create and remove
        if (hearts.Count < currentHp)
        {
            for (int i = hearts.Count; i < currentHp; i++)
            {
                var newHeart = Instantiate(heart, transform).rectTransform;
                newHeart.DOAnchorPosX((Screen.width / 2f) + (heart.rectTransform.sizeDelta.x / 2f), 0f, false);
                hearts.Add(newHeart);
            }
        }
        else if (hearts.Count > currentHp)
        {
            do
            {
                RectTransform heartToRemove = hearts[(int)hearts.Count / 2];
                BreakEffect(heartToRemove);


                Destroy(heartToRemove.gameObject, 0.2f);
                heartToRemove.DOScale(3f, 0.2f);
                heartToRemove.GetComponent<Image>().DOFade(0.0f, 0.2f);

                hearts.Remove(heartToRemove);


            } while (hearts.Count > currentHp);
        }

        if (hearts.Count == 0) { return; }  // should do nothing when there are no hp at all

        // Calculate the location
        float startDrawPoint = 0.0f;
        startDrawPoint -= heart.rectTransform.sizeDelta.x * hearts.Count / 2f;
        startDrawPoint += heart.rectTransform.sizeDelta.x / 2f;

        // Order it to move
        int tmp = 0;
        foreach (RectTransform hp in hearts)
        {
            hp.DOAnchorPosX(startDrawPoint + (heart.rectTransform.sizeDelta.x * tmp), 1.0f, false);
            tmp++;
            hp.gameObject.SetActive(true);
        }
    }

    /// <summary>
    /// return with new hp value
    /// </summary>
    public int ChangeHp(int value)
    {
        currentHp += value;
        currentHp = Mathf.Clamp(currentHp, 0, maxHp);
        UpdateHP();

        // trigger Game over canvas
        if (currentHp == 0)
        {
            DeathCanvas();
        }

        return currentHp;
    }

    public void BreakEffect(RectTransform heartToBreak)
    {
        for (int i = 0; i < breakFX.Length; i++)
        {
            var fx = Instantiate(breakFX[i], heartToBreak.anchoredPosition, Quaternion.identity);
            fx.transform.SetParent(heartToBreak.transform.parent);
            fx.rectTransform.anchoredPosition = heartToBreak.anchoredPosition;
            Destroy(fx.gameObject, fx.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length);
        }
    }

    public int GetCurrentHP()
    {
        return currentHp;
    }

    public void DeathCanvas()
    {
        var gameover = gameOverCanvas;
        var camScript = Camera.main.GetComponent<CameraFollow>();
        if (gameover == null)
        {
            Debug.Log("gameover reference is null.");
            gameOverCanvas = GameObject.Find("GameOver Canvas");
        }

        if (camScript == null)
        {
            Debug.LogWarning("camera script refernece not found.");
        }

        gameover.SetActive(true);
        gameover.GetComponent<CanvasGroup>().DOFade(1.0f, 4f);

        Camera.main.GetComponent<CameraFollow>().SetCameraToMiddle();
    }
}
