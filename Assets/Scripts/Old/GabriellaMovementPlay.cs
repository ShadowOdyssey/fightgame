using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using Unity.VisualScripting;

public class GabriellaMovementPlay : MonoBehaviour
{
    public RoundManager roundSystem;

    public Button buttonForward;                 // Button to move right
    public Button buttonBackward;                // Button to move left
    public Button buttonAttack1;                 // Button for Attack 1
    public Button buttonAttack2;                 // Button for Attack 2
    public Button buttonAttack3;                 // Button for Attack 3

    public Transform enemyTransform;             // Reference to any opponent transform
    public GameObject hitEffect;                 // GameObject of Hit Effect that will be Set Active true or false to show it on screen

    public int totalLife = 100;                  // Current player life

    public float attackRange = 14f;              // Distance player can attack opponent
    public float moveSpeed = 2f;                 // Speed of movement for small steps
    public float stepSize = 0.1f;                // Set the size of each small step
    public float minimumDistance = 1.5f;         // Minimum distance to maintain from Marcus
    public float colliderBuffer = 0.5f;          // Buffer to keep distance from Marcus

    private MarcusAI enemySystem;                // Get the system from the current opponent in scene
    private Animator gabriellaAnimator;          // Reference to the Animator component
    private float moveDirection = 0f;            // Direction of movement (right or left)
    private bool isMovingForward = false;        // Tracks if forward button is held
    private bool isMovingBackward = false;       // Tracks if backward button is held
    private bool isAttacking = false;            // Tracks if an attack animation is playing
    private bool isHit = false;                  // Tracks if player is hit
    private bool checkDamage = false;            // Track if opponent can receive damage

    private void Awake()
    {
        gabriellaAnimator = gameObject.GetComponent<Animator>();
        enemySystem = GameObject.FindGameObjectWithTag("Opponent").GetComponent<MarcusAI>();

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

    private void FixedUpdate()
    {
        // Ensure movement only happens when buttons are held and not during attacks
        if (isAttacking == false && roundSystem.roundStarted == true && isHit == false) // Check if round started so Gabriella can move and if Gabriella is not attacking
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
                AnimIsIdle();
            }

            if (isMovingBackward == true || isMovingForward == true) // Check if Gabriella is moving so apply new position, turned 2 lines code into 1 since both forward and backward calls same method
            {
                // It is not yet right! - Felipe
                Vector3 newPosition = transform.localPosition + Vector3.forward * moveDirection * stepSize;
                transform.localPosition = newPosition;
            }
        }

        if (isAttacking == true && enemySystem.distanceToTarget < attackRange)
        {
            if (checkDamage == true)
            {
                enemySystem.TakeDamage(20);
                checkDamage = false;
            }
        }

