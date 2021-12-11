using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    AudioSource audiosource;
    public const float DEFAULT_VOLUME = 0.5f;
    public const string VOLUME_LEVEL_KEY = "VolumeLevel";
    void Start()
    {
        audiosource = GetComponent<AudioSource>();

        float volume = PlayerPrefs.GetFloat(VOLUME_LEVEL_KEY, DEFAULT_VOLUME);
        audiosource.volume = volume;

        DontDestroyOnLoad(gameObject);
    }

    
    void Update()
    {
        
    }
    public void AdjustVolume(float level)
    {
        audiosource.volume = level;
        PlayerPrefs.SetFloat("VolumeLevel", level);
    }
}
