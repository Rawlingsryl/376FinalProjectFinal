using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiantSlimeScript : MonoBehaviour
{
    public Transform planet; // The planet's transform
    public float gravity = -10f; // Gravity strength
    public float moveSpeed = 5f; // Movement speed
    public float rotationSpeed = 1f;
    public float distanceToPLayer;
    public Transform player;

    public bool isFlatPlanet = false;

    private Rigidbody rb;
    private Vector3 groundNormal;
    private Animator ani;
    private bool senseLooked;
    public float timeSinceLastLook;
    public enum State{
        Idle,
        Sensing,
        Chasing,
    }

    public State state;

    // Start is called before the first frame update
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


    // Update is called once per frame
    void Update()
    {

        distanceToPLayer = Vector3.Distance(transform.position,player.transform.position);

        if(distanceToPLayer < 20 && state == State.Idle){

            state = State.Sensing;
        }

        if(distanceToPLayer > 20){
            state = State.Idle;
        }



        if(state == State.Idle){
            ani.SetTrigger("Idle");
            timeSinceLastLook = 0;
        }

        if(state == State.Sensing){
            ani.SetTrigger("SensedSomething");
            if(!senseLooked){
                transform.LookAt(player);
                senseLooked = true;
            }
            if (AnimatorIsPlaying("WalkFWD")){
                senseLooked = false;
                timeSinceLastLook = 7.0f;
                state = State.Chasing;
            }

        }

        if(state == State.Chasing){
            timeSinceLastLook += Time.deltaTime;
            if(timeSinceLastLook > 5.0f){
                transform.LookAt(player);
                timeSinceLastLook = 0.0f;
            }
            rb.MovePosition(rb.position + transform.forward * moveSpeed * Time.deltaTime);
            rb.MovePosition(rb.position - transform.up * moveSpeed * 0.1f * Time.deltaTime);
        }
    }
}
