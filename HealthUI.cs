using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthDisplay : MonoBehaviour
{
    public TextMeshProUGUI healthText;  // Assign this in the inspector
    public PlayerGravity player;  // Reference to your player script

    void Update()
    {
        if (healthText != null && player != null)
        {
            healthText.text = "Health: " + player.health.ToString();
        }
    }
}
