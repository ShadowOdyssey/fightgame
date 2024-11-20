using UnityEngine;
using UnityEngine.UI; // Required for using UI elements
using UnityEngine.EventSystems; // Required for using EventTrigger

public class AriaMovement : MonoBehaviour
{
    public Button buttonForward;  // Button to move right
    public Button buttonBackward; // Button to move left
    public Button buttonAttack1;  // Button for the first special attack

    public float moveSpeed = 0.2f;  // Speed of movement (adjust as needed for slower movement)
    private float moveDirection = 0f;  // Direction of movement (right or left)

    private Animator ariaAnimator;  // Reference to the Animator component

    public void Start()
    {
        // Get the Animator component from Aria
        ariaAnimator = GetComponent<Animator>();

        // Check if the Animator is available
        if (ariaAnimator == null)
        {
            Debug.LogError("Animator component missing from Aria! Please attach the Animator component.");
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
        // Move Aria only when the buttons are pressed
        if (moveDirection != 0)
        {
            Vector3 newPosition = transform.position + Vector3.right * moveDirection * moveSpeed * Time.deltaTime;
            transform.position = newPosition; // Move along the X-axis

            // Update animation states based on movement
            ariaAnimator.SetBool("IsWalking", true); // Set IsWalking to true if moving
            ariaAnimator.SetBool("IsBackwards", moveDirection < 0); // Set IsBackwards based on direction
        }
        else
        {
            ariaAnimator.SetBool("IsWalking", false); // Set IsWalking to false if not moving
            ariaAnimator.SetBool("IsBackwards", false); // Ensure IsBackwards is reset
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
        ariaAnimator.SetBool("IsWalking", false); // Ensure walking animation is disabled
        ariaAnimator.SetBool("IsBackwards", false); // Reset IsBackwards parameter
    }

    // Special Attack 1 method
    private void Attack1()
    {
        Debug.Log("Attack1 pressed"); // Debug log to confirm the button was pressed
        ariaAnimator.SetTrigger("Attack1"); // Assuming you have an animation trigger for the first attack
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
