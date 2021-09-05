using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject[] Lilypadobjs;
    void Start()
    {
        InvokeRepeating("SpawnLilyPad", 1.0f, 3.0f);
    }

    
    void Update()
    {
        
    }
    void SpawnLilyPad()
    {
        foreach(GameObject Lilypad in Lilypadobjs)
        {
            Instantiate(Lilypad);
        }
        
    }
}
