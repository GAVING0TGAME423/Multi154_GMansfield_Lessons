using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavPlayerMovement : MonoBehaviour
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

    private void Start()
    {
        rgBody = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
        camera = GetComponentInChildren<Camera>();
        LookTarget = GameObject.Find("Head Aim Target").transform;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            DroppedHive?.Invoke(transform.position + (transform.forward * 10));
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

    private void FixedUpdate()
    {
        Vector3 rot = transform.rotation.eulerAngles;
        rot.y += rotate * rotationSpeed * Time.deltaTime;
        rgBody.MoveRotation(Quaternion.Euler(rot));
        rotate = 0;

        Vector3 move = transform.forward * trans;
        rgBody.velocity = move * speed * Time.deltaTime;
        trans = 0;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.CompareTag("Hazard"))
        {
            animator.SetTrigger("Died");
            StartCoroutine(Zoomout());
        }
        else
        {
            animator.SetTrigger("Twitch Left Ear");
        }
    }
    IEnumerator Zoomout()
    {
        const int ITERATIONS = 24;
        for(int z = 0; z < ITERATIONS; z++)
        {
            camera.transform.Translate(camera.transform.forward * -1 * 15.0f/ITERATIONS);
            yield return new WaitForSeconds(1.0f / ITERATIONS);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Hazard"))
        {
            // LookTarget.position = other.transform.position;
            StartCoroutine(LookandLookaway(LookTarget.position, other.transform.position));
        }
    }
    private IEnumerator LookandLookaway(Vector3 targetpos, Vector3 hazardpos)
    {
        Vector3 targetdir = targetpos - transform.position;
        Vector3 hazarddir = hazardpos - transform.position;

        float angle = Vector2.SignedAngle(new Vector2(targetpos.x, targetpos.z), new Vector2(hazardpos.x, hazardpos.z));

        const int INTERVALS = 20;
        const float Interval = 0.5f / INTERVALS;
        float angleinterval = angle / INTERVALS;
        for(int i=0; i < INTERVALS; i++)
        {
            LookTarget.RotateAround(transform.position, Vector3.up, -angleinterval);
            yield return new WaitForSeconds(Interval);
        }
        for (int i = 0; i < INTERVALS; i++)
        {
            LookTarget.RotateAround(transform.position, Vector3.up, angleinterval);
            yield return new WaitForSeconds(Interval);
        }
    }
}
