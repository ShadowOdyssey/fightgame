using UnityEngine;
using UnityEngine.UI; // Required for using UI elements
using UnityEngine.EventSystems; // Required for using EventTrigger

public class MarcusMovement : MonoBehaviour
{
    public Button buttonForward;  // Button to move right
    public Button buttonBackward; // Button to move left
    public Button buttonAttack1;  // Button for the first special attack

    public float moveSpeed = 5f;  // Speed of movement
    private float moveDirection = 0f;  // Direction of movement (right or left)

    private Animator marcusAnimator;  // Reference to the Animator component
    public int health = 100; // Health points for Marcus

    public GameObject hitEffectPrefab; // Prefab for Hit effect

    private void Start()
    {
        // Get the Animator component from Marcus
        marcusAnimator = GetComponent<Animator>();

        // Check if the Animator is available
        if (marcusAnimator == null)
        {
            Debug.LogError("Animator component missing from Marcus! Please attach the Animator component.");
        }

        // Add EventTrigger listeners for forward button
        if (buttonForward != null)
        {
            AddEventTrigger(buttonForward.gameObject, EventTriggerType.PointerDown, (eventData) => { MoveRight(); });
            AddEventTrigger(buttonForward.gameObject, EventTriggerType.PointerUp, (eventData) => { StopMoving(); });
        }

        // Add EventTrigger listeners for backward button
        if (buttonBackward != null)
        {
            AddEventTrigger(buttonBackward.gameObject, EventTriggerType.PointerDown, (eventData) => { MoveLeft(); });
            AddEventTrigger(buttonBackward.gameObject, EventTriggerType.PointerUp, (eventData) => { StopMoving(); });
        }

        // Add listener for the first special attack button
        if (buttonAttack1 != null)
        {
            buttonAttack1.onClick.AddListener(Attack1);
        }
        else
        {
            Debug.LogError("ButtonAttack1 is not assigned in the Inspector!");
        }
    }

    // Update is called once per frame
    private void Update()
    {
        // Move Marcus only when the buttons are pressed
        if (moveDirection != 0)
        {
            Vector3 newPosition = transform.position + Vector3.right * moveDirection * moveSpeed * Time.deltaTime;
            transform.position = newPosition; // Move along the X-axis

            // Update animation states based on movement
            marcusAnimator.SetBool("IsWalking", true); // Set IsWalking to true if moving
            marcusAnimator.SetBool("IsBackwards", moveDirection < 0); // Set IsBackwards based on direction
        }
        else
        {
            marcusAnimator.SetBool("IsWalking", false); // Set IsWalking to false if not moving
            marcusAnimator.SetBool("IsBackwards", false); // Ensure IsBackwards is reset
        }
    }

    // Method to start moving right
    private void MoveRight()
    {
        moveDirection = 1f; // Set movement direction to the right (positive X)
    }

    // Method to start moving left
    private void MoveLeft()
    {
        moveDirection = -1f; // Set movement direction to the left (negative X)
    }

    // Method to stop movement
    private void StopMoving()
    {
        moveDirection = 0f; // Stop movement
        marcusAnimator.SetBool("IsWalking", false); // Ensure walking animation is disabled
        marcusAnimator.SetBool("IsBackwards", false); // Reset IsBackwards parameter
    }

    // Special Attack 1 method
    private void Attack1()
    {
        Debug.Log("Attack1 pressed"); // Debug log to confirm the button was pressed
        marcusAnimator.SetTrigger("Attack1"); // Assuming you have an animation trigger for the first attack
    }

    // Method to take damage
    public void TakeDamage(int damage)
    {
        health -= damage; // Subtract damage from health
        Debug.Log($"Marcus took {damage} damage! Health remaining: {health}");

        // Check if health is zero or below
        if (health <= 0)
        {
            Die(); // Call die method if health is zero
        }
    }

    // Method to handle Marcus's death
    private void Die()
    {
        Debug.Log("Marcus has died!");
        // Implement death logic here (e.g., disable movement, play death animation, etc.)
    }

    // Method for detecting collision with the opponent
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Opponent"))
        {
            Debug.Log("Marcus hit by Opponent!");
            
            // Call the TakeDamage method here to handle damage
            TakeDamage(20); // Adjust the damage value as necessary
        }
    }

    // Method to display the hurt effect
    public void DisplayHurtEffect()
    {
        // Instantiate the hurt prefab at Marcus's position
        if (hitEffectPrefab != null)
        {
            Instantiate(hitEffectPrefab, transform.position, Quaternion.identity);
        }

        // Optionally, you can play a hurt animation or any other effect here
    }

    // Helper method to add event triggers to buttons
    private void AddEventTrigger(GameObject target, EventTriggerType eventType, System.Action<BaseEventData> action)
    {
        EventTrigger trigger = target.GetComponent<EventTrigger>();
        if (trigger == null)
        {
            trigger = target.AddComponent<EventTrigger>();
        }

        EventTrigger.Entry entry = new EventTrigger.Entry { eventID = eventType };
        entry.callback.AddListener((eventData) => action(eventData));
        trigger.triggers.Add(entry);
    }
}
