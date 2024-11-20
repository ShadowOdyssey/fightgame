using UnityEngine;
using UnityEngine.UI;

public class FemMovement : MonoBehaviour
{
    public float moveSpeed = 5f;  // Speed at which the player moves
    public Button forwardButton;  // UI button for forward movement
    public Button backwardButton; // UI button for backward movement
    public Animator femanimator;  // Reference to the Animator
    private bool isMovingForward = false; // Flag for forward movement
    private bool isMovingBackward = false; // Flag for backward movement

    public void Start()
    {
        // Assign listeners to the forward and backward buttons
        if (forwardButton != null)
        {
            forwardButton.onClick.AddListener(OnForwardButtonPressed);
            forwardButton.onClick.AddListener(OnForwardButtonReleased);
        }

        if (backwardButton != null)
        {
            backwardButton.onClick.AddListener(OnBackwardButtonPressed);
            backwardButton.onClick.AddListener(OnBackwardButtonReleased);
        }
    }

    private void Update()
    {
        if (isMovingForward)
        {
            MoveForward();  // Move forward if forward button is pressed
        }
        else if (isMovingBackward)
        {
            MoveBackward();  // Move backward if backward button is pressed
        }
        else
        {
            StopMoving();  // Stop movement when no button is pressed
        }
    }

    private void MoveForward()
    {
        // Move the character forward
        transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
        // Set forward walking animation to true
        femanimator.SetBool("isWalkingForward", true);
        femanimator.SetBool("isWalkingBackward", false);
    }

    private void MoveBackward()
    {
        // Move the character backward
        transform.Translate(Vector3.back * moveSpeed * Time.deltaTime);
        // Set backward walking animation to true
        femanimator.SetBool("isWalkingBackward", true);
        femanimator.SetBool("isWalkingForward", false);
    }

    private void StopMoving()
    {
        // Set both walking animations to false (return to idle)
        femanimator.SetBool("isWalkingForward", false);
        femanimator.SetBool("isWalkingBackward", false);
    }

    private void OnForwardButtonPressed()
    {
        // Set the flag to move forward
        isMovingForward = true;
        isMovingBackward = false; // Ensure backward movement is stopped
    }

    private void OnForwardButtonReleased()
    {
        // Stop moving forward
        isMovingForward = false;
    }

    private void OnBackwardButtonPressed()
    {
        // Set the flag to move backward
        isMovingBackward = true;
        isMovingForward = false; // Ensure forward movement is stopped
    }

    private void OnBackwardButtonReleased()
    {
        // Stop moving backward
        isMovingBackward = false;
    }
}
