using UnityEngine;

public class EnemySystem : MonoBehaviour
{
    #region Variables

    [Header("Animator Setup")]
    [Tooltip("Attach current enemy Animator component here")]
    public Animator enemyAnimator;

    [Header("Hit Effect Setup")]
    [Tooltip("Attach current enemy HitEffect GameObject here")]
    public GameObject hitEffect;

    [Header("Enemy Setup")]
    [Tooltip("Actual enemy attack AI or multiplayer selected - It should be public because in multiplayer we will allow Player to read this value")]
    public int actualAttack = 1;
    [Tooltip("Setup actual enemy difficulty level - Current levels you can choice: 0 = easy 1 = moderate 2 = normal 3 = hard")]
    public int enemyDifficulty = 0;
    [Tooltip("Setup actual enemy attack range to hit player")]
    public float attackRange = 13f;
    [Tooltip("Setup actual enemy movement speed")]
    public float moveSpeed = 2f;
    [Tooltip("Should be public to player read the value on it - Dont change it")]
    public float distanceToTarget = 0f;

    #region Hidden Variables

    [Header("Monitor")] // Turn variables into public to show monitor
    [Tooltip("Actual Round System from RoundManager object in the current scene, it will be loaded when scene to awake")]
    private RoundManager roundSystem;
    [Tooltip("Actual Player System from selected player in singleplayer or multiplayer, it will be loaded when scene to awake")]
    private PlayerSystem playerSystem;
    [Tooltip("Actual Player Transform from selected player in singleplayer or multiplayer, it will be loaded when scene to awake")]
    private Transform playerBody;
    [Tooltip("Initial position from Enemy to use it when a new round to start to move Enemy to initial position")]
    private Vector3 initialPosition;
    [Tooltip("It should be always 100 - Dont change it!")]
    private readonly int randomMaxValue = 100;
    [Tooltip("Actual enemy time while randomizing! It should be always zero! - Dont change it!")]
    private float randomizeTimer = 0f;
    [Tooltip("Move Random determines if IA will move, stop or reverse actual movemente")]
    private int moveRandom = 0;
    [Tooltip("Attack Random determines if IA will attack, stop or to select different skills")]
    private int attackRandom = 0;
    [Tooltip("Skill Random determines wich skill will AI to use to attack")]
    private int skillRandom = 0;
    [Tooltip("Attack Cooldown 1 determines the cooldown when Attack 1 was used")]
    private float attackCooldown1 = 3f;
    [Tooltip("Attack Cooldown 2 determines the cooldown when Attack 2 was used")]
    private float attackCooldown2 = 9f;
    [Tooltip("Attack Cooldown 3 determines the cooldown when Attack 3 was used")]
    private float attackCooldown3 = 15f;
    [Tooltip("Check if Enemy applied damage in a certain ammount of time, if Player not to be inside range when time is over so Enemy dont dealed damage to Player")]
    private float damageTime = 0f;
    [Tooltip("Enabled when rounds starts, it will be triggered automatically")]
    private bool introAnimated = false;
    [Tooltip("Move Success Random determines if AI decided to change movement when enabled and the action is based in enemy difficulty level")]
    private bool moveSuccessRandom = false;
    [Tooltip("Attack Success Random determines if AI decided to change attack action when enabled and the action is based in enemy difficulty level")]
    private bool attackSuccessRandom = false;
    [Tooltip("Reset Random will reset actual random values when enabled")]
    private bool isResetRandom = false;
    [Tooltip("Enemy can move when enabled")]
    private bool isWalking = false;
    [Tooltip("Enemy is attacking when enabled, it is a behaviour")]
    private bool isAttacking = false;
    [Tooltip("IsCooldown1 determines that Attack 1 is on cooldown when enabled")]
    private bool isCooldown1 = false;
    [Tooltip("IsCooldown2 determines that Attack 2 is on cooldown when enabled")]
    private bool isCooldown2 = false;
    [Tooltip("IsCooldown3 determines that Attack 3 is on cooldown when enabled")]
    private bool isCooldown3 = false;
    [Tooltip("Enemy is being hit when enabled")]
    private bool isHit = false;
    [Tooltip("Enemy can attack when enabled")]
    private bool canFight = false;
    [Tooltip("Enemy can deals damage to player when enabled")]
    private bool checkDamage = false;
    [Tooltip("Enemy can make a decision when enabled")]
    private bool canRandomize = false;
    [Tooltip("Enemy is moving forward when enabled")]
    private bool changedAnimDirectionToForward = false;
    [Tooltip("Enemy is moving backward when enabled")]
    private bool changedAnimDirectionToBackward = false;
    [Tooltip("If enabled means player triggers will be reseted on each end of round")]
    private bool wasResetTriggers = false;

    #endregion

    #endregion

    #region Loading Components

    private void Awake()
    {
        // When multiplayer to be done we need to look for the right components, the other components declared dont need to be found, just attached in Inspector

        playerBody = null;
        playerSystem = null;

        roundSystem = GameObject.Find("RoundManager").GetComponent<RoundManager>();
        initialPosition = gameObject.transform.position; // Store the initial position to use it when a new round to start, so can move Enemy always to initial position
    }

    #endregion

    #region Setup Loaded Components

