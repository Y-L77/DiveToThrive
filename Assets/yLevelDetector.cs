using UnityEngine;
using UnityEngine.UI;

public class yLevelDetector : MonoBehaviour
{
    public Transform player; // Drag the player object here in the Inspector
    public Text depthText;   // Drag the UI Text object here in the Inspector

    void Start()
    {
        if (player == null)
        {
            Debug.LogError("Player Transform not assigned!");
        }

        if (depthText == null)
        {
            Debug.LogError("DepthText UI not assigned!");
        }
    }

    void Update()
    {
        if (player != null && depthText != null)
        {
            int yLevel = Mathf.RoundToInt(Mathf.Abs(player.position.y));
            depthText.text = "Depth: " + yLevel.ToString(); // Convert integer to string
        }
    }
}
