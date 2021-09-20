using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SpawnManager : NetworkBehaviour
{
    public GameObject[] Lilypadobjs;
   public override void OnStartServer()
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
            GameObject templilypad = Instantiate(Lilypad);
            NetworkServer.Spawn(templilypad);
        }
        
    }
}
