using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ItemCollect : NetworkBehaviour

{
    private Dictionary<Item.VegetableType, int> ItemInventory = new Dictionary<Item.VegetableType, int>();

    public delegate void CollectItem(Item.VegetableType item);
    public static event CollectItem itemcollected;
    Collider itemcollider = null;
    private void Start()
    {
        foreach (Item.VegetableType item in System.Enum.GetValues(typeof(Item.VegetableType)))
        {
            ItemInventory.Add(item, 0);
        }
    }
    private void AddToInventory(Item item)
    {
        ItemInventory[item.typeofveggie]++;
    }

    private void PrintInventory()
    {
        string output = "";

        foreach (KeyValuePair<Item.VegetableType, int> kvp in ItemInventory)
        {
            output += string.Format("{0}: {1}", kvp.Key, kvp.Value);
        }
        Debug.Log(output);
    }
    private void Update()
    {
        if (!isLocalPlayer)
        {
            return;
        }

        if (itemcollider && Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Recieved Item");
            Item item = itemcollider.gameObject.GetComponent<Item>();
            AddToInventory(item);
            itemcollected?.Invoke(item.typeofveggie);
            PrintInventory();
            CmdItemCollected(item.typeofveggie);
        }
        
    }
    [Command]
    void CmdItemCollected(Item.VegetableType itemtype)
    {
        Debug.Log("command " + itemtype);
        RpcItemCollected(itemtype);
    }
    [ClientRpc]
    void RpcItemCollected(Item.VegetableType itemtype)
    {
        itemcollected?.Invoke(itemtype);
    }
       

    private void OnTriggerEnter(Collider other)
    {
        if (!isLocalPlayer)
        {
            return;
        }
        if (other.CompareTag("Item"))
        {
            itemcollider = other;
           
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (!isLocalPlayer)
        {
            return;
        }
        if (other.CompareTag("Item") && Input.GetKeyDown(KeyCode.Space))
        {
            itemcollider = null;
        }
    }
}
