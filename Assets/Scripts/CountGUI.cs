using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CountGUI : MonoBehaviour
{
    private TextMeshProUGUI tmProElem;
    public string itemName;
    int count = 0;

    void Start()
    {
        tmProElem = GetComponent<TextMeshProUGUI>();
    }

   
  
    public void UpdateCount()
    {
        count++;
        tmProElem.text = itemName + ":" + count;
    }
}
