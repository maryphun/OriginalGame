using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using DG.Tweening;
using MyBox;

public class Cinematic : MonoBehaviour
{
    [SerializeField] private float cinematicTime = 3f;
    [SerializeField] private PlayerController playerCharacter;
    [SerializeField] private MaryTimer timer;
    [SerializeField] private WaveManager waveManager;

    private PlayerInput input;
    private CanvasGroup canvas;

    private void Awake()
    {
        canvas = GetComponent<CanvasGroup>();
        input = new PlayerInput();
        input.Cinematic.Clicked.performed += _ => Wake();
        Cursor.visible = false;
        if (playerCharacter == null)
        {
            playerCharacter = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
            Debug.LogWarning("No default reference to target player.");
        }
    }

    private void OnEnable()
    {
        input.Enable();
    }

    private void OnDisable()
    {
        input.Disable();
    }

    private void Wake()
    {

        input.Cinematic.Disable();
        canvas.DOFade(0.0f, cinematicTime);
        StartCoroutine(CinematicEnd(cinematicTime));
        CameraFollow camScript = Camera.main.GetComponent<CameraFollow>();
        camScript.SetCameraToMiddle();
        //StartCoroutine(camScript.ZoomOut(5f, cinematicTime));

        playerCharacter.PlayAnim("rise", 0, 0f);
    }

    private IEnumerator CinematicEnd(float time)
    {
        yield return new WaitForSeconds(time);
        canvas.DOComplete();
        gameObject.SetActive(false);
        Cursor.visible = true;
        Camera.main.GetComponent<CameraFollow>().ResetCameraDiff();
        timer.InitiateTime(100f);   // 1 minute and 40 seconds
        timer.SetPauseTimer(false);  // set active
        timer.ShowTimer(true, true, 1f);
        waveManager.gameObject.SetActive(true);
    }

}
