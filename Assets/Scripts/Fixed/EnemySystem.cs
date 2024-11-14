using UnityEngine;

public class EnemySystem : MonoBehaviour
{
    #region Variables

    [Header("Animator Setup")]
    [Tooltip("Attach current enemy Animator component here")]
    public Animator animator;

    [Header("Hit Effect Setup")]
    [Tooltip("Attach current enemy HitEffect GameObject here")]
    public GameObject hitEffect;

    [Header("Enemy Setup")]
    public int totalHealth = 100;
    public int enemyDifficulty = 0; // 0 = easy 1 = moderate 2 = normal 3 = hard
    public int randomMaxValue = 100;
    public float attackRange = 13f;
    public float moveSpeed = 2f;
    public float randomizeTimer = 0f;
    public float distanceToTarget = 0f;

    #region Hidden Variables

    [Header("Monitor")] // Turn variables into public to show monitor
    [Tooltip("Actual Round System from RoundManager object in the current scene, it will be loaded when scene to awake")]
    private RoundManager roundSystem;
    [Tooltip("Actual Player System from selected player in singleplayer or multiplayer, it will be loaded when scene to awake")]
    private PlayerSystem playerSystem;
    [Tooltip("Actual Player Transform from selected player in singleplayer or multiplayer, it will be loaded when scene to awake")]
    private Transform playerBody;
    [Tooltip("Move Random determines if IA will move, stop or reverse actual movemente")]
    private int moveRandom = 0;
    [Tooltip("Attack Random determines if IA will attack, stop or to select different skills")]
    private int attackRandom = 0;
    [Tooltip("Hit Count determines the combo hit and the combo should be applied in a short period of time by player")]
    private int hitCount = 0;
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

    #endregion

    #endregion

    #region Loading Components

    private void Awake()
    {
        // When multiplayer to be done we need to look for the right components, the other components declared dont need to be found, just attached in Inspector

        roundSystem = GameObject.Find("RoundManager").GetComponent<RoundManager>();
        playerBody = GameObject.Find("Gabriella").GetComponent<Transform>();
        playerSystem = GameObject.Find("Gabriella").GetComponent<PlayerSystem>();
    }

    #endregion

    #region Setup Loaded Components

    private void Start()
    {
        distanceToTarget = Vector3.Distance(transform.position, playerBody.position); // Get initial position from Player to get the first distance measure only once
    }

    #endregion

    #region Real Time Operations

