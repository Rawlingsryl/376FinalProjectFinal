using UnityEngine;

public class CoinAnimation : MonoBehaviour
{
    public float rotationSpeed = 100f;
    public float moveSpeed = 0.5f;
    public float moveAmount = 0.5f;

    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime, Space.World);
        float newY = startPosition.y + moveAmount * Mathf.Sin(Time.time * moveSpeed);
        transform.position = new Vector3(startPosition.x, newY, startPosition.z);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerGravity playerGravity = other.GetComponent<PlayerGravity>();
            if (playerGravity != null)
            {
                playerGravity.AddCoin();
                Destroy(gameObject);  // Destroy the coin
            }
        }
    }
}
