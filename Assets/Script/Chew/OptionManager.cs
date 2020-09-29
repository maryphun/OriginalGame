using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionManager : MonoBehaviour
{
    private List<GameObject> pauseObjects;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        pauseObjects = new List<GameObject>();
        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            pauseObjects.Add(this.gameObject.transform.GetChild(i).gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            PauseGame();
            
        }
        if (Input.GetKeyDown(KeyCode.F1))
        {
            ResumeGame();
        }
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
        foreach (GameObject obj in pauseObjects)
        {
            obj.SetActive(true);
        }
    } 

    public void ResumeGame()
    {
        Time.timeScale = 1;
        foreach (GameObject obj in pauseObjects)
        {
            obj.SetActive(false);
        }
    }
}
