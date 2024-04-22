using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeGravity : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform tp;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerGravity playerGravity = other.GetComponent<PlayerGravity>();
            if (playerGravity != null)
            {
                playerGravity.planet = tp;
            }
        }
    }
}
