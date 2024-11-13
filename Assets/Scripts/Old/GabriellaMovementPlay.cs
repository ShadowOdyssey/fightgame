using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class GabriellaMovementPlay : MonoBehaviour
{
    public RoundManager roundSystem;

    public Button buttonForward;    // Button to move right
    public Button buttonBackward;   // Button to move left
    public Button buttonAttack1;    // Button for Attack 1
    public Button buttonAttack2;    // Button for Attack 2
    public Button buttonAttack3;    // Button for Attack 3

    public Transform marcusTransform;   // Reference to Marcus's transform
    public GameObject hitEffectPrefab;  // Prefab to show when Gabriella is hit

    public float moveSpeed = 2f;        // Speed of movement for small steps
    private float moveDirection = 0f;   // Direction of movement (right or left)

    public float stepSize = 0.1f;       // Set the size of each small step
    public float minimumDistance = 1.5f; // Minimum distance to maintain from Marcus
    public float colliderBuffer = 0.5f; // Buffer to keep distance from Marcus

    private Animator gabriellaAnimator;  // Reference to the Animator component
    private Quaternion lastMarcusRotation;  // Tracks Marcus's last rotation

    private bool isMovingForward = false; // Tracks if forward button is held
    private bool isMovingBackward = false; // Tracks if backward button is held
    private bool isAttacking = false;     // Tracks if an attack animation is playing
    private bool isHit = false;            // Tracks if Gabriella is hit

    private void Awake()
    {
        gabriellaAnimator = GetComponent<Animator>();

        if (gabriellaAnimator == null)
        {
            Debug.LogError("Animator component missing from Gabriella! Please attach the Animator component.");
        }

        if (buttonForward != null)
        {
            AddEventTrigger(buttonForward, EventTriggerType.PointerDown, OnMoveRightButtonPressed);
            AddEventTrigger(buttonForward, EventTriggerType.PointerUp, OnMoveButtonReleased);
        }
        if (buttonBackward != null)
        {
            AddEventTrigger(buttonBackward, EventTriggerType.PointerDown, OnMoveLeftButtonPressed);
            AddEventTrigger(buttonBackward, EventTriggerType.PointerUp, OnMoveButtonReleased);
        }
        if (buttonAttack1 != null)
        {
            buttonAttack1.onClick.AddListener(OnAttack1ButtonPressed);
        }
        if (buttonAttack2 != null)
        {
            buttonAttack2.onClick.AddListener(OnAttack2ButtonPressed);
        }
        if (buttonAttack3 != null)
        {
            buttonAttack3.onClick.AddListener(OnAttack3ButtonPressed);
        }
    }

    private void Start()
    {
        /*// Store Marcus's initial rotation
        if (marcusTransform != null)
        {
            Debug.Log("Saved initial Marcus rotation in Gabriella script");
            lastMarcusRotation = marcusTransform.localRotation;

            // We dont need it since characters only moves forward and backward, so them are always facing each other
        }
        */
    }

    private void FixedUpdate()
    {
        // Ensure movement only happens when buttons are held and not during attacks
        if (!isAttacking && !isHit && roundSystem.roundStarted == true)
        {
            if (isMovingForward && CanMoveForward())
            {
                MoveRight();
            }
            else if (isMovingBackward && CanMoveBackward())
            {
                MoveLeft();
            }
            else
            {
                StopMoving();
            }
        }

        if (isMovingBackward == true || isMovingForward == true)
        {
            // It is not yet right! - Felipe
            Vector3 newPosition = transform.localPosition + Vector3.forward * moveDirection * stepSize;
            transform.localPosition = newPosition;
        }

        /*
        // Synchronize Gabriella's rotation to the opposite of Marcus's rotation
        if (marcusTransform != null && marcusTransform.localRotation != lastMarcusRotation)
        {
            // Apply an inverse rotation to Gabriella
            Debug.Log("Applied inverse rotation in Marcus character from Gabriella rotation");
            transform.rotation = Quaternion.Inverse(marcusTransform.localRotation); // It is right! Added the real body of Gabriella that should be moved - Felipe
            lastMarcusRotation = marcusTransform.localRotation; // It is right! - Felipe

            // We dont need it since characters only moves forward and backward, so them are always facing each other
        }
        */
    }

    // Check if Gabriella can move forward without colliding with Marcus
    private bool CanMoveForward()
    {
        return Vector3.Distance(transform.localPosition, marcusTransform.localPosition) > (minimumDistance + colliderBuffer); // It is not yet right! Added the real body of Gabriella that should be moved - Felipe
    }

    // Check if Gabriella can move backward without colliding with Marcus
    private bool CanMoveBackward()
    {
        return Vector3.Distance(transform.localPosition, marcusTransform.localPosition) > (minimumDistance + colliderBuffer); // It is not yet right! Added the real body of Gabriella that should be moved - Felipe
    }

    // Button press handlers
    public void OnMoveRightButtonPressed(BaseEventData eventData)
    {
        if (CanMoveForward())
        {
            if (roundSystem.roundStarted == true)
            {
                Debug.Log("Gabriella is moving forward");
            }
            else
            {
                Debug.Log("Gabriella cant move forward because round not started yet");
            }

            isMovingForward = true;
            isMovingBackward = false;
        }
    }

    public void OnMoveLeftButtonPressed(BaseEventData eventData)
    {
        if (CanMoveBackward())
        {
            if (roundSystem.roundStarted == true)
            {
                Debug.Log("Gabriella is moving backward");
            }
            else
            {
                Debug.Log("Gabriella cant move backard because round not started yet");
            }

            isMovingBackward = true;
            isMovingForward = false;
        }
    }

    // Method to start moving right
    private void MoveRight()
    {
        if (gabriellaAnimator.GetBool("isGabriellaForward") == false) // Check if MoveForward is false to trigger it only 1 time and to save processing this way - Felipe
        {
            Debug.Log("Gabriella moved to right");
            moveDirection = 1f; // Setup new direction only once before to apply new position - Felipe
            gabriellaAnimator.SetBool("isGabriellaForward", true); // Values in parameters should be low case in the first letter because is variable name - Felipe
            gabriellaAnimator.SetBool("isGabriellaBackwards", false); // Values in parameters should be low case in the first letter because is variable name - Felipe
        }
    }

    // Method to start moving left
    private void MoveLeft() // MoveLeft is being called alot, consuming processing and it is bad to mobile, so...
    {
        if (gabriellaAnimator.GetBool("isGabriellaBackwards") == false) // Check if MoveBackwards is false to trigger it only 1 time and to save processing this way - Felipe
        {
            Debug.Log("Gabriella moved to left");

            moveDirection = -1f; // Setup new direction only once before to apply new position - Felipe
            gabriellaAnimator.SetBool("isGabriellaForward", false); // Values in parameters should be low case in the first letter because is variable name - Felipe
            gabriellaAnimator.SetBool("isGabriellaBackwards", true); // Values in parameters should be low case in the first letter because is variable name - Felipe
        }
    }

    // Stop movement and set idle animation
    private void StopMoving() // StopMoving is being called alot, consuming processing and it is bad to mobile, so...
    {
        // Check if Gabriella moved forward to StopMoving trigger the boolean change in the animation parameter

        if (gabriellaAnimator.GetBool("isGabriellaForward") == true) // Check if Gabriella moved forward to StopMoving trigger the boolean change in the animation parameter
        {
            Debug.Log("Gabriella stopped to move forward");
            gabriellaAnimator.SetBool("isGabriellaForward", false); // Values in parameters should be low case in the first letter because is variable name - Felipe
        }

        if (gabriellaAnimator.GetBool("isGabriellaBackwards") == true) // Check if Gabriella moved backward to StopMoving trigger the boolean change in the animation parameter
        {
            Debug.Log("Gabriella stopped to move backward");
            gabriellaAnimator.SetBool("isGabriellaBackwards", false); // Values in parameters should be low case in the first letter because is variable name - Felipe
        }

        // If both triggers to be false, execute nothing, so it saves processing instead to apply false to both animation parameters in each frame, it only will trigger if is true and only once
    }

    public void OnAttack1ButtonPressed()
    {
        Debug.Log("Gabriella activated Attack 1");
        gabriellaAnimator.SetTrigger("attack1"); // Values in parameters should be low case in the first letter because is variable name - Felipe
        isAttacking = true;
        StopMoving();
        StartCoroutine(EndAttackAnimation());
    }

    public void OnAttack2ButtonPressed()
    {
        Debug.Log("Gabriella activated Attack 2");
        gabriellaAnimator.SetTrigger("attack2"); // Values in parameters should be low case in the first letter because is variable name - Felipe
        isAttacking = true;
        StopMoving();
        StartCoroutine(EndAttackAnimation());
    }

    public void OnAttack3ButtonPressed()
    {
        Debug.Log("Gabriella activated Attack 3");
        gabriellaAnimator.SetTrigger("attack3"); // Values in parameters should be low case in the first letter because is variable name - Felipe
        isAttacking = true;
        StopMoving();
        StartCoroutine(EndAttackAnimation());
    }

    // Method to handle hit from Marcus
    public void TakeHit()
    {
        if (!isHit)
        {
            Debug.Log("Gabriella got hit");
            isHit = true;
            gabriellaAnimator.SetTrigger("react"); // Trigger reaction animation - Values in parameters should be low case in the first letter because is variable name - Felipe
            Instantiate(hitEffectPrefab, transform.localPosition, Quaternion.identity); // Show hit effect
            StartCoroutine(HandleHitCooldown());
        }
    }

    // Coroutine to manage attack animation duration
    private System.Collections.IEnumerator EndAttackAnimation()
    {
        yield return new WaitForSeconds(1f); // Adjust duration based on the attack animation length
        isAttacking = false; // Allow movement again after attack animation finishes
    }

    // Coroutine to manage hit cooldown
    private System.Collections.IEnumerator HandleHitCooldown()
    {
        yield return new WaitForSeconds(1f); // Adjust the duration as needed
        isHit = false; // Reset hit state after cooldown
    }

    // Utility method to add event triggers to buttons
    private void AddEventTrigger(Button button, EventTriggerType eventTriggerType, UnityAction<BaseEventData> eventHandler)
    {
        EventTrigger trigger = button.gameObject.AddComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry
        {
            eventID = eventTriggerType
        };
        entry.callback.AddListener(eventHandler);
        trigger.triggers.Add(entry);
    }

    // Called when the button is released
    private void OnMoveButtonReleased(BaseEventData eventData)
    {
        if (isMovingForward == true)
        {
            isMovingForward = false;
        }

        if (isMovingBackward == true)
        {
            isMovingBackward = false;
        }
    }
}
