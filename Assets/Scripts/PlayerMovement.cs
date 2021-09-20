using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerMovement : NetworkBehaviour
{
    private Rigidbody RBPlayer;
    private Vector3 direction = Vector3.zero;
    public float speed = 10.0f;
    public GameObject[] spawnpoints = null;
    
    void Start()
    {
        if (!isLocalPlayer)
        {
            return;
        }
        RBPlayer = GetComponent<Rigidbody>();
        spawnpoints = GameObject.FindGameObjectsWithTag("Respawn");

       
    }
    

    private void Update()
    {
        if (!isLocalPlayer)
        {
            return;
        }
        float HorizontalMove = Input.GetAxis("Horizontal");
        float VerticalMove = Input.GetAxis("Vertical");
        direction = new Vector3(HorizontalMove, 0, VerticalMove);
    }


    void FixedUpdate()
    {
        if (!isLocalPlayer)
        {
            return;
        }
        RBPlayer.AddForce(direction * speed, ForceMode.Force);

        if(transform.position.z > 40)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, 40);
        }
        else if (transform.position.z < -40)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, -40);
        }
    }

    private void Respawn()
    {
        int index = 0;
        while(Physics.CheckBox (spawnpoints [index].transform.position, new Vector3 (1.5f,1.5f,1.5f)))
        {
            index++;
        }
        RBPlayer.MovePosition(spawnpoints[index].transform.position);
    }

   
       
    
    
    private void OnTriggerExit(Collider other)
    {
        if (!isLocalPlayer)
        {
            return;
        }
        if (other.CompareTag("Hazard"))
        {
            Respawn();
        }
    }

}
