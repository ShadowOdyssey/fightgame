using UnityEngine;

public class ArenaBoundary : MonoBehaviour
{
    public Vector2 minBoundary; // Min X, Z limits
    public Vector2 maxBoundary; // Max X, Z limits

    private Transform playerTransform;

    void Start()
    {
        GameObject player = GameObject.FindWithTag("Gabriella"); // Find player by tag
        if (player != null)
        {
            playerTransform = player.transform; // Assign transform if player is found
        }
        else
        {
            Debug.LogError("Player not found. Please ensure the player GameObject is tagged with 'Gabriella'.");
        }
    }

    void Update()
    {
        if (playerTransform != null) // Only proceed if playerTransform is assigned
        {
            Vector3 clampedPosition = playerTransform.position;
            clampedPosition.x = Mathf.Clamp(clampedPosition.x, minBoundary.x, maxBoundary.x);
            clampedPosition.z = Mathf.Clamp(clampedPosition.z, minBoundary.y, maxBoundary.y);

            playerTransform.position = clampedPosition;
        }
    }
}