        if (isHit == true)
        {
            transform.position = new Vector3 (transform.position.x, transform.position.y, transform.position.z - moveSpeed * Time.deltaTime * 3f);
        }
    }

    // Stop movement and set idle animation
    private void AnimIsIdle() // StopMoving is being called alot, consuming processing and it is bad to mobile, so...
    {
        // Check if Gabriella moved forward to StopMoving trigger the boolean change in the animation parameter

        if (gabriellaAnimator.GetBool("isForward") == true) // Check if Gabriella moved forward to StopMoving trigger the boolean change in the animation parameter
        {
            //Debug.Log("Gabriella stopped to move forward");
            gabriellaAnimator.SetBool("isForward", false); // Values in parameters should be low case in the first letter because is variable name - Felipe
        }

        if (gabriellaAnimator.GetBool("isBackward") == true) // Check if Gabriella moved backward to StopMoving trigger the boolean change in the animation parameter
        {
            //Debug.Log("Gabriella stopped to move backward");
            gabriellaAnimator.SetBool("isBackward", false); // Values in parameters should be low case in the first letter because is variable name - Felipe
        }

        if (gabriellaAnimator.GetBool("isHit") == true)
        {
            gabriellaAnimator.SetBool("isHit", false); // Values in parameters should be low case in the first letter because is variable name - Felipe
        }

        if (gabriellaAnimator.GetBool("isAttack1") == true)
        {
            gabriellaAnimator.SetBool("isAttack1", false); // Values in parameters should be low case in the first letter because is variable name - Felipe
        }

        if (gabriellaAnimator.GetBool("isAttack2") == true)
        {
            gabriellaAnimator.SetBool("isAttack2", false); // Values in parameters should be low case in the first letter because is variable name - Felipe
        }

        if (gabriellaAnimator.GetBool("isAttack3") == true)
        {
            gabriellaAnimator.SetBool("isAttack3", false); // Values in parameters should be low case in the first letter because is variable name - Felipe
        }

        if (gabriellaAnimator.GetBool("isIdle") == false)
        {
            gabriellaAnimator.SetBool("isIdle", true); // Values in parameters should be low case in the first letter because is variable name - Felipe
        }

        // If both triggers to be false, execute nothing, so it saves processing instead to apply false to both animation parameters in each frame, it only will trigger if is true and only once
    }

    // Button press handlers
    public void OnMoveRightButtonPressed(BaseEventData eventData)
    {
        if (CanMoveForward())
        {
            if (roundSystem.roundStarted == true)
            {
                //Debug.Log("Gabriella is moving forward");

                isMovingForward = true;
                isMovingBackward = false;
            }
            else
            {
                //Debug.Log("Gabriella cant move forward because round not started yet");

                if (isMovingForward == true) // Check if any move boolean is activate when Gabriela cant move and deactivate it
                {
                    isMovingForward = false;
                    isMovingBackward = false;
                }
            }
        }
    }

    public void OnMoveLeftButtonPressed(BaseEventData eventData)
    {
        if (CanMoveBackward())
        {
            if (roundSystem.roundStarted == true)
            {
                //Debug.Log("Gabriella is moving backward");

                isMovingBackward = true;
                isMovingForward = false;
            }
            else
            {
                //Debug.Log("Gabriella cant move backward because round not started yet");

                if (isMovingBackward == true) // Check if any boolean is activate when Gabriela cant move and deactivate it
                {
                    isMovingBackward = false;
                    isMovingForward = false;
                }
            }
        }
    }

    // Method to handle hit from Marcus
    public void TakeHit(int damageAmmount)
    {
        if (isAttacking == true) // Check if player is attacking
        {
            isAttacking = false; // Disable it because we will activate Hit animation
        }

        if (isHit == false)
        {
            Debug.Log("Gabriella got a hit and got " + damageAmmount + " of damage!");

            totalLife = totalLife - damageAmmount;

            if (totalLife > 0)
            {
                AnimIsHit();
                hitEffect.SetActive(true); // Show hit effect
                Invoke("DisableEffect", 1f);
            }
            else
            {
                // Game Over
            }

            isHit = true;
        }
    }

    // Method to start moving right
    private void MoveRight()
    {
        if (gabriellaAnimator.GetBool("isForward") == false) // Check if MoveForward is false to trigger it only 1 time and to save processing this way - Felipe
        {
            //Debug.Log("Gabriella moved to right");

            moveDirection = 1f; // Setup new direction only once before to apply new position - Felipe
            gabriellaAnimator.SetBool("isForward", true); // Values in parameters should be low case in the first letter because is variable name - Felipe
            gabriellaAnimator.SetBool("isIdle", false); // Values in parameters should be low case in the first letter because is variable name - Felipe
            gabriellaAnimator.SetBool("isBackward", false); // Values in parameters should be low case in the first letter because is variable name - Felipe
            gabriellaAnimator.SetBool("isHit", false); // Values in parameters should be low case in the first letter because is variable name - Felipe
            gabriellaAnimator.SetBool("isAttack1", false); // Values in parameters should be low case in the first letter because is variable name - Felipe
            gabriellaAnimator.SetBool("isAttack2", false); // Values in parameters should be low case in the first letter because is variable name - Felipe
            gabriellaAnimator.SetBool("isAttack3", false); // Values in parameters should be low case in the first letter because is variable name - Felipe
        }
    }

    // Method to start moving left
    private void MoveLeft() // MoveLeft is being called alot, consuming processing and it is bad to mobile, so...
    {
        if (gabriellaAnimator.GetBool("isBackward") == false) // Check if MoveBackwards is false to trigger it only 1 time and to save processing this way - Felipe
        {
            //Debug.Log("Gabriella moved to left");

            moveDirection = -1f; // Setup new direction only once before to apply new position - Felipe
            gabriellaAnimator.SetBool("isBackward", true); // Values in parameters should be low case in the first letter because is variable name - Felipe
            gabriellaAnimator.SetBool("isIdle", false); // Values in parameters should be low case in the first letter because is variable name - Felipe
            gabriellaAnimator.SetBool("isForward", false); // Values in parameters should be low case in the first letter because is variable name - Felipe
            gabriellaAnimator.SetBool("isHit", false); // Values in parameters should be low case in the first letter because is variable name - Felipe
            gabriellaAnimator.SetBool("isAttack1", false); // Values in parameters should be low case in the first letter because is variable name - Felipe
            gabriellaAnimator.SetBool("isAttack2", false); // Values in parameters should be low case in the first letter because is variable name - Felipe
            gabriellaAnimator.SetBool("isAttack3", false); // Values in parameters should be low case in the first letter because is variable name - Felipe
        }
    }

    // Method when player get hit
    private void AnimIsHit()
    {
        if (gabriellaAnimator.GetBool("isHit") == false)
        {
            gabriellaAnimator.SetBool("isHit", true); // Trigger isHit animation - Values in parameters should be low case in the first letter because is variable name - Felipe
            gabriellaAnimator.SetBool("isIdle", false); // Values in parameters should be low case in the first letter because is variable name - Felipe
            gabriellaAnimator.SetBool("isForward", false); // Values in parameters should be low case in the first letter because is variable name - Felipe
            gabriellaAnimator.SetBool("isBackward", false); // Values in parameters should be low case in the first letter because is variable name - Felipe
            gabriellaAnimator.SetBool("isAttack1", false); // Values in parameters should be low case in the first letter because is variable name - Felipe
            gabriellaAnimator.SetBool("isAttack2", false); // Values in parameters should be low case in the first letter because is variable name - Felipe
            gabriellaAnimator.SetBool("isAttack3", false); // Values in parameters should be low case in the first letter because is variable name - Felipe
        }
    }

    // Method when player using basic attack
    private void AnimIsAttack1()
    {
        if (gabriellaAnimator.GetBool("isAttack1") == false)
        {
            gabriellaAnimator.SetBool("isAttack1", true); // Values in parameters should be low case in the first letter because is variable name - Felipe
            gabriellaAnimator.SetBool("isIdle", false); // Values in parameters should be low case in the first letter because is variable name - Felipe
            gabriellaAnimator.SetBool("isForward", false); // Values in parameters should be low case in the first letter because is variable name - Felipe
            gabriellaAnimator.SetBool("isBackward", false); // Values in parameters should be low case in the first letter because is variable name - Felipe
            gabriellaAnimator.SetBool("isHit", false); // Trigger isHit animation - Values in parameters should be low case in the first letter because is variable name - Felipe
            gabriellaAnimator.SetBool("isAttack2", false); // Values in parameters should be low case in the first letter because is variable name - Felipe
            gabriellaAnimator.SetBool("isAttack3", false); // Values in parameters should be low case in the first letter because is variable name - Felipe
        }
    }

    private void AnimIsAttack2()
    {
        if (gabriellaAnimator.GetBool("isAttack2") == false)
        {
            gabriellaAnimator.SetBool("isAttack2", true); // Values in parameters should be low case in the first letter because is variable name - Felipe
            gabriellaAnimator.SetBool("isIdle", false); // Values in parameters should be low case in the first letter because is variable name - Felipe
            gabriellaAnimator.SetBool("isForward", false); // Values in parameters should be low case in the first letter because is variable name - Felipe
            gabriellaAnimator.SetBool("isBackward", false); // Values in parameters should be low case in the first letter because is variable name - Felipe
            gabriellaAnimator.SetBool("isHit", false); // Trigger isHit animation - Values in parameters should be low case in the first letter because is variable name - Felipe
            gabriellaAnimator.SetBool("isAttack1", false); // Values in parameters should be low case in the first letter because is variable name - Felipe
            gabriellaAnimator.SetBool("isAttack3", false); // Values in parameters should be low case in the first letter because is variable name - Felipe
        }
    }

    private void AnimIsAttack3()
    {
        if (gabriellaAnimator.GetBool("isAttack3") == false)
        {
            gabriellaAnimator.SetBool("isAttack3", true); // Values in parameters should be low case in the first letter because is variable name - Felipe
            gabriellaAnimator.SetBool("isIdle", false); // Values in parameters should be low case in the first letter because is variable name - Felipe
            gabriellaAnimator.SetBool("isForward", false); // Values in parameters should be low case in the first letter because is variable name - Felipe
            gabriellaAnimator.SetBool("isBackward", false); // Values in parameters should be low case in the first letter because is variable name - Felipe
            gabriellaAnimator.SetBool("isHit", false); // Trigger isHit animation - Values in parameters should be low case in the first letter because is variable name - Felipe
            gabriellaAnimator.SetBool("isAttack1", false); // Values in parameters should be low case in the first letter because is variable name - Felipe
            gabriellaAnimator.SetBool("isAttack2", false); // Values in parameters should be low case in the first letter because is variable name - Felipe
        }
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

    public void OnAttack1ButtonPressed()
    {
        if (isAttacking == false && roundSystem.roundStarted == true && isHit == false)
        {
            Debug.Log("Gabriella activated Attack 1");
            isAttacking = true;
            AnimIsAttack1();
        }
    }

    public void OnAttack2ButtonPressed()
    {
        if (isAttacking == false && roundSystem.roundStarted == true && isHit == false)
        {
            Debug.Log("Gabriella activated Attack 2");
            isAttacking = true;
            AnimIsAttack2();
        }
    }

    public void OnAttack3ButtonPressed()
    {
        if (isAttacking == false && roundSystem.roundStarted == true && isHit == false)
        {
            Debug.Log("Gabriella activated Attack 3");
            isAttacking = true;
            AnimIsAttack3();
        }
    }

    public void IsHitAnimationFinished() // It shows zero references but is activated by the last frame of Hit animation
    {
        isHit = false; // Hit animation finished
        AnimIsIdle(); // Reset animation to Idle
    }

    public void AttackAnimFinished() // It shows zero references but is activated by the last frame of any Attack animation
    {
        checkDamage = true;
        isAttacking = false; // Attack animation finished
        AnimIsIdle(); // Reset animation to Idle
    }

    // Check if Gabriella can move forward without colliding with Marcus
    private bool CanMoveForward()
    {
        return Vector3.Distance(transform.localPosition, enemyTransform.localPosition) > (minimumDistance + colliderBuffer); // It is not yet right! - Felipe
    }

    // Check if Gabriella can move backward without colliding with Marcus
    private bool CanMoveBackward()
    {
        return Vector3.Distance(transform.localPosition, enemyTransform.localPosition) > (minimumDistance + colliderBuffer); // It is not yet right! - Felipe
    }

    private void DisableEffect()
    {
        hitEffect.SetActive(false);
    }
}
