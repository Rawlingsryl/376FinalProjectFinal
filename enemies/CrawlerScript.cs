using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrawlerScript : MonoBehaviour
{
    public Transform planet; // The planet's transform
    public float gravity = -10f; // Gravity strength
    public float moveSpeed = 5f; // Movement speed
    public float rotationSpeed = 1f;
    public bool isFlatPlanet = false;

    public float timeSinceTurn = 0.0f;
    private Rigidbody rb;
    private Vector3 groundNormal;
    public float randomRotate;
    // Start is called before the first frame update
    void Start()
    {
        if (!isFlatPlanet)
        {
            // Custom planet gravity
            Vector3 gravityDirection = (transform.position - planet.position).normalized;
            rb.AddForce(gravityDirection * gravity);
        }
        else
        {
            // Assume flat ground behavior, optionally enable Unity's default gravity
            rb.useGravity = true; // This is an example, adjust based on your game's needs
        }
        
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotation;
        rb.useGravity = false;
        randomRotate = Random.Range(-10.0f,10.0f);
    }

    private void FixedUpdate()
    {
        Vector3 gravityDirection = (transform.position - planet.position).normalized;
        rb.AddForce(gravityDirection * gravity);

        // SphereCast parameters
        float sphereRadius = 0.05f; // Adjust based on your player's size
        float castDistance = 10.0f; // Max distance to check for ground
        Vector3 castDirection = -transform.up; // Casting downwards relative to the player
        RaycastHit hitInfo;

        // Perform the SphereCast
        bool hasHit = Physics.SphereCast(transform.position, sphereRadius, castDirection, out hitInfo, castDistance);

        if (hasHit && hitInfo.collider.CompareTag("Planet"))
        {
            groundNormal = hitInfo.normal;

            // Adjust player's height above the ground as needed
            float desiredHeight = 0.1f; // Desired height above terrain
            float currentHeight = hitInfo.distance - sphereRadius; // Adjust for sphere radius
            float heightDifference = desiredHeight - currentHeight;
            Vector3 heightAdjustment = groundNormal * heightDifference * Time.deltaTime * 5;
            rb.MovePosition(rb.position + heightAdjustment);
        }
        else
        {
            groundNormal = -gravityDirection;
        }

        // Rotate the player to align with the ground normal
        Quaternion toRotation = Quaternion.FromToRotation(transform.up, groundNormal) * transform.rotation;
        transform.rotation = Quaternion.Slerp(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
    }

    // Update is called once per frame
    void Update()
    {
        timeSinceTurn += Time.deltaTime;
        rb.MovePosition(rb.position + transform.forward * moveSpeed * Time.deltaTime);
        rb.MovePosition(rb.position - transform.up * moveSpeed * 0.1f * Time.deltaTime);
        
        if(timeSinceTurn > 10){
            transform.RotateAround(transform.position, transform.up, Time.deltaTime * randomRotate);
        }
        if(timeSinceTurn > 12){
            timeSinceTurn = 0;
        }


    }
}
