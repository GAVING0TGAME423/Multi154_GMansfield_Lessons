using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BearBrain : MonoBehaviour
{
    private Bot BotScript;
    private Vector3 hivepos;
    private bool hivedropped = false;
    private bool isStopped = false;
    void Start()
    {
        BotScript = GetComponent<Bot>();
        NavPlayerMovement.DroppedHive += HiveReady;
    }
    void HiveReady(Vector3 pos)
    {
        hivepos = pos;
        hivedropped = true;
    }
    
    void Update()
    {
        if (!isStopped)
        {
            if (hivedropped)
            {
                Debug.Log("Chasing Hive");
                BotScript.Seek(hivepos);
            }
            else
            {
                if (BotScript.CanTargetSeeMe())
                {
                    Debug.Log("Evading");
                    BotScript.Evade();
                }
                else if (BotScript.CanSeeTarget())
                {
                    Debug.Log("Pursuing");
                    BotScript.Pursue();
                }
                else
                {
                    Debug.Log("Wandering");
                    BotScript.Wander();
                }
            }
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision .collider .CompareTag("Player"))
        {
            BotScript.Stop();
            isStopped = true;
        }
    }
}
