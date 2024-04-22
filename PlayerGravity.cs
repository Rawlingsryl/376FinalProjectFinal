using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;

public class PlayerGravity : MonoBehaviour
{
    public int health = 4;
    public TextMeshProUGUI text;
    private bool isImmune = false;
    private float immuneTime = 1.0f;
     private Renderer childRenderer;
    private int coins = 0;
    public int coinsToWin = 2;

    public Transform planet; // The planet's transform
    public float gravity = -10f; // Gravity strength
    public float moveSpeed = 5f; // Movement speed
    public float jumpForce = 10f; // Jumping force
    public float rotationSpeed = 10f;

    public bool flatPlanet = false; // Toggle for planet gravity vs. flat surface

    private Rigidbody rb;
    private Vector3 groundNormal;
    private bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotation;
        rb.useGravity = false; // Initially disable Unity's default gravity
        childRenderer = GetComponentInChildren<Renderer>();
    }

    void Update()
    {
        if (health <= 0)
        {
            Debug.Log("Player is dead!");
            // Add death logic here, e.g., disable controls, show death screen, etc.
        }

        if (coins == coinsToWin) 
        {
            text.enabled = true;
        }
    }

    void FixedUpdate()
    {
        if (!flatPlanet)
        {
            // Custom planet gravity
            Vector3 gravityDirection = (transform.position - planet.position).normalized;
            rb.AddForce(gravityDirection * gravity);
        }
        else
        {
            // Standard gravity (e.g., walking on a flat surface)
            rb.useGravity = true; // Enable Unity's default gravity
        }

        CheckGround();
        MovePlayer();
        CheckJump();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy") && !isImmune)
        {
            health--;  // Reduce health by 
            StartCoroutine(ActivateImmunity());

            if (health == 0)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }
    }

    IEnumerator ActivateImmunity()
    {
        isImmune = true;
        for (int i = 0; i < 10; i++) // Blink 5 times over the immunity period.
        {
            childRenderer.enabled = !childRenderer.enabled;  // Toggle childRenderer to blink
            yield return new WaitForSeconds(immuneTime / 10);  // Wait for a tenth of the immune time before toggling visibility again
        }
        childRenderer.enabled = true;  // Ensure childRenderer is enabled after blinking
        isImmune = false;
    }

    private void MovePlayer()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Transform cameraTransform = Camera.main.transform;
        Vector3 forward = Vector3.Cross(cameraTransform.right, groundNormal).normalized;
        Vector3 right = Vector3.Cross(groundNormal, cameraTransform.forward).normalized;
        Vector3 moveDir = (forward * v + right * h).normalized;

        rb.MovePosition(rb.position + moveDir * moveSpeed * Time.fixedDeltaTime);

        if (h != 0)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDir, transform.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }
    }

    private void CheckJump()
    {
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        }
    }

    private void CheckGround()
    {
        // SphereCast parameters
        float sphereRadius = 0.5f; // Adjust based on your player's size
        float castDistance = 10.0f; // Max distance to check for ground
        Vector3 castDirection = -transform.up; // Casting downwards relative to the player
        RaycastHit hitInfo;

        // Perform the SphereCast
        bool hasHit = Physics.SphereCast(transform.position, sphereRadius, castDirection, out hitInfo, castDistance);

        if (hasHit && hitInfo.collider.CompareTag("Planet"))
        {
            isGrounded = true;
            groundNormal = hitInfo.normal;

            // Adjust player's height above the ground as needed
            float desiredHeight = 0.05f; // Desired height above terrain
            float currentHeight = hitInfo.distance - sphereRadius; // Adjust for sphere radius
            float heightDifference = desiredHeight - currentHeight;
            Vector3 heightAdjustment = groundNormal * heightDifference * Time.deltaTime * 5;
            rb.MovePosition(rb.position + heightAdjustment);

            Quaternion toRotation = Quaternion.FromToRotation(transform.up, groundNormal) * transform.rotation;
            transform.rotation = Quaternion.Slerp(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
        }
        else
        {
            isGrounded = false;
        }
    }

    public void SetGravitySource(Transform newPlanet)
    {
        planet = newPlanet;
        rb.velocity = Vector3.zero; // Optionally reset the player's velocity to ensure a smooth transition
    }

    public void AddCoin()
    {
        coins++;
        Debug.Log("Coins collected: " + coins);
        CheckWinCondition();
    }

    private void CheckWinCondition()
    {
        if (coins == coinsToWin)
        {
            Debug.Log("You win!");
            // Implement additional win game logic here
        }
    }
}
