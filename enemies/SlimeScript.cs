using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeScript : MonoBehaviour
{
    public Transform planet; // The planet's transform
    public float gravity = -10f; // Gravity strength
    public float moveSpeed = 5f; // Movement speed
    public float rotationSpeed = 1f;
    public float distanceToPLayer;
    public Transform player;

    private Rigidbody rb;
    private Vector3 groundNormal;
    private Animator ani;
    public bool isFlatPlanet = false; // Toggle for different gravity behaviors
    private float timeSinceLastLook;
    public float timeSinceLastRoll;
    public int randNum;
    private bool justRolled;

    public enum State
    {
        Idle,
        Sensing,
        Chasing,
        Patrol
    }

    public State state;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotation;
        rb.useGravity = false; // Initially disable Unity's default gravity
        ani = GetComponent<Animator>();
        state = State.Idle;
    }

    void FixedUpdate()
    {
        ApplyGravity();
    }

    void ApplyGravity()
    {
        if (!isFlatPlanet)
        {
            // Custom planet gravity
            Vector3 gravityDirection = (transform.position - planet.position).normalized;
            rb.AddForce(gravityDirection * gravity);
        }
        else
        {
            // Assume flat ground behavior, enable Unity's default gravity
            rb.useGravity = true;
        }
    }


    //The two AnimatorIsPlaying functions come from edu4hd0 on discussions.unity.com.
    //https://discussions.unity.com/t/how-can-i-check-if-an-animation-is-playing-or-has-finished-using-animator-c/57888/2

    bool AnimatorIsPlaying(){
        return ani.GetCurrentAnimatorStateInfo(0).length >
        ani.GetCurrentAnimatorStateInfo(0).normalizedTime;
    }
    bool AnimatorIsPlaying(string stateName){
        return AnimatorIsPlaying() && ani.GetCurrentAnimatorStateInfo(0).IsName(stateName);
    }

    void roll(){
        if(timeSinceLastRoll > 5.0f){
            randNum = Random.Range(0,10);
            justRolled = true;
            timeSinceLastRoll = 0.0f;
        }
    }

    // Update is called once per frame
    void Update()
    {

        distanceToPLayer = Vector3.Distance(transform.position,player.transform.position);

        if(distanceToPLayer < 20 && state == State.Idle){
            timeSinceLastRoll = 0.0f;
            state = State.Sensing;
        }

        if(distanceToPLayer > 10){
            state = State.Idle;
        }



        if(state == State.Idle){
            timeSinceLastRoll += Time.deltaTime;
            ani.SetTrigger("Idle");

            roll();

            if(randNum < 5){
                //stand still and idle animation
            }
            else{
                timeSinceLastRoll = 0.0f;
                state = State.Patrol;

                if(justRolled == true){
                    int rotation = Random.Range(0,360);
                    transform.Rotate(0, rotation, 0);
                    justRolled = false;
                    
                }
                rb.MovePosition(rb.position + transform.forward * moveSpeed * Time.deltaTime);
                rb.MovePosition(rb.position - transform.up * moveSpeed * 0.1f * Time.deltaTime);
            }

        }

        if(state == State.Patrol){
            ani.SetTrigger("walk");
            if(justRolled == true){
                    int rotation = Random.Range(0,360);
                    transform.Rotate(0, rotation, 0);
                    justRolled = false;
                    
                }
                rb.MovePosition(rb.position + transform.forward * moveSpeed * Time.deltaTime);
                rb.MovePosition(rb.position - transform.up * moveSpeed * 0.1f * Time.deltaTime);
                roll();
                if(randNum < 5){
                    state = State.Idle;
                }
        }

        if(state == State.Sensing){
            timeSinceLastLook += Time.deltaTime;
            ani.SetTrigger("SensedSomething");
            if(timeSinceLastLook > 0.1f){
                transform.LookAt(player);
                timeSinceLastLook = 0.0f;
            }

            if (AnimatorIsPlaying("WalkFWD")){
                state = State.Chasing;
            }

        }

        if(state == State.Chasing){
            timeSinceLastLook += Time.deltaTime;
            if(timeSinceLastLook > 0.1f){
                transform.LookAt(player);
                timeSinceLastLook = 0.0f;
            }
            rb.MovePosition(rb.position + transform.forward * moveSpeed * Time.deltaTime);
            rb.MovePosition(rb.position - transform.up * moveSpeed * 0.1f * Time.deltaTime);
        }
    }
}
