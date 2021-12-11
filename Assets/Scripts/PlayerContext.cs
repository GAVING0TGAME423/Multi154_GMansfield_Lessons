using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public abstract class PlayerState
{
    protected NetworkBehaviour thisObject;
    protected string stateName;
    protected GameObject player;

    protected PlayerState(NetworkBehaviour thisObj)
    {
        thisObject = thisObj;
        player = thisObject.gameObject;
    }
    public abstract void Start();
    public abstract void Update();
    public abstract void FixedUpdate();
    public abstract void OnTriggerExit(Collider other);
    public abstract void OnTriggerEnter(Collider other);

    public abstract void OnCollisionEnter(Collision collision);
}

public class RiverState : PlayerState
{
    private Rigidbody RBPlayer;
    private Vector3 direction = Vector3.zero;
    public float speed = 20.0f;
    public GameObject[] spawnpoints = null;

    public RiverState(NetworkBehaviour thisObj) : base(thisObj)
    {
        stateName = "River Level";
        GameData.gameplaystart = Time.time;
    }
    public override void Start()
    {

        RBPlayer = player.GetComponent<Rigidbody>();
        spawnpoints = GameObject.FindGameObjectsWithTag("Respawn");
    }

    

    public override void Update()
    {
       
        float HorizontalMove = Input.GetAxis("Horizontal");
        float VerticalMove = Input.GetAxis("Vertical");
        direction = new Vector3(HorizontalMove, 0, VerticalMove);
    }
   

    public override void FixedUpdate()
    {
        
        RBPlayer.AddForce(direction * speed, ForceMode.Force);

        if (player.transform.position.z > 40)
        {
            player.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, 40);
        }
        else if (player.transform.position.z < -40)
        {
            player.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, -40);
        }
    }

    private void Respawn()
    {
        int index = 0;
        while (Physics.CheckBox(spawnpoints[index].transform.position, new Vector3(1.5f, 1.5f, 1.5f)))
        {
            index++;
        }
        RBPlayer.MovePosition(spawnpoints[index].transform.position);
        RBPlayer.velocity = Vector3.zero;
    }

    public override void OnCollisionEnter(Collision collision)
    {
        //throw new System.NotImplementedException();
        // does something with compiler, dont touch, leave commented
    }

    public override void OnTriggerEnter(Collider other)
    {
       
        if (other.CompareTag("Exit"))
        {
            NetworkManager networkmanager = 
                GameObject.Find("NetworkManager").GetComponent<NetworkManager>();
            networkmanager.ServerChangeScene("Forest Level");
        }
    }
     public override void OnTriggerExit(Collider other)
    {
       
        if (other.CompareTag("Hazard"))
        {
            Respawn();
        }
    }
}

public class ForestState : PlayerState
{
    public float speed = 40.0f;
    public float rotationSpeed = 30.0f;
    Rigidbody rgBody = null;
    float trans = 0;
    float rotate = 0;
    private Animator animator;
    private Camera camera;
    private Transform LookTarget;
    public delegate void DropHive(Vector3 pos);
    public static event DropHive DroppedHive;

