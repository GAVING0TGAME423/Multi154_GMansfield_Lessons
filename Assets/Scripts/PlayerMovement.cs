using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody RBPlayer;
    private Vector3 direction = Vector3.zero;
    public float speed = 10.0f;
    public GameObject spawnpoint = null;
    private Dictionary<Item.VegetableType, int> ItemInventory = new Dictionary<Item.VegetableType, int>(); 
    void Start()
    {
        RBPlayer = GetComponent<Rigidbody>();

        foreach(Item.VegetableType item in System.Enum.GetValues(typeof(Item.VegetableType)))
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

        foreach(KeyValuePair<Item.VegetableType, int > kvp in ItemInventory)
        {
            output += string.Format("{0}: {1}", kvp.Key, kvp.Value);
        }
        Debug.Log(output);
    }

    private void Update()
    {
        float HorizontalMove = Input.GetAxis("Horizontal");
        float VerticalMove = Input.GetAxis("Vertical");
        direction = new Vector3(HorizontalMove, 0, VerticalMove);
    }


    void FixedUpdate()
    {
       
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
        RBPlayer.MovePosition(spawnpoint.transform.position);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Item"))
        {
            Item item = other.gameObject.GetComponent<Item>();
            AddToInventory(item);
            PrintInventory();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Hazard"))
        {
            Respawn();
        }
    }

}
