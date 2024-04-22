using UnityEngine;

public class Teleporter : MonoBehaviour
{
    public Transform teleportDestination;
    public Transform newPlanet;
    public GameObject objectToCheck;
    public bool ToFlatPlanet = false;
    private bool collected = false;

    void Update()
    {
        if (objectToCheck == null)
        {
            collected = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && collected == true)  // Make sure the player has the tag "Player"
        {
            other.transform.position = teleportDestination.position;  // Teleport the player
            other.transform.rotation = teleportDestination.rotation;

            // Access the PlayerGravity script attached to the player
            PlayerGravity playerScript = other.GetComponent<PlayerGravity>();
            
            // Update the planet reference
            playerScript.planet = newPlanet;

            // Set flatPlanet to true to disable custom gravity
            playerScript.flatPlanet = ToFlatPlanet;

            Rigidbody playerRigidbody = other.GetComponent<Rigidbody>();
        if (playerRigidbody != null)
        {
            if (ToFlatPlanet)
            {
                // Disable Rigidbody gravity if flatPlanet is true
                playerRigidbody.useGravity = true;
                playerRigidbody.velocity = Vector3.zero; // Optionally reset velocity
                other.transform.rotation = Quaternion.identity; // Reset rotation to zero
            }
            else
            {
                // Enable Rigidbody gravity
                playerRigidbody.useGravity = false;
            }
        }
        }
    }
}