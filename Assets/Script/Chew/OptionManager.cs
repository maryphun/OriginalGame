using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

enum WindowMode
{
    Fullscreen,
    BorderlessFullscreen,
    Windowed
};

public class OptionManager : MonoBehaviour
{
    private List<GameObject> pauseObjects;
    private Debuginput input;
    public static bool isPaused;
    public Slider masterVolume;
    public Slider soundVolume;
    public Slider musicVolume;
    public TMPro.TMP_Dropdown screenResolution;
    public TMPro.TMP_Dropdown windowMode;
    //public bool applyChange;
    
    int[] horizontalResolution = new int[] { 1280, 1600, 1920 };

    private Resolution resolution;
    private List<Vector2Int> resolutionList;
    private List<string> resolutionString;

    private FullScreenMode currentWindowMode;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {
        
        isPaused = false;
        pauseObjects = new List<GameObject>();
        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            pauseObjects.Add(this.gameObject.transform.GetChild(i).gameObject);
        }

        input = new Debuginput();
        input.Enable();
   
        input.debugging.Option.performed += _ => PauseGame();

        masterVolume.value = PlayerPrefs.GetFloat("MasterVolume", 1);
        SetMasterVolume(masterVolume.value);

        soundVolume.value = PlayerPrefs.GetFloat("SoundVolume", 1);
        SetSoundVolume(soundVolume.value);

        musicVolume.value = PlayerPrefs.GetFloat("MusicVolume", 1);
        SetMusicVolume(musicVolume.value);

        resolutionList = new List<Vector2Int>();
        resolutionString = new List<string>();
        foreach (int reso in horizontalResolution)
        {
            //4:3 aspect ratio
            resolutionList.Add(new Vector2Int(reso, reso / 4 * 3));
            //16:9 aspect ratio
            resolutionList.Add(new Vector2Int(reso, reso / 16 * 9));
        }
        resolutionList = resolutionList.OrderBy(val => val.x * val.y).ToList();
        foreach (Vector2 reso in resolutionList)
        {
            resolutionString.Add(reso.x + "x" + reso.y);
        }
        
        screenResolution.AddOptions(resolutionString);
        //default resolution as 1920x1080
        screenResolution.value = PlayerPrefs.GetInt("CurrentResolution", resolutionList.FindIndex(val => val.x == 1920 && val.y == 1080));
        windowMode.value = PlayerPrefs.GetInt("WindowMode", (int)WindowMode.Fullscreen);
        SetWindowMode(windowMode.value);
        Screen.SetResolution(resolutionList[screenResolution.value].x, resolutionList[screenResolution.value].y, currentWindowMode);
    }

    public void OpenMenu()
    {
        foreach (GameObject obj in pauseObjects)
        {
            obj.SetActive(true);
        }
    }

    public void CloseMenu()
    {
        foreach (GameObject obj in pauseObjects)
        {
            obj.SetActive(false);
        }
    }

    public void PauseGame()
    {
        if (!isPaused)
        {
            Time.timeScale = 0;
            OpenMenu();
        }
        else
        {
            Time.timeScale = 1;
            CloseMenu();
        }
        isPaused = !isPaused;
    } 

    public void SetMasterVolume(float value)
    {
        JSAM.AudioManager.SetMasterVolume(value);
        PlayerPrefs.SetFloat("MasterVolume", value);
    }

    public void SetSoundVolume(float value)
    {
        JSAM.AudioManager.SetSoundVolume(value);
        PlayerPrefs.SetFloat("SoundVolume", value);

    }

    public void SetMusicVolume(float value)
    {
        JSAM.AudioManager.SetMusicVolume(value);
        PlayerPrefs.SetFloat("MusicVolume", value);

    }

    public void SetWindowMode(int value)
    {
        switch(value)
        {
            case (int)WindowMode.Fullscreen:
                currentWindowMode = FullScreenMode.ExclusiveFullScreen;
                break;
            case (int)WindowMode.BorderlessFullscreen:
                currentWindowMode = FullScreenMode.FullScreenWindow;
                break;
            case (int)WindowMode.Windowed:
                currentWindowMode = FullScreenMode.Windowed;
                break;
        }
        SetResolution();

    }

    public void SetResolution()
    {
        //var oldResolution = Screen.currentResolution;
        Screen.SetResolution(resolutionList[screenResolution.value].x, resolutionList[screenResolution.value].y, currentWindowMode);
        StartCoroutine(ChangeResolution());
        //StartCoroutine(ApplyResolution(applyChange,oldResolution));

    }

    private IEnumerator ChangeResolution()
    {
        yield return new WaitUntil(() => Screen.width == resolutionList[screenResolution.value].x && Screen.height == resolutionList[screenResolution.value].y);
    }
     
//    private IEnumerator ApplyResolution(bool doApply,Resolution revertVal)
//    {
//        yield return new WaitForSeconds(15);
//        if (!doApply)
//        {
//            Screen.SetResolution(revertVal.width,revertVal.height, currentWindowMode);
//            StartCoroutine(ChangeResolution());
//        }
//    }
}
