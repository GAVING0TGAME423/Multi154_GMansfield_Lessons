using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterShaderSample : MonoBehaviour
{
    
    private Renderer rend;
    void Start()
    {
        rend = GetComponent<Renderer>();
    }

   
    void Update()
    {
        rend.material.SetColor("WHColor", Color.yellow);
    }
}
