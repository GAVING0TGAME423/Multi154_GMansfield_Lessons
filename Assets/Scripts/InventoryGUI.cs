using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryGUI : MonoBehaviour
{
    public List<GameObject> items;
    void Start()
    {
        ItemCollect.itemcollected += IncrementItem;
    }

    
    void Update()
    {
        
    }
    void IncrementItem(Item.VegetableType itemtype)
    {
        CountGUI cg = items[(int)itemtype].GetComponent<CountGUI>();
        cg.UpdateCount();
    }
}