    private void Start()
    {
        switch (roundSystem.currentPlayerCharacter)
        {
            case 1: playerBody = GameObject.Find("GabriellaPlayer").GetComponent<Transform>(); break;
            case 2: playerBody = GameObject.Find("MarcusPlayer").GetComponent<Transform>(); break;
            case 3: playerBody = GameObject.Find("SelenaPlayer").GetComponent<Transform>(); break;
            case 4: playerBody = GameObject.Find("BryanPlayer").GetComponent<Transform>(); break;
            case 5: playerBody = GameObject.Find("NunPlayer").GetComponent<Transform>(); break;
            case 6: playerBody = GameObject.Find("OliverPlayer").GetComponent<Transform>(); break;
            case 7: playerBody = GameObject.Find("OrionPlayer").GetComponent<Transform>(); break;
            case 8: playerBody = GameObject.Find("AriaPlayer").GetComponent<Transform>(); break;
        }

        playerSystem = roundSystem.playerSystem;
        distanceToTarget = Vector3.Distance(transform.position, playerBody.position); // Get initial position from Player to get the first distance measure only once
    }

    #endregion

    #region Real Time Operations

    private void Update()
    {
        #region Animate Intro while round dont start

        if (roundSystem.currentRound == 1 && introAnimated == false)
        {
            introAnimated = true; // Prevents to execute animation call many times, this way we only call 1 time the correct animation
            StartIntroAnimation(); // Round 1 started so activate Intro animation
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

        #region Checking if round started

        if (canFight == false && roundSystem.roundStarted == true && moveSuccessRandom == false && roundSystem.roundOver == false) // Only can execute commands in FixedUpdate if round started, but...
        {
            //Debug.Log("Round Started");

            canRandomize = true; // Round started, so activate randomizer
            canFight = true; // Round started, so can fight
        }

        #endregion

        #region Checking if round finished

        if (roundSystem.roundOver == true && wasResetTriggers == false) // If round not started and is 2nd or 3rd round, load Idle animation till round start again! - Felipe
        {
            ResetAllTriggers();
        }

        #endregion

        #region Randomize Movement

        if (canRandomize == true && isAttacking == false) // Begin to randomize to AI to make decisions so fighting against IA will not to be linear and predictible
        {
            randomizeTimer = randomizeTimer + Time.deltaTime; // Count the time to start to randomize

            if (randomizeTimer >= 3f) // I was setup 3 seconds, but you can change this value if you want
            {
                moveRandom = Random.Range(1, randomMaxValue); // Randomizing a new value

                switch (enemyDifficulty) // Check sucess in the random number generated
                {
                    case 0: // Less agressive and less defensive

                        if (moveRandom <= 1 && moveRandom <= 80) // 80% chance to AI not to change behaviour
                        {
                            moveSuccessRandom = false; // Continue to do what is doing
                        }

                        if (moveRandom >= 81 &&  moveRandom <= 100) // 20% chance to AI to change behaviour
                        {
                            moveSuccessRandom = true; // No agression and no defense - Call for more Idle and stop to move in the middle of the combate and dont attack
                        }

                        break;
                    
                    case 1: // less agressive and more defensive

                        if (moveRandom <= 1 && moveRandom <= 40) // 40% chance to AI not to change behaviour
                        {
                            moveSuccessRandom = false; // Continue to do what is doing
                        }

                        if (moveRandom >= 41 && moveRandom <= 100) // 60% chance to AI to change behaviour
                        {
                            moveSuccessRandom = true; // Move more far from player or use defensive skills if possible
                        }

                        break;
                    
                    case 2: // More agressive and less defensive

                        if (moveRandom <= 1 && moveRandom <= 50) // 50% chance to AI not to change behaviour
                        {
                            moveSuccessRandom = false; // Continue to do what is doing
                        }

                        if (moveRandom >= 51 && moveRandom <= 100) // 50% chance to AI to change behaviour
                        {
                            moveSuccessRandom = true; // Move more near from player or use aggressive skills if possible
                        }

                        break;
                    
                    case 3: // More agressive and more defensive

                        if (moveRandom <= 1 && moveRandom <= 20) // 20% chance to AI not to change behaviour
                        {
                            moveSuccessRandom = false; // Continue to do what is doing
                        }

                        if (moveRandom >= 21 && moveRandom <= 100) // 80% chance to AI to change behaviour
                        {
                            moveSuccessRandom = true; // Move more near from player and use aggressive skills if player is far or move more far from player or use defensive skills if possible
                        }

                        break;
                }

                if (isWalking == true && moveSuccessRandom == true) // Apply the success in random number based in the level difficulty
                {
                    switch (enemyDifficulty)
                    {
                        case 0: // Stop to move AI and let it more vulnerable to attacks

                            if (isAttacking == false)
                            {
                                canFight = false;
                                isWalking = false;
                                changedAnimDirectionToBackward = false;
                                changedAnimDirectionToForward = false;
                                EnemyIsIdle();
                            }

                            break;

                        case 1: // Follow less the player and use more defensife skills
                            
                            if (changedAnimDirectionToForward == true && isAttacking == false)
                            {
                                changedAnimDirectionToForward = false;
                                changedAnimDirectionToBackward = true;
                            }

                            break;

                        case 2: // Follow more the player and use more aggressive skills

                            if (changedAnimDirectionToBackward == true && isAttacking == false)
                            {
                                changedAnimDirectionToForward = true;
                                changedAnimDirectionToBackward = false;
                            }

                            break;

                        case 3: // Follow player if far and move far if near player

                            if (isAttacking == false)
                            {
                                if (distanceToTarget > attackRange)
                                {
                                    changedAnimDirectionToForward = true;
                                    changedAnimDirectionToBackward = false;
                                }

                                if (distanceToTarget <= attackRange)
                                {
                                    changedAnimDirectionToForward = false;
                                    changedAnimDirectionToBackward = true;
                                }
                            }

                            break;
                    }
                }

                isResetRandom = true; // After to get the random value, to check AI difficulty, time to reset timer of the randomizer to generate a new random value
                randomizeTimer = 0f; // Zero the randomizer time to use it to reset randomizer system
                canRandomize = false; // We got the values we was wish, time to close the randomize thread
            }
        }

        #endregion

        #region Reset Randomizer

        if (isResetRandom == true) // Beginning to reset randomizer timer
        {
            randomizeTimer = randomizeTimer + Time.deltaTime; // Using randomizer timer to reset now

            if (randomizeTimer > 2f) // I was put 2 seconds, but you can change this value to a value you to wish
            {
                randomizeTimer = 0f; // Randomizer time was reseted

                if (attackRandom != 0) // Only reset if AttackRandom was being used
                {
                    attackRandom = 0; // Randomize attack again if AI will attack or not
                }

                if (canFight == false) // Check if AI is not fighting...
                {
                    canFight = true; // AI can fight now
                }

                if (moveSuccessRandom == true) // Check if AI got succes on the closed thead, reset it to generate new value...
                {
                    moveSuccessRandom = false; // Disable last success in random to make random success avaiable again
                }

                isResetRandom = false; // We already was reset randomizer system

                if (canRandomize == false) // Only activate if CanRandomize was being used
                {
                    canRandomize = true; // Randomize AI actions again!!!
                }
            }
        }

        #endregion

        #region Check if Enemy dealed damage to player

        if (checkDamage == true)
        {
            damageTime = damageTime + Time.deltaTime;

            if (distanceToTarget <= attackRange && damageTime > 0f)
            {
                checkDamage = false;
                damageTime = 0f;
                playerSystem.TakeHit(20);
            }

            if (damageTime > 0.2f)
            {
                checkDamage = false;
                damageTime = 0f;
            }
        }

        #endregion

        #region Check actual cooldown in skills

        if (isCooldown1 == true) // If Attack 1 cooldown is activated...
        {
            attackCooldown1 = attackCooldown1 - Time.deltaTime; // Count cooldwon time

            if (attackCooldown1 < 0f) // If cooldown time finished...
            {
                attackCooldown1 = 3f; // Reset cooldown time
                isCooldown1 = false; // Disable cooldown event so AI can use the skill again
            }
        }

        if (isCooldown2 == true) // If Attack 2 cooldown is activated...
        {
            attackCooldown2 = attackCooldown2 - Time.deltaTime; // Count cooldwon time

            if (attackCooldown2 < 0f) // If cooldown time finished...
            {
                attackCooldown2 = 9f; // Reset cooldown time
                isCooldown2 = false; // Disable cooldown event so AI can use the skill again
            }
        }

        if (isCooldown3 == true) // If Attack 3 cooldown is activated...
        {
            attackCooldown3 = attackCooldown3 - Time.deltaTime; // Count cooldwon time

            if (attackCooldown3< 0f) // If cooldown time finished...
            {
                attackCooldown3 = 15f; // Reset cooldown time
                isCooldown3 = false; // Disable cooldown event so AI can use the skill again
            }
        }

        #endregion
    }

    private void FixedUpdate()
    {
        #region Check if enemy is not alive anymore

        if (roundSystem.opponentHealthBar.slider.value <= 0) return; // Prevent further actions if Enemy is dead

        #endregion

        #region Check if enemy is still alive and perform actions if alive

        if (canFight == true && roundSystem.roundOver == false) // Prevent further actions if round not started yet
        {
            if (wasResetTriggers == true)
            {
                wasResetTriggers = false; // Prepare to use Reset Triggers again when the round to finish
            }

            distanceToTarget = Vector3.Distance(transform.position, playerBody.position);

            //Debug.Log("Actual distance to target from Enemy is: " + distanceToTarget); // Debug actual distance between Enemy and Player

            #region Check distance to player and make decisions

            #region Decide what to do when player is inside attack range

            if (distanceToTarget < attackRange && distanceToTarget > attackRange - 1f && isHit == false)
            {
                #region Randomize Attack

                if (isResetRandom == false && isAttacking == false)
                {
                    // Call for attack action randomize, AI can decide if attack or not when player is inside attack area

                    attackRandom = Random.Range(1, 100);

                    if (enemyDifficulty == 0 && attackRandom >= 81 && attackRandom <= 100) // 20% the easy AI have to attack
                    {
                        attackSuccessRandom = true;
                    }

                    if (enemyDifficulty == 1 && attackRandom >= 61 && attackRandom <= 100) // 40% the moderate AI have to attack
                    {
                        attackSuccessRandom = true;
                    }

                    if (enemyDifficulty == 2 && attackRandom >= 41 && attackRandom <= 100) // 60% the normal AI have to attack
                    {
                        attackSuccessRandom = true;
                    }

                    if (enemyDifficulty == 3 && attackRandom >= 21 && attackRandom <= 100) // 80% the hard AI have to attack
                    {
                        attackSuccessRandom = true;
                    }

                    if (attackSuccessRandom == false) // Check if enemy decided to attack player
                    {
                        EnemyIsIdle(); // Call for Idle because enemy is to much near player
                    }
                    else
                    {
                        RandomizeSkill(); // Enemy can attack player now and will select wich skill will use
                    }

                    isResetRandom = true; // Close the attack random call and reset it
                }

                #endregion

                // Attack area is determined by Attack Range and Attack Range -1, so if Attack Range is 8, the area will between 8f and 7f in the distance value, if player is 6.9f or less the AI will move backward
            }

            #endregion

            #region Decide to do when player is outside attack range

            if (distanceToTarget > attackRange && isAttacking == false && isHit == false)
            {
                if (checkDamage == true) // Check if player got out from attack area when damage is trying to be applied
                {
                    checkDamage = false; // Player is outside attack area so dont apply damage to player, only apply damage if player is really inside attack area
                }

                FixWalkAnimDirectionToForward();
                EnemyCanMove(); // Follow player if is outside attack area to attack the player
            }

            #endregion

            #region Decide what to do when player is beyound attack range

            if (distanceToTarget < attackRange && distanceToTarget < attackRange - 1f && isAttacking == false && isHit == false)
            {
                FixWalkAnimDirectionToBackward(); // Move opponent to backward if player to much near or if it is taking damage
                EnemyCanMove(); // Get far from player if is to much inside attack area to not let AI vulnerable for attacks
            }

            #endregion

            #endregion
        }

        #endregion
    }

    #endregion

    #region Movement Operations

    private void EnemyCanMove()
    {
        #region Check if enemy is being hit, if is being hit dont trigger move animation

        if (isHit == false)
        {
            StartWalkAnimation();
        }

        #endregion

        #region Apply movement to enemy

        if (isWalking == true && enemyDifficulty > 0 || // Only AI difficulty zero stops to move, so check if is AI difficulty level zero
            isWalking == true && enemyDifficulty == 0 && moveSuccessRandom == false)  // If is difficulty level zero, we make sure to only apply movement if success was false, because if true AI should stop to move
        {
            #region Move enemy forward

            if (changedAnimDirectionToForward == true || moveSuccessRandom == true && enemyDifficulty == 2 || moveSuccessRandom == true && enemyDifficulty == 3)
            {
                // Check if normal and hard enemy difficulty got success to move forward

                transform.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - moveSpeed * Time.deltaTime);
            }

            #endregion

            #region Move enemy backward

            if (changedAnimDirectionToBackward == true || moveSuccessRandom == true && enemyDifficulty == 1 || moveSuccessRandom == true && enemyDifficulty == 3 || isHit == true)
            {
                // Check if moderate and hard enemy difficulty got success to move forward

                transform.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + moveSpeed * Time.deltaTime);
            }

            #endregion
        }

