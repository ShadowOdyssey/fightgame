using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class PlayerSystem : MonoBehaviour
{
    #region Variables

    [Header("Animator Setup")]
    [Tooltip("Attach current player Animator component here")]
    public Animator playerAnimator;

    public BoxCollider playerCollider;

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
    [Tooltip("Setup actual player attack range")]
    public float attackRange = 14f;
    [Tooltip("Setup actual player movement speed")]
    public float moveSpeed = 2f;
    [Tooltip("Setup actual player step size to change movement speed")]
    public float stepSize = 0.1f;

    #region Hidden Variables

    [Header("Monitor")] // Turn variables into public to show monitor
    [Tooltip("Actual Camera System from MainCamera object in the current scene, it will be loaded when scene to awake")]
    private CameraSystem cameraSystem;
    [Tooltip("Actual Round System from RoundManager object in the current scene, it will be loaded when scene to awake")]
    private RoundManager roundSystem;
    [Tooltip("Actual Training System from RoundManager object in the current scene, it will be loaded when scene to awake")]
    public TrainingSystem trainingSystem;
    [Tooltip("Actual Cooldown System from RoundManager object in the current scene, it will be loaded when scene to awake")]
    private CooldownSystem cooldownSystem;
    [Tooltip("Actual Enemy System from selected enemy by IA, it will be loaded when scene to awake")]
    private EnemySystem enemySystem;
    [Tooltip("Actual Enemy System Multiplayer from selected enemy by multiplayer, it will be loaded when scene to awake")]
    private OpponentMultiplayer enemySystemMultiplayer;
    [Tooltip("Actual Enemy Transform from selected enemy by IA or multiplayer, it will be loaded when scene to awake")]
    private Transform enemyBody;
    [Tooltip("Initial position from Player to use it when a new round to start to move Player to initial position")]
    private Vector3 initialPosition;
    [Tooltip("Current movement direction player is using when moving")]
    private int moveDirection = 0;
    [Tooltip("Check if Player applied damage in a certain ammount of time, if Enemy not to be inside range when time is over so Player dont dealed damage to Enemy")]
    private float damageTime = 0f;
    [Tooltip("Enabled when rounds starts, it will be triggered automatically")]
    private bool introAnimated = false;
    [Tooltip("If enabled means player is moving forward, it means forward button is being held")]
    private bool isMovingForward = false;
    [Tooltip("If enabled means player is moving backward, it means backward button is being held")]
    private bool isMovingBackward = false;
    [Tooltip("If enabled means player is attacking")]
    private bool isAttacking = false;
    [Tooltip("If enabled means player is used Attack 1 and should wait cooldown to finish")]
    private bool isCooldown1 = false;
    [Tooltip("If enabled means player is used Attack 2 and should wait cooldown to finish")]
    private bool isCooldown2 = false;
    [Tooltip("If enabled means player is used Attack 3 and should wait cooldown to finish")]
    private bool isCooldown3 = false;
    [Tooltip("If enabled means player got a hit")]
    private bool isHit = false;
    [Tooltip("If enabled means player dealed a damage to an opponent")]
    private bool checkDamage = false;
    [Tooltip("If enabled means player completed a tutorial stage in Training Mode")]
    public bool completedTutorial = false;
    [Tooltip("If enabled means player triggers will be reseted on each end of round")]
    private bool wasResetTriggers = false;

    private bool selectedMultiplayer = false;
    #endregion

    #endregion

    #region Loading Components

    public void Awake()
    {
        // When multiplayer to be done we need to look for the right components, the other components declared dont need to be found, just attached in Inspector

        cameraSystem = null;
        roundSystem = null;
        cooldownSystem = null;
        enemySystem = null;
        enemyBody = null;

        roundSystem = GameObject.Find("RoundManager").GetComponent<RoundManager>();
    }

    public void Start()
    {
        if (roundSystem.isMultiplayer == false)
        {
            playerCollider.enabled = true;
            RegisterInput();
        }

        if (roundSystem.isMultiplayer == true)
        {
            switch (roundSystem.currentEnemyCharacter)
            {
                case 1: enemyBody = GameObject.Find("GabriellaEnemy").GetComponent<Transform>(); enemySystemMultiplayer = GameObject.Find("GabriellaEnemy").GetComponent<OpponentMultiplayer>(); enemySystem = GameObject.Find("GabriellaEnemy").GetComponent<EnemySystem>(); break;
                case 2: enemyBody = GameObject.Find("MarcusEnemy").GetComponent<Transform>(); enemySystemMultiplayer = GameObject.Find("MarcusEnemy").GetComponent<OpponentMultiplayer>(); enemySystem = GameObject.Find("MarcusEnemy").GetComponent<EnemySystem>(); break;
                case 3: enemyBody = GameObject.Find("SelenaEnemy").GetComponent<Transform>(); enemySystemMultiplayer = GameObject.Find("SelenaEnemy").GetComponent<OpponentMultiplayer>(); enemySystem = GameObject.Find("SelenaEnemy").GetComponent<EnemySystem>(); break;
                case 4: enemyBody = GameObject.Find("BryanEnemy").GetComponent<Transform>(); enemySystemMultiplayer = GameObject.Find("BryanEnemy").GetComponent<OpponentMultiplayer>(); enemySystem = GameObject.Find("BryanEnemy").GetComponent<EnemySystem>(); break;
                case 5: enemyBody = GameObject.Find("NunEnemy").GetComponent<Transform>(); enemySystemMultiplayer = GameObject.Find("NunEnemy").GetComponent<OpponentMultiplayer>(); enemySystem = GameObject.Find("NunEnemy").GetComponent<EnemySystem>(); break;
                case 6: enemyBody = GameObject.Find("OliverEnemy").GetComponent<Transform>(); enemySystemMultiplayer = GameObject.Find("OliverEnemy").GetComponent<OpponentMultiplayer>(); enemySystem = GameObject.Find("OliverEnemy").GetComponent<EnemySystem>(); break;
                case 7: enemyBody = GameObject.Find("OrionEnemy").GetComponent<Transform>(); enemySystemMultiplayer = GameObject.Find("OrionEnemy").GetComponent<OpponentMultiplayer>(); enemySystem = GameObject.Find("OrionEnemy").GetComponent<EnemySystem>(); break;
                case 8: enemyBody = GameObject.Find("AriaEnemy").GetComponent<Transform>(); enemySystemMultiplayer = GameObject.Find("AriaEnemy").GetComponent<OpponentMultiplayer>(); enemySystem = GameObject.Find("AriaEnemy").GetComponent<EnemySystem>(); break;
            }
        }
        else
        {
            switch (roundSystem.currentEnemyCharacter)
            {
                case 1: enemyBody = GameObject.Find("GabriellaEnemy").GetComponent<Transform>(); enemySystem = GameObject.Find("GabriellaEnemy").GetComponent<EnemySystem>(); break;
                case 2: enemyBody = GameObject.Find("MarcusEnemy").GetComponent<Transform>(); enemySystem = GameObject.Find("MarcusEnemy").GetComponent<EnemySystem>(); break;
                case 3: enemyBody = GameObject.Find("SelenaEnemy").GetComponent<Transform>(); enemySystem = GameObject.Find("SelenaEnemy").GetComponent<EnemySystem>(); break;
                case 4: enemyBody = GameObject.Find("BryanEnemy").GetComponent<Transform>(); enemySystem = GameObject.Find("BryanEnemy").GetComponent<EnemySystem>(); break;
                case 5: enemyBody = GameObject.Find("NunEnemy").GetComponent<Transform>(); enemySystem = GameObject.Find("NunEnemy").GetComponent<EnemySystem>(); break;
                case 6: enemyBody = GameObject.Find("OliverEnemy").GetComponent<Transform>(); enemySystem = GameObject.Find("OliverEnemy").GetComponent<EnemySystem>(); break;
                case 7: enemyBody = GameObject.Find("OrionEnemy").GetComponent<Transform>(); enemySystem = GameObject.Find("OrionEnemy").GetComponent<EnemySystem>(); break;
                case 8: enemyBody = GameObject.Find("AriaEnemy").GetComponent<Transform>(); enemySystem = GameObject.Find("AriaEnemy").GetComponent<EnemySystem>(); break;
            }
        }
    }

    #endregion

    #region Real Time operations

    private void Update()
    {
        #region Animate Intro while round dont start

        if (roundSystem.currentRound == 1 && introAnimated == false)
        {
            introAnimated = true; // Prevents to execute animation call many times, this way we only call 1 time the correct animation
            StartIntroAnimation(); // Round 1 started so activate Intro Animation
        }

        if (roundSystem.currentRound == 2 && introAnimated == true)
        {
            introAnimated = false; // Prevents to execute animation call many times, this way we only call 1 time the correct animation
            Invoke(nameof(StartIntroAnimation), 5f); // We delay Intro animation to start to let last Defeat or Victory animation to run for some time before Intro animation starts again
        }

        if (roundSystem.currentRound == 3 && introAnimated == false)
        {
            introAnimated = true; // Prevents to execute animation call many times, this way we only call 1 time the correct animation
            Invoke(nameof(StartIntroAnimation), 5f); // We delay Intro animation to start to let last Defeat or Victory animation to run for some time before Intro animation starts again
        }

        #endregion

        #region Check if round finished

        if (roundSystem.roundOver == true && wasResetTriggers == false)
        {
            ResetAllTriggers();
        }

        #endregion

        #region Check if Player dealed damage to Enemy

        if (checkDamage == true)
        {
            damageTime = damageTime + Time.deltaTime;

            if (enemySystem.distanceToTarget <= attackRange && damageTime > 0f)
            {
                checkDamage = false;
                damageTime = 0f;

                if (roundSystem.isMultiplayer == false)
                {
                    enemySystem.TakeDamage(20);
                }
                else
                {
                    enemySystemMultiplayer.TakeDamage(20);
                }
            }

            if (damageTime > 0.2f)
            {
                checkDamage = false;
                damageTime = 0f;
            }
        }

        #endregion
    }

    private void FixedUpdate()
    {
        #region Movement Player because round started

        // Ensure movement only happens when buttons are held and not during attacks
        if (isAttacking == false && roundSystem.roundStarted == true && isHit == false && roundSystem.roundOver == false) // Check if round started so Player can move and if Player is not attacking
        {
            if (wasResetTriggers == true)
            {
                wasResetTriggers = false; // Prepare to use Reset Triggers again when the round to finish
            }

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

        #endregion

        #region Player got a hit so move Player a bit to backwards or enemy can hit Player forever

        if (isHit == true && roundSystem.roundOver == false)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - moveSpeed * Time.deltaTime * 3f);
        }

        #endregion
    }

    #endregion

    #region Camera Movement

    private void OnTriggerEnter(Collider other)
    {
        if (roundSystem.isMultiplayer == false)
        {
            if (other.name == "Left Collider")
            {
                cameraSystem.MoveToLeft();
            }

            if (other.name == "Right Collider")
            {
                cameraSystem.MoveToRight();
            }
        }
        else
        {
            if (other.name == "Left Collider" && selectedMultiplayer == true)
            {
                cameraSystem.MoveToLeft();
            }

            if (other.name == "Right Collider" && selectedMultiplayer == true)
            {
                cameraSystem.MoveToRight();
            }
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (roundSystem.isMultiplayer == false)
        {
            if (other.name == "Left Collider" || other.name == "Right Collider")
            {
                cameraSystem.StopToMove();
            }
        }
        else
        {
            if (other.name == "Left Collider" && selectedMultiplayer == true || other.name == "Right Collider" && selectedMultiplayer == true)
            {
                cameraSystem.StopToMove();
            }
        }
    }

    #endregion

    #region Stop movement and set idle animation

    public void RegisterInput()
    {
        if (roundSystem.isMultiplayer == true)
        {
            playerCollider.enabled = true;
            selectedMultiplayer = true;
        }

        cameraSystem = GameObject.Find("Camera").GetComponent<CameraSystem>();
        trainingSystem = GameObject.Find("RoundManager").GetComponent<TrainingSystem>();
        cooldownSystem = GameObject.Find("RoundManager").GetComponent<CooldownSystem>();

        initialPosition = transform.position;

        Debug.Log("Character " + gameObject.name + " was choice to start input events");

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

    private void AnimIsIdle() // StopMoving is being called alot, consuming processing and it is bad to mobile, so...
    {
        // Check if Player moved forward to StopMoving trigger the boolean change in the animation parameter

        if (playerAnimator.GetBool("isIdle") == false)
        {
            playerAnimator.SetBool("isIdle", true); // Prevents to execute animation call many times, this way we only call 1 time the correct animation
        }

        if (playerAnimator.GetBool("isForward") == true) // Check if Player moved forward to StopMoving trigger the boolean change in the animation parameter
        {
            //Debug.Log("Player stopped to move forward");

            playerAnimator.SetBool("isForward", false); // Values in parameters should be low case in the first letter because is variable name - 
        }

        if (playerAnimator.GetBool("isBackward") == true) // Check if Player moved backward to StopMoving trigger the boolean change in the animation parameter
        {
            //Debug.Log("Player stopped to move backward");

            playerAnimator.SetBool("isBackward", false); // Values in parameters should be low case in the first letter because is variable name - 
        }

        if (playerAnimator.GetBool("isHit") == true)
        {
            playerAnimator.SetBool("isHit", false); // Values in parameters should be low case in the first letter because is variable name - 
        }

        if (playerAnimator.GetBool("isAttack1") == true)
        {
            playerAnimator.SetBool("isAttack1", false); // Values in parameters should be low case in the first letter because is variable name - 
        }

        if (playerAnimator.GetBool("isAttack2") == true)
        {
            playerAnimator.SetBool("isAttack2", false); // Values in parameters should be low case in the first letter because is variable name - 
        }

        if (playerAnimator.GetBool("isAttack3") == true)
        {
            playerAnimator.SetBool("isAttack3", false); // Values in parameters should be low case in the first letter because is variable name - 
        }

        // If both triggers to be false, execute nothing, so it saves processing instead to apply false to both animation parameters in each frame, it only will trigger if is true and only once
    }

    #endregion

    #region Method to handle hit from Enemy

    public void TakeHit(int damageAmmount)
    {
        if (isHit == false)
        {
            //Debug.Log("Player got a hit and got " + damageAmmount + " of damage!");

            if (roundSystem.isTrainingMode == false)
            {
                roundSystem.ApplyDamageToPlayer(damageAmmount); // Inform RoundManager that Player got damage by Enemy
                                                                // roundSystem.audioSystem.PlayerDamage(roundSystem.currentPLayerCharacter); // Start character Damage sound in another Audio Source different from what Enemy will use to play his Damage sound, only after damage has applied, create a new Audio Source for it
            }

            if (roundSystem.playerHealthBar.slider.value <= 0)
            {
                if (roundSystem.enemyTotalCombo != 0)
                {
                    roundSystem.EnemyFinishedCombo(); // Reset hit count because opponent died
                }
            }
            else
            {
                if (roundSystem.enemyTotalCombo == 0)
                {
                    roundSystem.EnemyStartCombo();
                }

                if (roundSystem.enemyTotalCombo == 1)
                {
                    roundSystem.EnemyContinueCombo();
                }

                if (roundSystem.enemyTotalCombo == 2)
                {
                    roundSystem.EnemyContinueCombo();
                }

                if (roundSystem.enemyTotalCombo == 3)
                {
                    roundSystem.EnemyFinishedCombo();
                }

                AnimIsHit(); // Start Hit animation in Player
                hitEffect.SetActive(true); // Activate Hit Effect in the body of Player
                // roundSystem.audioSystem.PlayerEffect(roundSystem.currentPLayerCharacter); // Start character Effect sound in another Audio Source different from what Enemy will use to play his Effect sound, only after effect is enabled, create a new Audio Source for it
                Invoke(nameof(DisableEffect), 1f); // Deactivate Hit Effect after 1 second
            }

            isHit = true;
        }
    }

    #endregion

    #region Button press handlers

    public void OnMoveRightButtonPressed(BaseEventData eventData)
    {
        if (roundSystem.roundStarted == true && roundSystem.roundOver == false)
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
        if (roundSystem.roundStarted == true && roundSystem.roundOver == false)
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
        if (playerAnimator.GetBool("isForward") == false) // Check if MoveForward is false to trigger it only 1 time and to save processing this way - 
        {
            //Debug.Log("Player moved to right");

            moveDirection = 1; // Setup new direction only once before to apply new position - 
            playerAnimator.SetBool("isForward", true); // Values in parameters should be low case in the first letter because is variable name - 
            playerAnimator.SetBool("isIdle", false); // Values in parameters should be low case in the first letter because is variable name - 
            playerAnimator.SetBool("isBackward", false); // Values in parameters should be low case in the first letter because is variable name - 
            playerAnimator.SetBool("isHit", false); // Values in parameters should be low case in the first letter because is variable name - 
            playerAnimator.SetBool("isAttack1", false); // Values in parameters should be low case in the first letter because is variable name - 
            playerAnimator.SetBool("isAttack2", false); // Values in parameters should be low case in the first letter because is variable name - 
            playerAnimator.SetBool("isAttack3", false); // Values in parameters should be low case in the first letter because is variable name - 
            //roundSystem.audioSystem.MoveRight(1, roundSystem.currentPlayerCharacter); // Start character Move Right sound in Player Audio only after animation has started - Optional
            if (trainingSystem.actualInfoIndex == 1 && completedTutorial == false) { trainingSystem.SelectInfo(); completedTutorial = true; }
        }
    }

    #endregion

    #region Method to start moving left
    
    private void MoveLeft() // MoveLeft is being called alot, consuming processing and it is bad to mobile, so...
    {
        if (playerAnimator.GetBool("isBackward") == false) // Check if MoveBackwards is false to trigger it only 1 time and to save processing this way - 
        {
            //Debug.Log("Player moved to left");

            moveDirection = -1; // Setup new direction only once before to apply new position - 
            playerAnimator.SetBool("isBackward", true); // Values in parameters should be low case in the first letter because is variable name - 
            playerAnimator.SetBool("isIdle", false); // Values in parameters should be low case in the first letter because is variable name - 
            playerAnimator.SetBool("isForward", false); // Values in parameters should be low case in the first letter because is variable name - 
            playerAnimator.SetBool("isHit", false); // Values in parameters should be low case in the first letter because is variable name - 
            playerAnimator.SetBool("isAttack1", false); // Values in parameters should be low case in the first letter because is variable name - 
            playerAnimator.SetBool("isAttack2", false); // Values in parameters should be low case in the first letter because is variable name - 
            playerAnimator.SetBool("isAttack3", false); // Values in parameters should be low case in the first letter because is variable name - 
            //roundSystem.audioSystem.MoveLeft(1, roundSystem.currentPlayerCharacter); // Start character Move Left sound in Player Audio only after animation has started - Optional
            if (trainingSystem.actualInfoIndex == 2 && completedTutorial == false) { trainingSystem.SelectInfo(); completedTutorial = true; }
        }
    }

    #endregion

    #region Method when player get hit

    private void AnimIsHit()
    {
        if (playerAnimator.GetBool("isHit") == false)
        {
            //Debug.Log("Hit Animation was activated");

            playerAnimator.SetBool("isHit", true); // Trigger isHit animation - Values in parameters should be low case in the first letter because is variable name - 
            playerAnimator.SetBool("isIdle", false); // Values in parameters should be low case in the first letter because is variable name - 
            playerAnimator.SetBool("isForward", false); // Values in parameters should be low case in the first letter because is variable name - 
            playerAnimator.SetBool("isBackward", false); // Values in parameters should be low case in the first letter because is variable name - 
            playerAnimator.SetBool("isAttack1", false); // Values in parameters should be low case in the first letter because is variable name - 
            playerAnimator.SetBool("isAttack2", false); // Values in parameters should be low case in the first letter because is variable name - 
            playerAnimator.SetBool("isAttack3", false); // Values in parameters should be low case in the first letter because is variable name - 
            //roundSystem.audioSystem.Hit(1, roundSystem.currentPlayerCharacter); // Start character Hit sound in Player Audio only after animation has started
            if (trainingSystem.actualInfoIndex == 6 && completedTutorial == false) { trainingSystem.SelectInfo(); completedTutorial = true; }
            Invoke(nameof(CheckHitStuck), 1f);
        }
    }

    #endregion

    #region Method when player using attacks

    private void AnimIsAttack1()
    {
        //Debug.Log("Attack 1 button was activated");

        if (isCooldown1 == false) // Check if skill is in cooldown
        {
            if (playerAnimator.GetBool("isAttack1") == false) // Prevents to execute animation call many times, this way we only call 1 time the correct animation
            {
                //Debug.Log("Attack 1 Animation was activated");

                cooldownSystem.ActivateCooldown1(); // Skill not in cooldown so lets activate cooldown
                playerAnimator.SetBool("isAttack1", true); // Prevents to execute animation call many times, this way we only call 1 time the correct animation
                playerAnimator.SetBool("isIdle", false); // Values in parameters should be low case in the first letter because is variable name - 
                playerAnimator.SetBool("isForward", false); // Values in parameters should be low case in the first letter because is variable name - 
                playerAnimator.SetBool("isBackward", false); // Values in parameters should be low case in the first letter because is variable name - 
                playerAnimator.SetBool("isHit", false); // Trigger isHit animation - Values in parameters should be low case in the first letter because is variable name - 
                playerAnimator.SetBool("isAttack2", false); // Values in parameters should be low case in the first letter because is variable name - 
                playerAnimator.SetBool("isAttack3", false); // Values in parameters should be low case in the first letter because is variable name - 
                isCooldown1= true; // Skill in cooldown mode, disable button action till the end of cooldown effect
                isAttacking = true; // We make sure only to trigger isAttacking after animation started
                //roundSystem.audioSystem.Attack1(1, roundSystem.currentPlayerCharacter); // Start character Attack 1 sound in Player Audio only after animation has started
                if (trainingSystem.actualInfoIndex == 3 && completedTutorial == false) { trainingSystem.SelectInfo(); completedTutorial = true; }
                Invoke(nameof(CheckAttack1Stuck), 1f);
            }
        }
    }

    private void AnimIsAttack2()
    {
        //Debug.Log("Attack 2 button was activated");

        if (isCooldown2 == false) // Check if skill is in cooldown
        {
            if (playerAnimator.GetBool("isAttack2") == false) // Prevents to execute animation call many times, this way we only call 1 time the correct animation
            {
                //Debug.Log("Attack 2 Animation was activated");

                cooldownSystem.ActivateCooldown2(); // Skill not in cooldown so lets activate cooldown
                playerAnimator.SetBool("isAttack2", true); // Values in parameters should be low case in the first letter because is variable name - 
                playerAnimator.SetBool("isIdle", false); // Values in parameters should be low case in the first letter because is variable name - 
                playerAnimator.SetBool("isForward", false); // Values in parameters should be low case in the first letter because is variable name - 
                playerAnimator.SetBool("isBackward", false); // Values in parameters should be low case in the first letter because is variable name - 
                playerAnimator.SetBool("isHit", false); // Trigger isHit animation - Values in parameters should be low case in the first letter because is variable name - 
                playerAnimator.SetBool("isAttack1", false); // Values in parameters should be low case in the first letter because is variable name - 
                playerAnimator.SetBool("isAttack3", false); // Values in parameters should be low case in the first letter because is variable name - 
                isCooldown2 = true; // Skill in cooldown mode, disable button action till the end of cooldown effect
                isAttacking = true; // We make sure only to trigger isAttacking after animation started
                //roundSystem.audioSystem.Attack2(1, roundSystem.currentPlayerCharacter); // Start character Attack 2 sound in Player Audio only after animation has started
                if (trainingSystem.actualInfoIndex == 4 && completedTutorial == false) { trainingSystem.SelectInfo(); completedTutorial = true; }
                Invoke(nameof(CheckAttack2Stuck), 1f);
            }
        }
    }

    private void AnimIsAttack3()
    {
        //Debug.Log("Attack 3 button was activated");

        if (isCooldown3 == false) // Check if skill is in cooldown
        {
            if (playerAnimator.GetBool("isAttack3") == false) // Prevents to execute animation call many times, this way we only call 1 time the correct animation
            {
                //Debug.Log("Attack 3 Animation was activated");

                cooldownSystem.ActivateCooldown3(); // Skill not in cooldown so lets activate cooldown
                playerAnimator.SetBool("isAttack3", true); // Values in parameters should be low case in the first letter because is variable name - 
                playerAnimator.SetBool("isIdle", false); // Values in parameters should be low case in the first letter because is variable name - 
                playerAnimator.SetBool("isForward", false); // Values in parameters should be low case in the first letter because is variable name - 
                playerAnimator.SetBool("isBackward", false); // Values in parameters should be low case in the first letter because is variable name - 
                playerAnimator.SetBool("isHit", false); // Trigger isHit animation - Values in parameters should be low case in the first letter because is variable name - 
                playerAnimator.SetBool("isAttack1", false); // Values in parameters should be low case in the first letter because is variable name - 
                playerAnimator.SetBool("isAttack2", false); // Values in parameters should be low case in the first letter because is variable name - 
                isCooldown3 = true; // Skill in cooldown mode, disable button action till the end of cooldown effect
                isAttacking = true; // We make sure only to trigger isAttacking after animation started
                //roundSystem.audioSystem.Attack3(1, roundSystem.currentPlayerCharacter); // Start character Attack 3 sound in Player Audio only after animation has started
                if (trainingSystem.actualInfoIndex == 5 && completedTutorial == false) { trainingSystem.SelectInfo(); completedTutorial = true; }
                Invoke(nameof(CheckAttack3Stuck), 1f);
            }
        }
    }

    private void CheckAttack1Stuck()
    {
        if (isAttacking == true) // Sometimes Animator dont triggers correctly in the end of the frame so we make sure it will be triggered
        {
            //Debug.Log("Attack 1 Stuck was checked");
            checkDamage = true; // Attack animation finished so check if Player deals damage in Enemy
            isAttacking = false; // Attack animation finished
            AnimIsIdle(); // Reset animation to Idle
        }
    }

    private void CheckAttack2Stuck()
    {
        if (isAttacking == true) // Sometimes Animator dont triggers correctly in the end of the frame so we make sure it will be triggered
        {
            //Debug.Log("Attack 2 Stuck was checked");
            checkDamage = true; // Attack animation finished so check if Player deals damage in Enemy
            isAttacking = false; // Attack animation finished
            AnimIsIdle(); // Reset animation to Idle
        }
    }

    private void CheckAttack3Stuck()
    {
        if (isAttacking == true) // Sometimes Animator dont triggers correctly in the end of the frame so we make sure it will be triggered
        {
            //Debug.Log("Attack 3 Stuck was checked");
            checkDamage = true; // Attack animation finished so check if Player deals damage in Enemy
            isAttacking = false; // Attack animation finished
            AnimIsIdle(); // Reset animation to Idle
        }
    }

    public void CheckHitStuck() // It shows zero references but is activated by the last frame of Hit animation
    {
        if (isHit == true)
        {
            //Debug.Log("Hit Stuck was checked");

            isHit = false; // Hit animation finished
            AnimIsIdle(); // Reset animation to Idle
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
        if (isAttacking == false && roundSystem.roundStarted == true && isHit == false && roundSystem.roundOver == false)
        {
            //Debug.Log("Player activated Attack 1");
            // roundSystem.audioSystem.PlayButtonSound(1, roundSyste.currentPlayerCharacter); // Optional
            AnimIsAttack1();
        }
    }

    public void OnAttack2ButtonPressed()
    {
        if (isAttacking == false && roundSystem.roundStarted == true && isHit == false && roundSystem.roundOver == false)
        {
            //Debug.Log("Player activated Attack 2");
            // roundSystem.audioSystem.PlayButtonSound(1, roundSyste.currentPlayerCharacter); // Optional
            AnimIsAttack2();
        }
    }

    public void OnAttack3ButtonPressed()
    {
        if (isAttacking == false && roundSystem.roundStarted == true && isHit == false && roundSystem.roundOver == false)
        {
            //Debug.Log("Player activated Attack 3");
            // roundSystem.audioSystem.PlayButtonSound(1, roundSyste.currentPlayerCharacter); // Optional
            AnimIsAttack3();
        }
    }

    #endregion

    #region Called when animation frames reach last frame

    public void LastFrameHit() // It shows zero references but is activated by the last frame of Hit animation
    {
        isHit = false; // Hit animation finished
        AnimIsIdle(); // Reset animation to Idle
    }

    public void LastFrameAttack() // It shows zero references but is activated by the last frame of any Attack animation
    {
        if (checkDamage == false)
        {
            checkDamage = true; // Attack animation finished so check if Player deals damage in Enemy
        }

        if (isAttacking == true)
        {
            isAttacking = false; // Attack animation finished so let PLayer to be free to do another attack
        }

        AnimIsIdle(); // Reset animation to Idle
    }

    #endregion

    #region Round Operations

    private void ResetAllAnimations()
    {
        playerAnimator.SetBool("isIdle", false); // Values in parameters should be low case in the first letter because is variable name - 
        playerAnimator.SetBool("isForward", false); // Values in parameters should be low case in the first letter because is variable name - 
        playerAnimator.SetBool("isBackward", false); // Values in parameters should be low case in the first letter because is variable name - 
        playerAnimator.SetBool("isHit", false); // Values in parameters should be low case in the first letter because is variable name - 
        playerAnimator.SetBool("isAttack1", false); // Values in parameters should be low case in the first letter because is variable name - 
        playerAnimator.SetBool("isAttack2", false); // Values in parameters should be low case in the first letter because is variable name - 
        playerAnimator.SetBool("isAttack3", false); // Values in parameters should be low case in the first letter because is variable name - 
        playerAnimator.SetBool("isDead", false); // Values in parameters should be low case in the first letter because is variable name - 
        playerAnimator.SetBool("isIntro", false); // Values in parameters should be low case in the first letter because is variable name - 
    }

    private void ResetAllTriggers()
    {
        isAttacking = false;
        isMovingBackward = false;
        isMovingBackward = false;
        isHit = false;
        checkDamage = false;
        isCooldown1 = false;
        isCooldown2 = false;
        isCooldown3 = false;
        cooldownSystem.ResetAllCooldowns();
        wasResetTriggers = true;
    }

    private void StartIntroAnimation()
    {
        //Debug.Log("Player started Intro anim");

        if (roundSystem.currentRound != 1)
        {
            cameraSystem.ResetCamera();
            gameObject.transform.position = initialPosition; // Move Player to start position because a new round started
        }

        playerAnimator.Play("isIntro"); // Play Intro animation because a new round started
        roundSystem.audioSystem.PlayIntro(1, roundSystem.currentPlayerCharacter); // Start character Intro sound in Player Audio only after animation has started
    }

    public void StartVictoryAnimation()
    {
        ResetAllAnimations(); // Prevents to execute animation call many times, this way we only call 1 time the correct animation

        //Debug.Log("Player Victory was activated");
        playerAnimator.Play("isVictory");
        //roundSystem.audioSystem.PlayVictory(1, roundSystem.currentPlayerCharacter); // Start character Victory sound in Player Audio only after animation has started
    }

    public void StartDrawAnimation()
    {
        ResetAllAnimations(); // Prevents to execute animation call many times, this way we only call 1 time the correct animation

        //Debug.Log("Player Draw was activated");
        playerAnimator.Play("isDefeat");
        //roundSystem.audioSystem.PlayDraw(1, roundSystem.currentPlayerCharacter); // Start character Draw sound in Player Audio only after animation has started
    }

    public void StartDefeatAnimation()
    {
        ResetAllAnimations(); // Prevents to execute animation call many times, this way we only call 1 time the correct animation

        //Debug.Log("Player Defeat was activated");
        playerAnimator.Play("isDefeat");
        //roundSystem.audioSystem.PlayDefeat(1, roundSystem.currentPlayerCharacter); // Start character Defeat sound in Player Audio only after animation has started
    }

    #endregion

    #region Effect Operations

    private void DisableEffect()
    {
        hitEffect.SetActive(false);
    }

    public void Cooldown1Finished()
    {
        isCooldown1 = false;
    }

    public void Cooldown2Finished()
    {
        isCooldown2 = false;
    }

    public void Cooldown3Finished()
    {
        isCooldown3 = false;
    }

    #endregion

    #region Multiplayer Operations

    public void DisableSingle()
    {
        this.enabled = false;
    }

    #endregion
}
