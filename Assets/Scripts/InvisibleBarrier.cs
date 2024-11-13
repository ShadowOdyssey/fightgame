using UnityEngine;

public class InvisibleBarrier : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Get the player's Rigidbody component
            Rigidbody playerRigidbody = other.GetComponent<Rigidbody>();

            if (playerRigidbody != null)
            {
                // Stop the player's movement
                playerRigidbody.velocity = Vector3.zero;
                playerRigidbody.angularVelocity = Vector3.zero;

                // Optionally, you can add additional logic here to handle the collision
                Debug.Log("Player has hit the invisible barrier.");
            }
        }
    }
}