        #endregion
    }

    private void FixWalkAnimDirectionToForward()
    {
        if (changedAnimDirectionToForward == false) // Reverse Backward animation to make it Forward animation
        {
            changedAnimDirectionToForward = true;
            changedAnimDirectionToBackward = false;
        }
    }

    private void FixWalkAnimDirectionToBackward()
    {
        if (changedAnimDirectionToBackward == false) // Reverse Backward animation to make it Forward animation
        {
            changedAnimDirectionToForward = false;
            changedAnimDirectionToBackward = true;
        }
    }

    #endregion

    #region Called when Enemy takes damage

    public void TakeDamage(int damage)
    {
        if (isHit == false) // With this trigger we make sure opponent only will take damage 1 time
        {
            roundSystem.ApplyDamageToOpponent(damage); // Inform RoundManager that Enemy tooks damage by player
            // roundSystem.audioSystem.EnemyDamage(roundSystem.currentEnemyCharacter); // Start character Damage sound in another Audio Source different from what Player will use to play his Damage sound, only after damage has applied, create a new Audio Source for it

            if (roundSystem.opponentHealthBar.slider.value <= 0)
            {
                if (roundSystem.playerTotalCombo != 0)
                {
                    roundSystem.PlayerFinishedCombo(); // Reset hit count because opponent died
                }
            }
            else
            {
                if (roundSystem.playerTotalCombo == 0)
                {
                    roundSystem.PlayerStartCombo();
                }

                if (roundSystem.playerTotalCombo == 1)
                {
                    StartBlockAnimation(); // Play Block animation on first hit
                    roundSystem.PlayerContinueCombo();
                }
                
                if (roundSystem.playerTotalCombo == 2)
                {
                    StartBlockAnimation(); // Just for debug will be removed later
                    //StartStunnedAnimation(); // Play Stunned animation on third hit
                    roundSystem.PlayerContinueCombo();
                }
                
                if (roundSystem.playerTotalCombo == 3)
                {
                    StartBlockAnimation(); // Just for debug will be removed later
                    //StartHurtAnimation(); // Play Hurt animation for other hits
                    roundSystem.PlayerFinishedCombo();
                }

                hitEffect.SetActive(true); // Activate Hit Effect in the body of AI
                // roundSystem.audioSystem.EnemyEffect(roundSystem.currentEnemyCharacter); // Start character Effect sound in another Audio Source different from what Player will use to play his Effect sound, only after effect is enabled, create a new Audio Source for it
                Invoke(nameof(DisableEffect), 1f); // Deactivate Hit Effect after 1 second
            }

            isHit = true; // Activate the condition that opponent was hit, now only the end of Hit animation will deactivate this trigger
        }
    }

    #endregion

    #region Animation Operations

    private void StartWalkAnimation()
    {
        // In both cases is checked if AI is level zero to stop to move and if is of other levels to move it correctly

        if (enemyAnimator.GetBool("isForward") == false && changedAnimDirectionToForward == true ||
            enemyDifficulty == 0 && moveSuccessRandom == false && enemyAnimator.GetBool("isForward") == false && changedAnimDirectionToForward == true)
        {
            enemyAnimator.SetBool("isForward", true); // Prevents to execute animation call many times, this way we only call 1 time the correct animation
            enemyAnimator.SetBool("isIdle", false); // Values in parameters should be low case in the first letter because is variable name - Felipe
            enemyAnimator.SetBool("isBackward", false); // Values in parameters should be low case in the first letter because is variable name - Felipe
            enemyAnimator.SetBool("isBlock", false); // Values in parameters should be low case in the first letter because is variable name - Felipe
            enemyAnimator.SetBool("isAttack1", false); // Values in parameters should be low case in the first letter because is variable name - Felipe
            enemyAnimator.SetBool("isAttack2", false); // Values in parameters should be low case in the first letter because is variable name - Felipe
            enemyAnimator.SetBool("isAttack3", false); // Values in parameters should be low case in the first letter because is variable name - Felipe
            //roundSystem.audioSystem.MoveLeft(2, roundSystem.currentEnemyCharacter); // Start character Move Left sound in Enemy Audio only after animation has started - Optional
            isWalking = true; // Prevents to execute animation call many times, this way we only call 1 time the correct animation
        }

        if (enemyAnimator.GetBool("isBackward") == false && changedAnimDirectionToBackward == true ||
            enemyDifficulty == 0 && moveSuccessRandom == false && enemyAnimator.GetBool("isBackward") == false && changedAnimDirectionToBackward == true)
        {
            enemyAnimator.SetBool("isBackward", true); // Prevents to execute animation call many times, this way we only call 1 time the correct animation
            enemyAnimator.SetBool("isIdle", false); // Values in parameters should be low case in the first letter because is variable name - Felipe
            enemyAnimator.SetBool("isForward", false); // Values in parameters should be low case in the first letter because is variable name - Felipe
            enemyAnimator.SetBool("isBlock", false); // Values in parameters should be low case in the first letter because is variable name - Felipe
            enemyAnimator.SetBool("isAttack1", false); // Values in parameters should be low case in the first letter because is variable name - Felipe
            enemyAnimator.SetBool("isAttack2", false); // Values in parameters should be low case in the first letter because is variable name - Felipe
            enemyAnimator.SetBool("isAttack3", false); // Values in parameters should be low case in the first letter because is variable name - Felipe
            //roundSystem.audioSystem.MoveRight(2, roundSystem.currentEnemyCharacter); // Start character Move Right sound in Enemy Audio only after animation has started - Optional
            isWalking = true; // Prevents to execute animation call many times, this way we only call 1 time the correct animation
        }
    }

    private void StartIdleAnimation()
    {
        if (enemyAnimator.GetBool("isIdle") == false)
        {
            enemyAnimator.SetBool("isIdle", true); // Prevents to execute animation call many times, this way we only call 1 time the correct animation
            enemyAnimator.SetBool("isForward", false); // Values in parameters should be low case in the first letter because is variable name - Felipe
            enemyAnimator.SetBool("isBackward", false); // Values in parameters should be low case in the first letter because is variable name - Felipe
            enemyAnimator.SetBool("isBlock", false); // Values in parameters should be low case in the first letter because is variable name - Felipe
            enemyAnimator.SetBool("isAttack1", false); // Values in parameters should be low case in the first letter because is variable name - Felipe
            enemyAnimator.SetBool("isAttack2", false); // Values in parameters should be low case in the first letter because is variable name - Felipe
            enemyAnimator.SetBool("isAttack3", false); // Values in parameters should be low case in the first letter because is variable name - Felipe
            //roundSystem.audioSystem.Idle(2, roundSystem.currentEnemyCharacter); // Start character Idle sound in Enemy Audio only after animation has started - Optional
            isWalking = false; // Prevents to execute animation call many times, this way we only call 1 time the correct animation
        }
    }

    private void StartAttack1Animation()
    {
        if (enemyAnimator.GetBool("isAttack1") == false)
        {
            enemyAnimator.SetBool("isAttack1", true); // Prevents to execute animation call many times, this way we only call 1 time the correct animation
            enemyAnimator.SetBool("isIdle", false); // Values in parameters should be low case in the first letter because is variable name - Felipe
            enemyAnimator.SetBool("isForward", false); // Values in parameters should be low case in the first letter because is variable name - Felipe
            enemyAnimator.SetBool("isBackward", false); // Values in parameters should be low case in the first letter because is variable name - Felipe
            enemyAnimator.SetBool("isBlock", false); // Values in parameters should be low case in the first letter because is variable name - Felipe
            enemyAnimator.SetBool("isAttack2", false); // Values in parameters should be low case in the first letter because is variable name - Felipe
            enemyAnimator.SetBool("isAttack3", false); // Values in parameters should be low case in the first letter because is variable name - Felipe
            //roundSystem.audioSystem.Attack1(2, roundSystem.currentEnemyCharacter); // Start character Attack 1 sound in Enemy Audio only after animation has started
            isWalking = false; // Prevents to execute animation call many times, this way we only call 1 time the correct animation
        }
    }

    private void StartAttack2Animation()
    {
        if (enemyAnimator.GetBool("isAttack2") == false)
        {
            enemyAnimator.SetBool("isAttack2", true); // Prevents to execute animation call many times, this way we only call 1 time the correct animation
            enemyAnimator.SetBool("isIdle", false); // Values in parameters should be low case in the first letter because is variable name - Felipe
            enemyAnimator.SetBool("isForward", false); // Values in parameters should be low case in the first letter because is variable name - Felipe
            enemyAnimator.SetBool("isBackward", false); // Values in parameters should be low case in the first letter because is variable name - Felipe
            enemyAnimator.SetBool("isBlock", false); // Values in parameters should be low case in the first letter because is variable name - Felipe
            enemyAnimator.SetBool("isAttack1", false); // Values in parameters should be low case in the first letter because is variable name - Felipe
            enemyAnimator.SetBool("isAttack3", false); // Values in parameters should be low case in the first letter because is variable name - Felipe
            //roundSystem.audioSystem.Attack2(2, roundSystem.currentEnemyCharacter); // Start character Attack 2 sound in Enemy Audio only after animation has started
            isWalking = false; // Prevents to execute animation call many times, this way we only call 1 time the correct animation
        }
    }

    private void StartAttack3Animation()
    {
        if (enemyAnimator.GetBool("isAttack3") == false)
        {
            enemyAnimator.SetBool("isAttack3", true); // Prevents to execute animation call many times, this way we only call 1 time the correct animation
            enemyAnimator.SetBool("isIdle", false); // Values in parameters should be low case in the first letter because is variable name - Felipe
            enemyAnimator.SetBool("isForward", false); // Values in parameters should be low case in the first letter because is variable name - Felipe
            enemyAnimator.SetBool("isBackward", false); // Values in parameters should be low case in the first letter because is variable name - Felipe
            enemyAnimator.SetBool("isBlock", false); // Values in parameters should be low case in the first letter because is variable name - Felipe
            enemyAnimator.SetBool("isAttack1", false); // Values in parameters should be low case in the first letter because is variable name - Felipe
            enemyAnimator.SetBool("isAttack2", false); // Values in parameters should be low case in the first letter because is variable name - Felipe
            //roundSystem.audioSystem.Attack3(2, roundSystem.currentEnemyCharacter); // Start character Attack 3 sound in Enemy Audio only after animation has started
            isWalking = false; // Prevents to execute animation call many times, this way we only call 1 time the correct animation
        }
    }

    private void StartBlockAnimation()
    {
        if (enemyAnimator.GetBool("isBlock") == false)
        {
            enemyAnimator.SetBool("isBlock", true); // Prevents to execute animation call many times, this way we only call 1 time the correct animation
            enemyAnimator.SetBool("isIdle", false); // Values in parameters should be low case in the first letter because is variable name - Felipe
            enemyAnimator.SetBool("isForward", false); // Values in parameters should be low case in the first letter because is variable name - Felipe
            enemyAnimator.SetBool("isBackward", false); // Values in parameters should be low case in the first letter because is variable name - Felipe
            enemyAnimator.SetBool("isAttack1", false); // Values in parameters should be low case in the first letter because is variable name - Felipe
            enemyAnimator.SetBool("isAttack2", false); // Values in parameters should be low case in the first letter because is variable name - Felipe
            enemyAnimator.SetBool("isAttack3", false); // Values in parameters should be low case in the first letter because is variable name - Felipe
            //roundSystem.audioSystem.Hit(2, roundSystem.currentEnemyCharacter); // Start character Hit sound in Enemy Audio only after animation has started
            isWalking = false; // Prevents to execute animation call many times, this way we only call 1 time the correct animation
        }
    }

    private void StartIntroAnimation()
    {
        //Debug.Log("Enemy started Intro anim");

        if (roundSystem.currentRound != 1)
        {
            gameObject.transform.position = initialPosition; // Move Enemy to initial position because a new round started
        }

        enemyAnimator.Play("isIntro"); // Call directly Intro animation and it goes automatically to Idle animation when Intro animation to finish
        roundSystem.audioSystem.PlayIntro(2, roundSystem.currentEnemyCharacter); // Start character Intro sound in Enemy Audio only after animation has started
    }

    public void StartVictoryAnimation()
    {
        ResetAllAnimations();

        //Debug.Log("Enemy Victory was activated");

        enemyAnimator.Play("isVictory"); // Call directly Victory animation and repeat it till another round start or to finish the fight
        // roundSystem.audioSystem.PlayVictory(2, roundSystem.currentEnemyCharacter); // Start character Victory sound in Enemy Audio only after animation has started
    }

    public void StartDrawAnimation()
    {
        ResetAllAnimations();

        //Debug.Log("Enemy Draw was activated");

        enemyAnimator.Play("isDefeat"); // Call directly Draw animation and repeat it till another round start or to finish the fight
        // roundSystem.audioSystem.PlayDraw(2, roundSystem.currentEnemyCharacter); // Start character Draw sound in Enemy Audio only after animation has started
    }

    public void StartDefeatAnimation()
    {
        ResetAllAnimations();

        //Debug.Log("Enemy Defeat was activated");

        enemyAnimator.Play("isDefeat"); // Call directly Defeat animation and repeat it till another round start or to finish the fight
        // roundSystem.audioSystem.PlayDefeat(2, roundSystem.currentEnemyCharacter); // Start character Defeat sound in Enemy Audio only after animation has started
    }

    private void StartStunnedAnimation()
    {
        ResetAllAnimations();
        enemyAnimator.Play("isStunned");
        // roundSystem.audioSystem.PlayStunned(2, roundSystem.currentEnemyCharacter); // Start character Stunned sound in Enemy Audio only after animation has started
    }

    private void StartHurtAnimation()
    {
        ResetAllAnimations();
        enemyAnimator.Play("isHurt");
        // roundSystem.audioSystem.PlayHurt(2, roundSystem.currentEnemyCharacter); // Start character Hurt sound in Enemy Audio only after animation has started
    }

    #endregion

    #region Last animations frame operations

    public void LastFrameAttack() // Called in the final frame of attack animation
    {
        if (checkDamage == false)
        {
            attackSuccessRandom = false; // Reset to allow another attack randomization after cooldown
            isAttacking = false; // Reset to allow another attack after cooldown
            checkDamage = true; // Check damage from last attack
            isWalking = true; // AI can move if player to get far from punch area
            canFight = true; // AI can follow player if is outside range
            StartIdleAnimation(); // Reset animation to repeat the attack if player is inside range yet            
        }
    }

    public void LastFrameBlock()
    {
        EnemyIsIdle(); // Start Idle animation after opponent to get a hit
    }

    #endregion

    #region Attack Operations

    private void RandomizeSkill()
    {
        skillRandom = Random.Range(1, 100); // Randomize current attack skill

        #region Enemy Difficulty 0 - Easy

        if (enemyDifficulty == 0 && skillRandom >= 0 && skillRandom <= 20) // 20% chance to use Attack 3
        {
            //Debug.Log("AI level 0 difficulty was choice Attack 3");

            actualAttack = 3;
        }

        if (enemyDifficulty == 0 && skillRandom >= 21 && skillRandom <= 50) // 30% chance to use Attack 2
        {
            //Debug.Log("AI level 0 difficulty was choice Attack 2");

            actualAttack = 2;
        }

        if (enemyDifficulty == 0 && skillRandom >= 51 && skillRandom <= 100) // 50% chance to use Attack 1
        {
            //Debug.Log("AI level 0 difficulty was choice Attack 1");

            actualAttack = 1;
        }

        #endregion

        #region Enemy Difficulty 1 - Moderate

        if (enemyDifficulty == 1 && skillRandom >= 0 && skillRandom <= 40) // 40% chance to use Attack 3
        {
            //Debug.Log("AI level 1 difficulty was choice Attack 3");

            actualAttack = 3;
        }

        if (enemyDifficulty == 1 && skillRandom >= 41 && skillRandom <= 70) // 30% chance to use Attack 2
        {
            //Debug.Log("AI level 1 difficulty was choice Attack 2");

            actualAttack = 2;
        }

        if (enemyDifficulty == 1 && skillRandom >= 71 && skillRandom <= 100) // 30% chance to use Attack 1
        {
            //Debug.Log("AI level 1 difficulty was choice Attack 1");

            actualAttack = 1;
        }

        #endregion

        #region Enemy Difficulty 2 - Normal

        if (enemyDifficulty == 2 && skillRandom >= 0 && skillRandom <= 10) // 10% chance to use Attack 3
        {
            //Debug.Log("AI level 2 difficulty was choice Attack 3");

            actualAttack = 3;
        }

        if (enemyDifficulty == 2 && skillRandom >= 11 && skillRandom <= 70) // 60% chance to use Attack 2
        {
            //Debug.Log("AI level 2 difficulty was choice Attack 2");

            actualAttack = 2;
        }

        if (enemyDifficulty == 2 && skillRandom >= 71 && skillRandom <= 100) // 30% chance to use Attack 1
        {
            //Debug.Log("AI level 2 difficulty was choice Attack 1");

            actualAttack = 1;
        }

        #endregion

        #region Enemy Difficulty 3 - Hard

        if (enemyDifficulty == 3 && skillRandom >= 0 && skillRandom <= 70) // 70% chance to use Attack 3
        {
            //Debug.Log("AI level 3 difficulty was choice Attack 3");

            actualAttack = 3;
        }

        if (enemyDifficulty == 3 && skillRandom >= 71 && skillRandom <= 90) // 20% chance to use Attack 2
        {
            //Debug.Log("AI level 3 difficulty was choice Attack 2");

            actualAttack = 2;
        }

        if (enemyDifficulty == 3 && skillRandom >= 91 && skillRandom <= 100) // 10% chance to use Attack 1
        {
            //Debug.Log("AI level 3 difficulty was choice Attack 1");

            actualAttack = 1;
        }

        #endregion

        EnemyIsAttacking(); // Attack if player is inside attack area
    }

    private void EnemyIsAttacking()
    {
        if (isAttacking == false) // We only want to make CPU to read here only 1 time
        {
            isAttacking = true; // Closing the IF so CPU only will read it 1 time and it should be called before any attack animation

            if (actualAttack == 1)
            {
                UseAttack1(); // Start Attack 1 if AI selected skill 1 to attack
            }

            if (actualAttack == 2)
            {
                UseAttack2(); // Start Attack 2 if AI selected skill 2 to attack
            }

            if (actualAttack == 3)
            {
                UseAttack3(); // Start Attack 3 if AI selected skill 3 to attack
            }

            // Attack animations will deactivate isAttacking in the last frame, so we make sure isAttacking is true before to play any Attack animation
        }
    }

    private void UseAttack3()
    {
        if (isCooldown3 == false) // If Attack 3 not in cooldown start Attack 3
        {
            StartAttack3Animation(); // Now activate the attack 3 animation
            isCooldown3 = true;
        }
        else
        {
            if (isCooldown1 == true && isCooldown2 == true && isCooldown3 == true)
            {
                LastFrameAttack(); // If all skills are in cooldown, cancel the attack
            }

            if (isCooldown3 == true && isCooldown1 == false && isCooldown2 == true ||
                isCooldown3 == true && isCooldown1 == false && isCooldown2 == false)
            {
                UseAttack1(); // If Attack 3 is in cooldown so change to Attack 1
            }

            if (isCooldown3 == true && isCooldown2 == false && isCooldown1 == true)
            {
                UseAttack2(); // If Attack 3 is in cooldown so change to Attack 2
            }
        }
    }

    private void EnemyIsIdle()
    {
        if (isHit == true) // Check if Hit trigger is still activated and disable it
        {
            isHit = false;
        }

        if (isAttacking == true) // Check if Attacking trigger is still activated and disable it
        {
            isAttacking = false;
        }

        StartIdleAnimation(); // Start Idle Animation
    }

    private void UseAttack1()
    {
        if (isCooldown1 == false) // If Attack 1 not in cooldown start Attack 1
        {
            StartAttack1Animation(); // Now activate the attack 1 animation
            isCooldown1 = true;
        }
        else
        {
            UseAttack2(); // If Attack 1 is in cooldown so change to Attack 2
        }
    }

    private void UseAttack2()
    {
        if (isCooldown2 == false) // If Attack 2 not in cooldown start Attack 2
        {
            StartAttack2Animation(); // Now activate the attack 2 animation
            isCooldown2 = true;
        }
        else
        {
            UseAttack3(); // If Attack 2 is in cooldown so change to Attack 3
        }
    }

    #endregion

    #region Round Operations

    private void ResetAllTriggers()
    {
        //Debug.Log("Reset all Enemy´s triggers");

        randomizeTimer = 0f; // Reset randomizer time when next round to start if to have a new round yet
        canFight = false; // Round finished, stop to fight - Felipe
        isWalking = false; // Disable movement
        canRandomize = false; // Round finished, stop to randomize - Felipe
        isHit = false; // Round finished, reset all variables
        isAttacking = false; // Round finished, reset all variables
        checkDamage = false; // Round finished, reset all variables
        moveSuccessRandom = false; // Round finished, reset all variables
        attackSuccessRandom = false; // Round finished, reset all variables
        changedAnimDirectionToBackward = false; // Disable all directions movement
        changedAnimDirectionToForward = false; // Disable all directions movement
        wasResetTriggers = true; // All tiggers was reset, so close this Reset Trigger call
    }

    private void ResetAllAnimations()
    {
        enemyAnimator.SetBool("isIntro", false); // Values in parameters should be low case in the first letter because is variable name - Felipe
        enemyAnimator.SetBool("isIdle", false); // Values in parameters should be low case in the first letter because is variable name - Felipe
        enemyAnimator.SetBool("isForward", false); // Values in parameters should be low case in the first letter because is variable name - Felipe
        enemyAnimator.SetBool("isBackward", false); // Values in parameters should be low case in the first letter because is variable name - Felipe
        enemyAnimator.SetBool("isBlock", false); // Values in parameters should be low case in the first letter because is variable name - Felipe
        enemyAnimator.SetBool("isDead", false); // Values in parameters should be low case in the first letter because is variable name - Felipe
        enemyAnimator.SetBool("isAttack1", false); // Values in parameters should be low case in the first letter because is variable name - Felipe
        enemyAnimator.SetBool("isAttack2", false); // Values in parameters should be low case in the first letter because is variable name - Felipe
        enemyAnimator.SetBool("isAttack3", false); // Values in parameters should be low case in the first letter because is variable name - Felipe
    }

    #endregion

    #region Effects Operations

    private void DisableEffect()
    {
        hitEffect.SetActive(false); // Deactivate Hit Effect in the body of AI
    }

    #endregion
}
