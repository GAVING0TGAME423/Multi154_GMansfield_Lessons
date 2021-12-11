using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData : MonoBehaviour
{
    public static float gameplaystart { get; set; }

    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void OnDestroy()
    {
        Debug.Log("Player started at" + gameplaystart + "seconds");
    }
}
