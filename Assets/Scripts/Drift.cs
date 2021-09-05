using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drift : MonoBehaviour
{
    public float speed = 10.0f;
    public enum DriftDirection
    {
        Left = -1,
        Right = 1
    }
    public DriftDirection driftdirection = DriftDirection.Left;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        switch (driftdirection)
        {
            case DriftDirection.Left:
             transform.Translate(Vector3.left * Time.deltaTime * speed);
                break;
            case DriftDirection.Right:
            transform.Translate(Vector3.right * Time.deltaTime * speed);
                break;
        }
        

        if(transform.position.x < -80 || transform.position.x > 80)
        {
            Destroy(gameObject);
        }

    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            GameObject child = collision.gameObject;
            child.transform.SetParent(gameObject.transform);
        }
        
    }
    private void OnCollisionExit(Collision collision)
    {
        if(collision.gameObject .CompareTag("Player"))
        {
            GameObject child = collision.gameObject;
            child.transform.SetParent(null);
        }
    }
   
}
