using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class GabriellaMovementPlay : MonoBehaviour
{
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

    private void Start()
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

        // Store Marcus's initial rotation
        if (marcusTransform != null)
        {
            Debug.Log("Saved initial Marcus rotation in Gabriella script");
            lastMarcusRotation = marcusTransform.rotation;
        }
    }

    private void FixedUpdate()
    {
        // Ensure movement only happens when buttons are held and not during attacks
        if (!isAttacking && !isHit)
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

        // Synchronize Gabriella's rotation to the opposite of Marcus's rotation
        if (marcusTransform != null && marcusTransform.rotation != lastMarcusRotation)
        {
            // Apply an inverse rotation to Gabriella
            Debug.Log("Applied inverse rotation in Marcus character from Gabriella rotation");
            transform.rotation = Quaternion.Inverse(marcusTransform.rotation);
            lastMarcusRotation = marcusTransform.rotation;
        }
    }

    // Check if Gabriella can move forward without colliding with Marcus
    private bool CanMoveForward()
    {
        return Vector3.Distance(transform.position, marcusTransform.position) > (minimumDistance + colliderBuffer);
    }

    // Check if Gabriella can move backward without colliding with Marcus
    private bool CanMoveBackward()
    {
        return Vector3.Distance(transform.position, marcusTransform.position) > (minimumDistance + colliderBuffer);
    }

    // Button press handlers
    public void OnMoveRightButtonPressed(BaseEventData eventData)
    {
        if (CanMoveForward())
        {
            isMovingForward = true;
            isMovingBackward = false;
        }
    }

    public void OnMoveLeftButtonPressed(BaseEventData eventData)
    {
        if (CanMoveBackward())
        {
            isMovingBackward = true;
            isMovingForward = false;
        }
    }

    // Method to start moving right
    private void MoveRight()
    {
        Debug.Log("Moved to right");

        moveDirection = 1f;
        Vector3 newPosition = transform.position + Vector3.right * moveDirection * stepSize;
        transform.position = newPosition;
        gabriellaAnimator.SetBool("IsGabriellaForward", true);
        gabriellaAnimator.SetBool("IsGabriellaBackwards", false);
    }

    // Method to start moving left
    private void MoveLeft()
    {
        Debug.Log("Moved to left");

        moveDirection = -1f;
        Vector3 newPosition = transform.position + Vector3.right * moveDirection * stepSize;
        transform.position = newPosition;
        gabriellaAnimator.SetBool("IsGabriellaForward", false);
        gabriellaAnimator.SetBool("IsGabriellaBackwards", true);
    }

    // Stop movement and set idle animation
    private void StopMoving()
    {
        gabriellaAnimator.SetBool("IsGabriellaForward", false);
        gabriellaAnimator.SetBool("IsGabriellaBackwards", false);
    }

    public void OnAttack1ButtonPressed()
    {
        gabriellaAnimator.SetTrigger("Attack1");
        isAttacking = true;
        StopMoving();
        StartCoroutine(EndAttackAnimation());
    }

    public void OnAttack2ButtonPressed()
    {
        gabriellaAnimator.SetTrigger("Attack2");
        isAttacking = true;
        StopMoving();
        StartCoroutine(EndAttackAnimation());
    }

    public void OnAttack3ButtonPressed()
    {
        gabriellaAnimator.SetTrigger("Attack3");
        isAttacking = true;
        StopMoving();
        StartCoroutine(EndAttackAnimation());
    }

    // Method to handle hit from Marcus
    public void TakeHit()
    {
        if (!isHit)
        {
            isHit = true;
            gabriellaAnimator.SetTrigger("React"); // Trigger reaction animation
            Instantiate(hitEffectPrefab, transform.position, Quaternion.identity); // Show hit effect
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
        isMovingForward = false;
        isMovingBackward = false;
    }
}
