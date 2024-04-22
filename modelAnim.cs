using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class modelAnim : MonoBehaviour
{
    Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim = gameObject.GetComponent<Animator>();
        anim.SetBool("isRunning",false);
        anim.SetBool("isJumping",false); 
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey("w")){  
            anim.SetBool("isRunning",true);
        }
        else if(Input.GetKey("a")){
            anim.SetBool("isRunning",true);
        }
        else if(Input.GetKey("s")){
            anim.SetBool("isRunning",true);
        }
        else if(Input.GetKey("d")){
            anim.SetBool("isRunning",true);
        }
        else if( Input.GetKey("space")){
            anim.SetBool("isJumping",true);
        }
        else{
            anim.SetBool("isRunning",false);
            anim.SetBool("isJumping",false);
        }
    }
}
