using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionManager : MonoBehaviour
{
    private List<GameObject> pauseObjects;
    private Debuginput input;
    private bool isPaused;

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
        
    }

    public void PauseGame()
    {
        if (!isPaused)
        {
            Time.timeScale = 0;
            foreach (GameObject obj in pauseObjects)
            {
                obj.SetActive(true);
            }
        }
        else
        {
            Time.timeScale = 1;
            foreach (GameObject obj in pauseObjects)
            {
                obj.SetActive(false);
            }
        }
        isPaused = !isPaused;
    } 

}