    public ForestState(NetworkBehaviour thisObj) : base(thisObj)
    {
        stateName = "ForestLevel";


    }
    public override void Start()
    {
        player.transform.position = new Vector3(-20, 0.5f, -10);

        Transform rabbit = player.transform.Find("Rabbit");
        rabbit.transform.localEulerAngles = new Vector3(0,180,0);
        rabbit.transform.localScale = Vector3.one;

        rgBody = player.GetComponent<Rigidbody>();
        animator = player.GetComponentInChildren<Animator>();
        camera = player.GetComponentInChildren<Camera>();
        camera.enabled = true;
        LookTarget = GameObject.Find("Head Aim Target").transform;
    }
    public override void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            DroppedHive?.Invoke(player.transform.position + (player.transform.forward * 10));
        }
        // Get the horizontal and vertical axis.
        // By default they are mapped to the arrow keys.
        // The value is in the range -1 to 1
        float translation = Input.GetAxis("Vertical");
        float rotation = Input.GetAxis("Horizontal");

        animator.SetFloat("Speed", translation);

        trans += translation;
        rotate += rotation;
    }

    public override void FixedUpdate()
    {
        Vector3 rot = player.transform.rotation.eulerAngles;
        rot.y += rotate * rotationSpeed * Time.deltaTime;
        rgBody.MoveRotation(Quaternion.Euler(rot));
        rotate = 0;

        Vector3 move = player.transform.forward * trans * speed;
        move.y = rgBody.velocity.y;
        rgBody.velocity = move; // * Time.deltaTime;

        trans = 0;
    }
    public override void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Hazard"))
        {
            animator.SetTrigger("Died");
            thisObject.StartCoroutine(Zoomout());
        }
        else
        {
            animator.SetTrigger("Twitch Left Ear");
        }
    }
    IEnumerator Zoomout()
    {
        const int ITERATIONS = 24;
        for (int z = 0; z < ITERATIONS; z++)
        {
            camera.transform.Translate(camera.transform.forward * -1 * 15.0f / ITERATIONS);
            yield return new WaitForSeconds(1.0f / ITERATIONS);
        }
    }
    public override void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Hazard"))
        {
            // LookTarget.position = other.transform.position;
            thisObject.StartCoroutine(LookandLookaway(LookTarget.position, other.transform.position));
        }
        if (other.CompareTag("Exit"))
        {
            NetworkManager networkmanager =
                GameObject.Find("NetworkManager").GetComponent<NetworkManager>();
            networkmanager.ServerChangeScene("End Scene");
        }
    }

    public override void OnTriggerExit(Collider other)
    {
        //throw new System.NotImplementedException();
    }

    private IEnumerator LookandLookaway(Vector3 targetpos, Vector3 hazardpos)
    {
        Vector3 targetdir = targetpos - player.transform.position;
        Vector3 hazarddir = hazardpos - player.transform.position;

        float angle = Vector2.SignedAngle(new Vector2(targetpos.x, targetpos.z), new Vector2(hazardpos.x, hazardpos.z));

        const int INTERVALS = 20;
        const float Interval = 0.5f / INTERVALS;
        float angleinterval = angle / INTERVALS;
        for (int i = 0; i < INTERVALS; i++)
        {
            LookTarget.RotateAround(player.transform.position, Vector3.up, -angleinterval);
            yield return new WaitForSeconds(Interval);
        }
        for (int i = 0; i < INTERVALS; i++)
        {
            LookTarget.RotateAround(player.transform.position, Vector3.up, angleinterval);
            yield return new WaitForSeconds(Interval);
        }
    }
}

public class PlayerContext : NetworkBehaviour
{
    PlayerState currentstate;
    
    
    void Start()
    {
        if (!isLocalPlayer) return;

        if(SceneManager.GetActiveScene().name == "River Level")
        {
            currentstate = new RiverState(this);
        }
        else if (SceneManager.GetActiveScene().name == "Forest Level")
        {
            currentstate = new ForestState(this);
        }
        else
        {
            this.gameObject.SetActive(false);
        }

        if (currentstate != null)
        {
            currentstate.Start();
        }
        
    }

    
    void Update()
    {
        if (!isLocalPlayer) return;

        currentstate.Update();
    }
     void FixedUpdate()
    {
        if (!isLocalPlayer) return;

        currentstate.FixedUpdate();
    }
     void OnTriggerExit(Collider other)
    {
        if (!isLocalPlayer) return;

        currentstate.OnTriggerExit(other);
    }
    void OnTriggerEnter(Collider other)
    {
        if (!isLocalPlayer) return;

        currentstate.OnTriggerEnter(other);
    }
    void OnCollisionEnter(Collision collision)
    {
        if (!isLocalPlayer) return;

        currentstate.OnCollisionEnter(collision);
    }
}
