using UnityEngine;
using UnityEngine.UI; // Required for using UI elements
using UnityEngine.EventSystems; // Required for using EventTrigger

public class OrionMovement : MonoBehaviour
{
    public Button buttonForward;  // Button to move right
    public Button buttonBackward; // Button to move left

    public float moveSpeed = 5f;  // Speed of movement
    private float moveDirection = 0f;  // Direction of movement (right or left)

    private Animator orionAnimator;  // Reference to the Animator component

    private void Start()
    {
        // Get the Animator component from Orion
        orionAnimator = GetComponent<Animator>();

        // Check if the Animator is available
        if (orionAnimator == null)
        {
            Debug.LogError("Animator component missing from Orion! Please attach the Animator component.");
        }

        // Add EventTrigger listeners for forward button
        if (buttonForward != null)
        {
            AddEventTrigger(buttonForward.gameObject, EventTriggerType.PointerDown, (eventData) => { MoveRight(); });
            AddEventTrigger(buttonForward.gameObject, EventTriggerType.PointerUp, (eventData) => { StopMoving(); });
        }
        else
        {
            Debug.LogError("ButtonForward is not assigned in the Inspector!");
        }

        // Add EventTrigger listeners for backward button
        if (buttonBackward != null)
        {
            AddEventTrigger(buttonBackward.gameObject, EventTriggerType.PointerDown, (eventData) => { MoveLeft(); });
            AddEventTrigger(buttonBackward.gameObject, EventTriggerType.PointerUp, (eventData) => { StopMoving(); });
        }
        else
        {
            Debug.LogError("ButtonBackward is not assigned in the Inspector!");
        }
    }

    // Update is called once per frame
    private void Update()
    {
        // Move Orion along the X-axis only when the buttons are clicked
        if (moveDirection != 0)
        {
            // Calculate new position without clamping
            Vector3 newPosition = transform.position + Vector3.right * moveDirection * moveSpeed * Time.deltaTime;
            transform.position = newPosition; // Move along the X-axis
            
            // Update animation states based on movement
            orionAnimator.SetBool("IsWalking", true); // Set IsWalking to true if moving
            orionAnimator.SetBool("IsBackwards", moveDirection < 0); // Set IsBackwards based on direction
        }
        else
        {
            orionAnimator.SetBool("IsWalking", false); // Set IsWalking to false if not moving
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
        orionAnimator.SetBool("IsBackwards", false); // Reset IsBackwards parameter
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