    private void Update()
    {
        #region Checking if round started

        if (canFight == false && roundSystem.roundStarted == true && moveSuccessRandom == false) // Only can execute commands in FixedUpdate if round started, but...
        {
            Debug.Log("Round Started");

            canRandomize = true; // Round started, so activate randomizer
            canFight = true; // Round started, so can fight
        }

        #endregion

        #region Checking if round finished

        if (canFight == true && roundSystem.roundStarted == false) // If round not started and is 2nd or 3rd round, load Idle animation till round start again! - Felipe
        {
            canRandomize = false; // Round finished, stop to randomize - Felipe
            randomizeTimer = 0f; // Reset randomizer time when next round to start if to have a new round yet
            canFight = false; // Round finished, stop to fight - Felipe
            isWalking = false; // Disable movement
            changedAnimDirectionToBackward = false; // Disable all directions movement
            changedAnimDirectionToForward = false; // Disable all directions movement
            EnemyIsIdle(); // Round finished, trigger Idle animation - We can change it later to defeat animation or victory animation on each round based in remaning life - Felipe
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
                    moveSuccessRandom = false; // Disable last success in random to make success avaiable again
                }

                isResetRandom = false; // We already was reset randomizer system

                if (canRandomize == false) // Only activate if CanRandomize was being used
                {
                    canRandomize = true; // Randomize AI actions again!!!
                }
            }
        }

        #endregion
    }

    private void FixedUpdate()
    {
        #region Check if enemy is not alive anymore

        if (totalHealth <= 0) return; // Prevent further actions if Enemy is dead

        #endregion

        #region Check if enemy is still alive and perform actions if alive

        if (canFight == true) // Prevent further actions if round not started yet
        {
            distanceToTarget = Vector3.Distance(transform.position, playerBody.position);

            //Debug.Log("Actual distance to target from Enemy is: " + distanceToTarget); // Debug actual distance between Enemy and Player

            #region Check distance to player and make decisions

            #region Decide what to do when player is inside attack range

            if (distanceToTarget < attackRange && distanceToTarget > attackRange - 1f && isHit == false)
            {
                #region Enemy deals damage to player

                if (checkDamage == true) // Only apply damage if player is really inside attack area
                {
                    playerSystem.TakeHit(15);
                    checkDamage = false;
                }

                #endregion

                #region Randomize Attack

                if (isResetRandom == false && isAttacking == false) // Call for attack action randomize, AI can decide if attack or not when player is inside attack area
                {
                    attackRandom = Random.Range(1, 100);

                    if (enemyDifficulty == 0 && attackRandom >= 81 && attackRandom <= 100) // 20% the easy AI have to attack
                    {
                        attackSuccessRandom = true;
                        EnemyIsAttacking(); // Attack if player is inside attack area
                    }

                    if (enemyDifficulty == 1 && attackRandom >= 61 && attackRandom <= 100) // 40% the moderate AI have to attack
                    {
                        attackSuccessRandom = true;
                        EnemyIsAttacking(); // Attack if player is inside attack area
                    }

                    if (enemyDifficulty == 2 && attackRandom >= 41 && attackRandom <= 100) // 60% the normal AI have to attack
                    {
                        attackSuccessRandom = true;
                        EnemyIsAttacking(); // Attack if player is inside attack area
                    }

                    if (enemyDifficulty == 3 && attackRandom >= 21 && attackRandom <= 100) // 80% the hard AI have to attack
                    {
                        attackSuccessRandom = true;
                        EnemyIsAttacking(); // Attack if player is inside attack area
                    }

                    if (attackSuccessRandom == false) // Check if enemy decided to attack player
                    {
                        EnemyIsIdle(); // Call for Idle because enemy is to much near player
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
            hitCount = 0; // Just for debug, it will be removed later

            totalHealth = totalHealth - damage;

            roundSystem.ApplyDamageToOpponent(damage); // Inform RoundManager that Enemy tooks damage by player

            if (totalHealth <= 0)
            {
                if (hitCount != 0)
                {
                    hitCount = 0; // Reset hit count because opponent died
                }

                Die(); // Kill opponent because life reached to zero
            }
            else
            {
                hitCount = hitCount + 1;

                if (hitCount == 1)
                {
                    // Play block animation on first hit
                    StartBlockAnim();
                }
                else if (hitCount == 3)
                {
                    // Play stunned animation on third hit
                    //animator.SetTrigger("stunned"); // Values in parameters should be low case in the first letter because is variable name - Felipe
                }
                else
                {
                    // Play hurt animation for other hits
                    //animator.SetTrigger("hurt"); // Values in parameters should be low case in the first letter because is variable name - Felipe
                }

                hitEffect.SetActive(true); // Activate Hit Effect in the body of AI
                Invoke(nameof(DisableEffect), 1f); // Deactivate Hit Effect after 1 second
            }

            isHit = true; // Activate the condition that opponent was hit, now only the end of Hit animation will deactivate this trigger
        }
    }

    #endregion

    /* I just let it here to consult about attack structure, we will not use coroutines
     * 
    private System.Collections.IEnumerator StunRecoveryCoroutine()
    {
        yield return new WaitForSeconds(2.0f); // Assume stunned animation duration

        // Play the getting-up animation
        animator.SetTrigger("gettingUp"); // Values in parameters should be low case in the first letter because is variable name - Felipe

        // Wait for getting-up animation to finish
        yield return new WaitForSeconds(1.5f);

        // Reset position to the original position
        transform.localPosition = originalPosition;

        // Reset hit count
        hitCount = 0;
    }
    */

    #region Animation Operations

    private void StartWalkAnimation()
    {
        if (animator.GetBool("isForward") == false && changedAnimDirectionToForward == true ||
            enemyDifficulty == 0 && moveSuccessRandom == false && animator.GetBool("isForward") == false && changedAnimDirectionToForward == true)
        // Prevents to execute animation call many times, this way we only call 1 time the correct animation
        {
            animator.SetBool("isIdle", false); // Values in parameters should be low case in the first letter because is variable name - Felipe
            animator.SetBool("isForward", true); // Values in parameters should be low case in the first letter because is variable name - Felipe
            animator.SetBool("isBackward", false); // Values in parameters should be low case in the first letter because is variable name - Felipe
            animator.SetBool("isAttacking", false); // Values in parameters should be low case in the first letter because is variable name - Felipe
            animator.SetBool("isBlock", false); // Values in parameters should be low case in the first letter because is variable name - Felipe
            isWalking = true;
        }

        if (animator.GetBool("isBackward") == false && changedAnimDirectionToBackward == true ||
            enemyDifficulty == 0 && moveSuccessRandom == false && animator.GetBool("isBackward") == false && changedAnimDirectionToBackward == true)
            // Prevents to execute animation call many times, this way we only call 1 time the correct animation
        {
            animator.SetBool("isIdle", false); // Values in parameters should be low case in the first letter because is variable name - Felipe
            animator.SetBool("isForward", false); // Values in parameters should be low case in the first letter because is variable name - Felipe
            animator.SetBool("isBackward", true); // Values in parameters should be low case in the first letter because is variable name - Felipe
            animator.SetBool("isAttacking", false); // Values in parameters should be low case in the first letter because is variable name - Felipe
            animator.SetBool("isBlock", false); // Values in parameters should be low case in the first letter because is variable name - Felipe
            isWalking = true;
        }
    }

    private void StartIdleAnimation()
    {
        if (animator.GetBool("isIdle") == false) // Prevents to execute animation call many times, this way we only call 1 time the correct animation
        {
            animator.SetBool("isIdle", true); // Values in parameters should be low case in the first letter because is variable name - Felipe
            animator.SetBool("isForward", false); // Values in parameters should be low case in the first letter because is variable name - Felipe
            animator.SetBool("isBackward", false); // Values in parameters should be low case in the first letter because is variable name - Felipe
            animator.SetBool("isAttacking", false); // Values in parameters should be low case in the first letter because is variable name - Felipe
            animator.SetBool("isBlock", false); // Values in parameters should be low case in the first letter because is variable name - Felipe
            isWalking = false;
        }
    }

    private void StartAttackAnimation()
    {
        if (animator.GetBool("isAttacking") == false) // Prevents to execute animation call many times, this way we only call 1 time the correct animation
        {
            animator.SetBool("isIdle", false); // Values in parameters should be low case in the first letter because is variable name - Felipe
            animator.SetBool("isForward", false); // Values in parameters should be low case in the first letter because is variable name - Felipe
            animator.SetBool("isBackward", false); // Values in parameters should be low case in the first letter because is variable name - Felipe
            animator.SetBool("isAttacking", true); // Values in parameters should be low case in the first letter because is variable name - Felipe
            animator.SetBool("isBlock", false); // Values in parameters should be low case in the first letter because is variable name - Felipe
            isWalking = false;
        }
    }

    private void StartBlockAnim()
    {
        if (animator.GetBool("isBlock") == false) // Prevents to execute animation call many times, this way we only call 1 time the correct animation
        {
            animator.SetBool("isBlock", true); // Values in parameters should be low case in the first letter because is variable name - Felipe
            animator.SetBool("isIdle", false); // Values in parameters should be low case in the first letter because is variable name - Felipe
            animator.SetBool("isForward", false); // Values in parameters should be low case in the first letter because is variable name - Felipe
            animator.SetBool("isBackward", false); // Values in parameters should be low case in the first letter because is variable name - Felipe
            animator.SetBool("isAttacking", false); // Values in parameters should be low case in the first letter because is variable name - Felipe
            isWalking = false;
        }
    }

    private void Die()
    {
        if (animator.GetBool("isDead") == false) // Prevents to execute animation call many times, this way we only call 1 time the correct animation
        {
            animator.SetBool("isDead", true); // Values in parameters should be low case in the first letter because is variable name - Felipe
            animator.SetBool("isIdle", false); // Values in parameters should be low case in the first letter because is variable name - Felipe
            animator.SetBool("isForward", false); // Values in parameters should be low case in the first letter because is variable name - Felipe
            animator.SetBool("isBackward", false); // Values in parameters should be low case in the first letter because is variable name - Felipe
            animator.SetBool("isAttacking", false); // Values in parameters should be low case in the first letter because is variable name - Felipe
            animator.SetBool("isBlock", false); // Values in parameters should be low case in the first letter because is variable name - Felipe
        }
    }

    #endregion

    #region Last animations frame operations

    public void CharacterFinishedAttack() // Called in the final frame of attack animation
    {
        attackSuccessRandom = false;
        isAttacking = false; // Reset to allow another attack after cooldown
        checkDamage = true; // Check damage from last attack
        isWalking = true; // AI can move if player to get far from punch area
        canFight = true; // AI can follow player if is outside range
        StartIdleAnimation(); // Reset animation to repeat the attack if player is inside range yet
    }

    public void HitAnimFinished()
    {
        EnemyIsIdle(); // Start Idle animation after opponent to get a hit
    }

    public void IsDead() // Called in the final frame of death animation
    {
        Destroy(gameObject, 5f); // Destroy Enemy after to reach the final frame in death animation after 5 seconds
    }

    #endregion

    #region Attack Operations

    private void EnemyIsAttacking()
    {
        if (isAttacking == false) // We only want to make CPU to read here only 1 time
        {
            isAttacking = true; // Closing the IF so CPU only will read it 1 time
            StartAttackAnimation(); // Now activate the attack animation
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

    #endregion

    #region Effects Operations

    private void DisableEffect()
    {
        hitEffect.SetActive(false); // Deactivate Hit Effect in the body of AI
    }

    #endregion
}
