using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenuManager : MonoBehaviour
{
    public GameObject pausepanel;

    void Start()
    {
        pausepanel.SetActive(false);
        float value = PlayerPrefs.GetFloat(AudioManager.VOLUME_LEVEL_KEY, AudioManager.DEFAULT_VOLUME);
        pausepanel.GetComponentInChildren<Slider>().value = value;
        DontDestroyOnLoad(gameObject);
    }

    
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            pausepanel.SetActive(true);
            Time.timeScale = 0;
        }
    }
    public void CloseMenu()
    {
        pausepanel.SetActive(false);
        Time.timeScale = 1;
    }
    public void ExitGame()
    {
        Application.Quit();
    }

}
