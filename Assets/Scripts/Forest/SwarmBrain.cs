using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwarmBrain : MonoBehaviour
{
    private bool hasHive = true;
    private Patrolling PatrolScript;
    private Bot BotScript;
    void Start()
    {
        PatrolScript = GetComponent<Patrolling>();
        BotScript = GetComponent<Bot>();
        HivePickUp.HivePickedUp += HiveTaken;
    }

    void HiveTaken()
    {
        hasHive = false;
    }
    
    void Update()
    {
        if (hasHive)
        {
            PatrolScript.PatrolWaypoints();
        }
        else
        {
            BotScript.Pursue();
        }
    }
}
