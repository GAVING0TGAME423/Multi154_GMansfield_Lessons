using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class RemoveNetworkhud : MonoBehaviour
{
    
    void Start()
    {
        GameObject net = GameObject.Find("NetworkManager");
        NetworkManagerHUD netmgrHUD = net.GetComponent<NetworkManagerHUD>();
        netmgrHUD.enabled = false;
    }

   
    void Update()
    {
       
    }
}
