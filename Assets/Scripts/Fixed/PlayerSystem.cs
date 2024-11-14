using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class PlayerSystem : MonoBehaviour
{
    #region Variables

    [Header("Animator Setup")]
    [Tooltip("Attach current player Animator component here")]
    public Animator PlayerAnimator;

    [Header("Buttons UI Setup")]
    [Tooltip("Attach current player Button Forward component here")]
    public Button buttonForward;
    [Tooltip("Attach current player Button Backward component here")]
    public Button buttonBackward;
    [Tooltip("Attach current player Button Attack 1 component here")]
    public Button buttonAttack1;
    [Tooltip("Attach current player Button Attack 2 component here")]
    public Button buttonAttack2;
    [Tooltip("Attach current player Button Attack 3 component here")]
    public Button buttonAttack3;

    [Header("Hit Effect Setup")]
    [Tooltip("Attach current player HitEffect GameObject here")]
    public GameObject hitEffect;

    [Header("Player Setup")]
    [Tooltip("Setup actual player health")]
    public int totalHealth = 100;
    [Tooltip("Setup actual player attack range")]
    public float attackRange = 14f;
    [Tooltip("Setup actual player movement speed")]
    public float moveSpeed = 2f;
    [Tooltip("Setup actual player step size to change movement speed")]
    public float stepSize = 0.1f;

    #region Hidden Variables

    [Header("Monitor")] // Turn variables into public to show monitor
    [Tooltip("Actual Round System from RoundManager object in the current scene, it will be loaded when scene to awake")]
    private RoundManager roundSystem;
    [Tooltip("Actual Enemy System from selected enemy by IA or multiplayer, it will be loaded when scene to awake")]
    private EnemySystem enemySystem;
    [Tooltip("Actual Enemy Transform from selected enemy by IA or multiplayer, it will be loaded when scene to awake")]
    private Transform enemyBody;
    [Tooltip("Current movement direction player is using when moving")]
    private float moveDirection = 0f;
    [Tooltip("If enabled means player is moving forward, it means forward button is being held")]
    private bool isMovingForward = false;
    [Tooltip("If enabled means player is moving backward, it means backward button is being held")]
    private bool isMovingBackward = false;
    [Tooltip("If enabled means player is attacking")]
    private bool isAttacking = false;
    [Tooltip("If enabled means player got a hit")]
    private bool isHit = false;
    [Tooltip("If enabled means player dealed a damage to an opponent")]
    private bool checkDamage = false;
    
    #endregion

    #endregion

    #region Loading Components

    private void Awake()
    {
        // When multiplayer to be done we need to look for the right components, the other components declared dont need to be found, just attached in Inspector

        roundSystem = GameObject.Find("RoundManager").GetComponent<RoundManager>();
        enemySystem = GameObject.Find("Marcus").GetComponent<EnemySystem>();
        enemyBody = GameObject.Find("Marcus").GetComponent<Transform>();

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

    #endregion

    #region Real Time operations

    private void Update()
    {
        if (checkDamage == true && enemySystem.distanceToTarget < attackRange)
        {
            enemySystem.TakeDamage(20);
            checkDamage = false;
        }
    }

    private void FixedUpdate()
    {
        // Ensure movement only happens when buttons are held and not during attacks
        if (isAttacking == false && roundSystem.roundStarted == true && isHit == false) // Check if round started so Player can move and if Player is not attacking
        {
            if (isMovingForward == true && isMovingBackward == false)
            {
                MoveRight();
            }
            else if (isMovingBackward == true && isMovingForward == false)
            {
                MoveLeft();
            }
            else
            {
                AnimIsIdle();
            }

            if (isMovingBackward == true || isMovingForward == true)
            {
                // Check if Player is moving so apply new position, turned 2 lines code into 1 since both forward and backward calls same method
                Vector3 newPosition = transform.localPosition + Vector3.forward * moveDirection * stepSize;
                transform.localPosition = newPosition;
            }
        }

        if (isHit == true)
        {
            transform.position = new Vector3 (transform.position.x, transform.position.y, transform.position.z - moveSpeed * Time.deltaTime * 3f);
        }
    }

    #endregion

    #region Stop movement and set idle animation

    private void AnimIsIdle() // StopMoving is being called alot, consuming processing and it is bad to mobile, so...
    {
        // Check if Player moved forward to StopMoving trigger the boolean change in the animation parameter

        if (PlayerAnimator.GetBool("isForward") == true) // Check if Player moved forward to StopMoving trigger the boolean change in the animation parameter
        {
            //Debug.Log("Player stopped to move forward");
            PlayerAnimator.SetBool("isForward", false); // Values in parameters should be low case in the first letter because is variable name - Felipe
        }

        if (PlayerAnimator.GetBool("isBackward") == true) // Check if Player moved backward to StopMoving trigger the boolean change in the animation parameter
        {
            //Debug.Log("Player stopped to move backward");
            PlayerAnimator.SetBool("isBackward", false); // Values in parameters should be low case in the first letter because is variable name - Felipe
        }

        if (PlayerAnimator.GetBool("isHit") == true)
        {
            PlayerAnimator.SetBool("isHit", false); // Values in parameters should be low case in the first letter because is variable name - Felipe
        }

        if (PlayerAnimator.GetBool("isAttack1") == true)
        {
            PlayerAnimator.SetBool("isAttack1", false); // Values in parameters should be low case in the first letter because is variable name - Felipe
        }

        if (PlayerAnimator.GetBool("isAttack2") == true)
        {
            PlayerAnimator.SetBool("isAttack2", false); // Values in parameters should be low case in the first letter because is variable name - Felipe
        }

        if (PlayerAnimator.GetBool("isAttack3") == true)
        {
            PlayerAnimator.SetBool("isAttack3", false); // Values in parameters should be low case in the first letter because is variable name - Felipe
        }

        if (PlayerAnimator.GetBool("isIdle") == false)
        {
            PlayerAnimator.SetBool("isIdle", true); // Values in parameters should be low case in the first letter because is variable name - Felipe
        }

        // If both triggers to be false, execute nothing, so it saves processing instead to apply false to both animation parameters in each frame, it only will trigger if is true and only once
    }

    #endregion

    #region Method to handle hit from Enemy

    public void TakeHit(int damageAmmount)
    {
        if (isAttacking == true) // Check if player is attacking
        {
            isAttacking = false; // Disable it because we will activate Hit animation
        }

        if (isHit == false)
        {
            Debug.Log("Player got a hit and got " + damageAmmount + " of damage!");

            totalHealth = totalHealth - damageAmmount;
            
            roundSystem.ApplyDamageToPlayer(damageAmmount); // Inform RoundManager that Player got damage by Enemy

            if (totalHealth > 0)
            {
                AnimIsHit();
                hitEffect.SetActive(true); // Show hit effect
                Invoke(nameof(DisableEffect), 1f);
            }
            else
            {
                // Game Over
            }

            isHit = true;
        }
    }

    #endregion

    #region Button press handlers

    public void OnMoveRightButtonPressed(BaseEventData eventData)
    {
        if (roundSystem.roundStarted == true)
        {
            //Debug.Log("Player is moving forward");

            isMovingForward = true;
            isMovingBackward = false;
        }
        else
        {
            //Debug.Log("Player cant move forward because round not started yet");

            if (isMovingForward == true) // Check if any move boolean is activate when Gabriela cant move and deactivate it
            {
                isMovingForward = false;
                isMovingBackward = false;
            }
        }
    }

    public void OnMoveLeftButtonPressed(BaseEventData eventData)
    {
        if (roundSystem.roundStarted == true)
        {
            //Debug.Log("Player is moving backward");

            isMovingBackward = true;
            isMovingForward = false;
        }
        else
        {
            //Debug.Log("Player cant move backward because round not started yet");

            if (isMovingBackward == true) // Check if any boolean is activate when Gabriela cant move and deactivate it
            {
                isMovingBackward = false;
                isMovingForward = false;
            }
        }
    }

    #endregion

    #region Method to start moving right
    
    private void MoveRight()
    {
        if (PlayerAnimator.GetBool("isForward") == false) // Check if MoveForward is false to trigger it only 1 time and to save processing this way - Felipe
        {
            //Debug.Log("Player moved to right");

            moveDirection = 1f; // Setup new direction only once before to apply new position - Felipe
            PlayerAnimator.SetBool("isForward", true); // Values in parameters should be low case in the first letter because is variable name - Felipe
            PlayerAnimator.SetBool("isIdle", false); // Values in parameters should be low case in the first letter because is variable name - Felipe
            PlayerAnimator.SetBool("isBackward", false); // Values in parameters should be low case in the first letter because is variable name - Felipe
            PlayerAnimator.SetBool("isHit", false); // Values in parameters should be low case in the first letter because is variable name - Felipe
            PlayerAnimator.SetBool("isAttack1", false); // Values in parameters should be low case in the first letter because is variable name - Felipe
            PlayerAnimator.SetBool("isAttack2", false); // Values in parameters should be low case in the first letter because is variable name - Felipe
            PlayerAnimator.SetBool("isAttack3", false); // Values in parameters should be low case in the first letter because is variable name - Felipe
        }
    }

    #endregion

    #region Method to start moving left
    
    private void MoveLeft() // MoveLeft is being called alot, consuming processing and it is bad to mobile, so...
    {
        if (PlayerAnimator.GetBool("isBackward") == false) // Check if MoveBackwards is false to trigger it only 1 time and to save processing this way - Felipe
        {
            //Debug.Log("Player moved to left");

            moveDirection = -1f; // Setup new direction only once before to apply new position - Felipe
            PlayerAnimator.SetBool("isBackward", true); // Values in parameters should be low case in the first letter because is variable name - Felipe
            PlayerAnimator.SetBool("isIdle", false); // Values in parameters should be low case in the first letter because is variable name - Felipe
            PlayerAnimator.SetBool("isForward", false); // Values in parameters should be low case in the first letter because is variable name - Felipe
            PlayerAnimator.SetBool("isHit", false); // Values in parameters should be low case in the first letter because is variable name - Felipe
            PlayerAnimator.SetBool("isAttack1", false); // Values in parameters should be low case in the first letter because is variable name - Felipe
            PlayerAnimator.SetBool("isAttack2", false); // Values in parameters should be low case in the first letter because is variable name - Felipe
            PlayerAnimator.SetBool("isAttack3", false); // Values in parameters should be low case in the first letter because is variable name - Felipe
        }
    }

    #endregion

    #region Method when player get hit

    private void AnimIsHit()
    {
        if (PlayerAnimator.GetBool("isHit") == false)
        {
            PlayerAnimator.SetBool("isHit", true); // Trigger isHit animation - Values in parameters should be low case in the first letter because is variable name - Felipe
            PlayerAnimator.SetBool("isIdle", false); // Values in parameters should be low case in the first letter because is variable name - Felipe
            PlayerAnimator.SetBool("isForward", false); // Values in parameters should be low case in the first letter because is variable name - Felipe
            PlayerAnimator.SetBool("isBackward", false); // Values in parameters should be low case in the first letter because is variable name - Felipe
            PlayerAnimator.SetBool("isAttack1", false); // Values in parameters should be low case in the first letter because is variable name - Felipe
            PlayerAnimator.SetBool("isAttack2", false); // Values in parameters should be low case in the first letter because is variable name - Felipe
            PlayerAnimator.SetBool("isAttack3", false); // Values in parameters should be low case in the first letter because is variable name - Felipe
        }
    }

    #endregion

    #region Method when player using attacks

    private void AnimIsAttack1()
    {
        if (PlayerAnimator.GetBool("isAttack1") == false)
        {
            PlayerAnimator.SetBool("isAttack1", true); // Values in parameters should be low case in the first letter because is variable name - Felipe
            PlayerAnimator.SetBool("isIdle", false); // Values in parameters should be low case in the first letter because is variable name - Felipe
            PlayerAnimator.SetBool("isForward", false); // Values in parameters should be low case in the first letter because is variable name - Felipe
            PlayerAnimator.SetBool("isBackward", false); // Values in parameters should be low case in the first letter because is variable name - Felipe
            PlayerAnimator.SetBool("isHit", false); // Trigger isHit animation - Values in parameters should be low case in the first letter because is variable name - Felipe
            PlayerAnimator.SetBool("isAttack2", false); // Values in parameters should be low case in the first letter because is variable name - Felipe
            PlayerAnimator.SetBool("isAttack3", false); // Values in parameters should be low case in the first letter because is variable name - Felipe
        }
    }

    private void AnimIsAttack2()
    {
        if (PlayerAnimator.GetBool("isAttack2") == false)
        {
            PlayerAnimator.SetBool("isAttack2", true); // Values in parameters should be low case in the first letter because is variable name - Felipe
            PlayerAnimator.SetBool("isIdle", false); // Values in parameters should be low case in the first letter because is variable name - Felipe
            PlayerAnimator.SetBool("isForward", false); // Values in parameters should be low case in the first letter because is variable name - Felipe
            PlayerAnimator.SetBool("isBackward", false); // Values in parameters should be low case in the first letter because is variable name - Felipe
            PlayerAnimator.SetBool("isHit", false); // Trigger isHit animation - Values in parameters should be low case in the first letter because is variable name - Felipe
            PlayerAnimator.SetBool("isAttack1", false); // Values in parameters should be low case in the first letter because is variable name - Felipe
            PlayerAnimator.SetBool("isAttack3", false); // Values in parameters should be low case in the first letter because is variable name - Felipe
        }
    }

    private void AnimIsAttack3()
    {
        if (PlayerAnimator.GetBool("isAttack3") == false)
        {
            PlayerAnimator.SetBool("isAttack3", true); // Values in parameters should be low case in the first letter because is variable name - Felipe
            PlayerAnimator.SetBool("isIdle", false); // Values in parameters should be low case in the first letter because is variable name - Felipe
            PlayerAnimator.SetBool("isForward", false); // Values in parameters should be low case in the first letter because is variable name - Felipe
            PlayerAnimator.SetBool("isBackward", false); // Values in parameters should be low case in the first letter because is variable name - Felipe
            PlayerAnimator.SetBool("isHit", false); // Trigger isHit animation - Values in parameters should be low case in the first letter because is variable name - Felipe
            PlayerAnimator.SetBool("isAttack1", false); // Values in parameters should be low case in the first letter because is variable name - Felipe
            PlayerAnimator.SetBool("isAttack2", false); // Values in parameters should be low case in the first letter because is variable name - Felipe
        }
    }

    #endregion

    #region Utility method to add event triggers to buttons

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

    #endregion

    #region Called when the button is released

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
            Debug.Log("Player activated Attack 1");
            isAttacking = true;
            AnimIsAttack1();
        }
    }

    public void OnAttack2ButtonPressed()
    {
        if (isAttacking == false && roundSystem.roundStarted == true && isHit == false)
        {
            Debug.Log("Player activated Attack 2");
            isAttacking = true;
            AnimIsAttack2();
        }
    }

    public void OnAttack3ButtonPressed()
    {
        if (isAttacking == false && roundSystem.roundStarted == true && isHit == false)
        {
            Debug.Log("Player activated Attack 3");
            isAttacking = true;
            AnimIsAttack3();
        }
    }

    #endregion

    #region Called when animation frames reach last frame

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

    #endregion

    #region Effect Operations

    private void DisableEffect()
    {
        hitEffect.SetActive(false);
    }

    #endregion
}
