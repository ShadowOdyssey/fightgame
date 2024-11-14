using UnityEngine;

public class MarcusAI : MonoBehaviour
{
    public RoundManager roundSystem;
    public GabriellaMovementPlay gabriellaSystem;

    public GameObject hitEffect; // GameObject of Hit Effect that will be Set Active true or false to show it on screen
    public Transform target; // Gabriella's transform

    public Animator animator;

    public int health = 100;
    public int enemyDifficulty = 0; // 0 = easy 1 = moderate 2 = normal 3 = hard
    public int randomMaxValue = 100;
    public float attackRange = 13f;
    public float moveSpeed = 2f;
    public float randomizeTimer = 0f;

    private Vector3 originalPosition; // Save original position to reset after getting up
    private int actualRandomValue = 0;
    private int attackRandom = 0;
    private int hitCount = 0; // Track the number of times Marcus is hit
    private float distanceToTarget = 0f;
    private bool successRandom = false; // The success is based in the difficulty level
    private bool isResetRandom = false;
    private bool isWalking = false;
    private bool isAttacking = false;
    private bool canFight = false;
    private bool checkDamage = false;
    private bool canRandomize = false;
    private bool changedAnimDirectionToForward = false;
    private bool changedAnimDirectionToBackward = false;

    private void Start()
    {
        distanceToTarget = Vector3.Distance(transform.position, target.position); // Get initial position from Gabriella to get the first distance measure only once
    }

    private void Update()
    {
        if (canFight == false && roundSystem.roundStarted == true && successRandom == false) // Only can execute commands in FixedUpdate if round started, but...
        {
            Debug.Log("Round Started");

            canRandomize = true; // Round started, so activate randomizer
            canFight = true; // Round started, so can fight
        }

        if (canFight == true && roundSystem.roundStarted == false) // If round not started and is 2nd or 3rd round, load Idle animation till round start again! - Felipe
        {
            canRandomize = false; // Round finished, stop to randomize - Felipe
            randomizeTimer = 0f; // Reset randomizer time when next round to start if to have a new round yet
            canFight = false; // Round finished, stop to fight - Felipe
            isWalking = false; // Disable movement
            changedAnimDirectionToBackward = false; // Disable all directions movement
            changedAnimDirectionToForward = false; // Disable all directions movement
            Idle(); // Round finished, trigger Idle animation - We can change it later to defeat animation or victory animation on each round based in remaning life - Felipe
        }

        if (canRandomize == true && isAttacking == false) // Begin to randomize to AI to make decisions so fighting against IA will not to be linear and predictible
        {
            randomizeTimer = randomizeTimer + Time.deltaTime; // Count the time to start to randomize

            if (randomizeTimer >= 3f) // I was setup 3 seconds, but you can change this value if you want
            {
                actualRandomValue = Random.Range(1, randomMaxValue); // Randomizing a new value

                switch (enemyDifficulty) // Check sucess in the random number generated
                {
                    case 0: // Less agressive and less defensive

                        if (actualRandomValue <= 1 && actualRandomValue <= 80) // 80% chance to AI not to change behaviour
                        {
                            successRandom = false; // Continue to do what is doing
                        }

                        if (actualRandomValue >= 81 &&  actualRandomValue <= 100) // 20% chance to AI to change behaviour
                        {
                            successRandom = true; // No agression and no defense - Call for more Idle and stop to move in the middle of the combate and dont attack
                        }

                        break;
                    
                    case 1: // less agressive and more defensive

                        if (actualRandomValue <= 1 && actualRandomValue <= 40) // 40% chance to AI not to change behaviour
                        {
                            successRandom = false; // Continue to do what is doing
                        }

                        if (actualRandomValue >= 41 && actualRandomValue <= 100) // 60% chance to AI to change behaviour
                        {
                            successRandom = true; // Move more far from player or use defensive skills if possible
                        }

                        break;
                    
                    case 2: // More agressive and less defensive

                        if (actualRandomValue <= 1 && actualRandomValue <= 50) // 50% chance to AI not to change behaviour
                        {
                            successRandom = false; // Continue to do what is doing
                        }

                        if (actualRandomValue >= 51 && actualRandomValue <= 100) // 50% chance to AI to change behaviour
                        {
                            successRandom = true; // Move more near from player or use aggressive skills if possible
                        }

                        break;
                    
                    case 3: // More agressive and more defensive

                        if (actualRandomValue <= 1 && actualRandomValue <= 20) // 20% chance to AI not to change behaviour
                        {
                            successRandom = false; // Continue to do what is doing
                        }

                        if (actualRandomValue >= 21 && actualRandomValue <= 100) // 80% chance to AI to change behaviour
                        {
                            successRandom = true; // Move more near from player and use aggressive skills if player is far or move more far from player or use defensive skills if possible
                        }

                        break;
                }

                if (isWalking == true && successRandom == true) // Apply the success in random number based in the level difficulty
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
                                Idle();
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

                if (successRandom == true) // Check if AI got succes on the closed thead, reset it to generate new value...
                {
                    successRandom = false; // Disable last success in random to make success avaiable again
                }

                isResetRandom = false; // We already was reset randomizer system

                if (canRandomize == false) // Only activate if CanRandomize was being used
                {
                    canRandomize = true; // Randomize AI actions again!!!
                }
            }
        }
    }

    private void FixedUpdate()
    {
        if (health <= 0) return; // Prevent further actions if Marcus is dead

        if (canFight == true) // Prevent further actions if round not started yet
        {
            distanceToTarget = Vector3.Distance(transform.position, target.position);

            //Debug.Log("Actual distance to target from Marcus is: " + distanceToTarget); // Debug actual distance between Marcus and Gabriella

            if (distanceToTarget < attackRange && distanceToTarget > attackRange - 1f)
            {
                if (checkDamage == true && distanceToTarget > attackRange - 0.5f) // Only apply damage if player is really inside attack area
                {
                    gabriellaSystem.TakeHit(15);
                    checkDamage = false;
                }

                if (isResetRandom == false) // Call for attack action randomize, AI can decide if attack or not when player is inside attack area
                {
                    attackRandom = Random.Range(1, 100);

                    if (enemyDifficulty == 0 && attackRandom >= 81 && attackRandom <= 100) // 20% the easy AI have to attack
                    {
                        Attack(); // Attack if player is inside attack area
                    }

                    if (enemyDifficulty == 1 && attackRandom >= 61 && attackRandom <= 100) // 40% the moderate AI have to attack
                    {
                        Attack(); // Attack if player is inside attack area
                    }

                    if (enemyDifficulty == 2 && attackRandom >= 41 && attackRandom <= 100) // 60% the normal AI have to attack
                    {
                        Attack(); // Attack if player is inside attack area
                    }

                    if (enemyDifficulty == 3 && attackRandom >= 21 && attackRandom <= 100) // 80% the hard AI have to attack
                    {
                        Attack(); // Attack if player is inside attack area
                    }

                    isResetRandom = true; // Close the attack random call and reset it
                }

                // Attack area is determined by Attack Range and Attack Range -1, so if Attack Range is 8, the area will between 8f and 7f in the distance value, if player is 6.9f or less the AI will move backward
            }
            
            if (distanceToTarget > attackRange && isAttacking == false)
            {
                if (checkDamage == true) // Check if player got out from attack area when damage is trying to be applied
                {
                    checkDamage = false; // Player is outside attack area so dont apply damage to player, only apply damage if player is really inside attack area
                }

                FixWalkAnimDirectionToForward();
                Move(); // Follow player if is outside attack area to attack the player
            }

            if (distanceToTarget < attackRange && distanceToTarget < attackRange - 1f && isAttacking == false)
            {
                FixWalkAnimDirectionToBackward();
                Move(); // Get far from player if is to much inside attack area to not let AI vulnerable for attacks
            }
        }
    }

    private void Move()
    {
        StartWalkAnimation();

        if (isWalking == true && enemyDifficulty > 0 || // Only AI difficulty zero stops to move, so check if is AI difficulty level zero
            isWalking == true && enemyDifficulty == 0 && successRandom == false)  // If is difficulty level zero, we make sure to only apply movement if success was false, because if true AI should stop to move
        {
            if (changedAnimDirectionToForward == true || successRandom == true && enemyDifficulty == 2 || successRandom == true && enemyDifficulty == 3)
            {
                // Check if normal and hard enemy difficulty got success to move forward

                transform.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - moveSpeed * Time.deltaTime);
            }
            
            if (changedAnimDirectionToBackward == true || successRandom == true && enemyDifficulty == 1 || successRandom == true && enemyDifficulty == 3)
            {
                // Check if moderate and hard enemy difficulty got success to move forward

                transform.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + moveSpeed * Time.deltaTime);
            }
        }
    }

    // Called when Marcus takes damage
    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0)
        {
            Die();
        }
        else
        {
            if (hitCount != hitCount + 1)
            {
                Debug.Log("Checking for actual Hit Count");

                hitCount = hitCount + 1; // Increment hit count
            }

            if (hitCount == 1)
            {
                // Play block animation on first hit
                animator.SetTrigger("block"); // Values in parameters should be low case in the first letter because is variable name - Felipe
                ShowHitEffect();
            }
            else if (hitCount == 3)
            {
                // Play stunned animation on third hit
                animator.SetTrigger("stunned"); // Values in parameters should be low case in the first letter because is variable name - Felipe
                ShowHitEffect();

                // Start coroutine to play getting up animation and reset position
                StartCoroutine(StunRecoveryCoroutine());
            }
            else
            {
                // Play hurt animation for other hits
                animator.SetTrigger("hurt"); // Values in parameters should be low case in the first letter because is variable name - Felipe
                ShowHitEffect();
            }
        }
    }

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

    private void StartWalkAnimation()
    {
        if (animator.GetBool("isForward") == false && changedAnimDirectionToForward == true ||
            enemyDifficulty == 0 && successRandom == false && animator.GetBool("isForward") == false && changedAnimDirectionToForward == true)
        // Prevents to execute animation call many times, this way we only call 1 time the correct animation
        {
            animator.SetBool("isIdle", false); // Values in parameters should be low case in the first letter because is variable name - Felipe
            animator.SetBool("isForward", true); // Values in parameters should be low case in the first letter because is variable name - Felipe
            animator.SetBool("isBackward", false); // Values in parameters should be low case in the first letter because is variable name - Felipe
            animator.SetBool("isAttacking", false); // Values in parameters should be low case in the first letter because is variable name - Felipe
            isWalking = true;
        }

        if (animator.GetBool("isBackward") == false && changedAnimDirectionToBackward == true ||
            enemyDifficulty == 0 && successRandom == false && animator.GetBool("isBackward") == false && changedAnimDirectionToBackward == true)
            // Prevents to execute animation call many times, this way we only call 1 time the correct animation
        {
            animator.SetBool("isIdle", false); // Values in parameters should be low case in the first letter because is variable name - Felipe
            animator.SetBool("isForward", false); // Values in parameters should be low case in the first letter because is variable name - Felipe
            animator.SetBool("isBackward", true); // Values in parameters should be low case in the first letter because is variable name - Felipe
            animator.SetBool("isAttacking", false); // Values in parameters should be low case in the first letter because is variable name - Felipe
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
            isWalking = false;
        }
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

    private void ShowHitEffect()
    {
        hitEffect.SetActive(true); // Activate Hit Effect in the body of AI
    }

    public void CharacterFinishedAttack() // Called in the final frame of attack animation
    {
        isAttacking = false; // Reset to allow another attack after cooldown
        checkDamage = true; // Check damage from last attack
        isWalking = true; // AI can move if player to get far from punch area
        canFight = true; // AI can follow player if is outside range
        StartIdleAnimation(); // Reset animation to repeat the attack if player is inside range yet
    }

    private void Die()
    {
        if (animator.GetBool("isDead") == false) // Prevents to execute animation call many times, this way we only call 1 time the correct animation
        {
            animator.SetBool("isDead", true); // Values in parameters should be low case in the first letter because is variable name - Felipe
        }
    }


    private void Idle()
    {
        StartIdleAnimation(); // Start Idle Animation
    }

    private void Attack()
    {
        if (isAttacking == false) // We only want to make CPU to read here only 1 time
        {
            isAttacking = true; // Closing the IF so CPU only will read it 1 time
            StartAttackAnimation(); // Now activate the attack animation
        }
    }

    public void IsDead() // Called in the final frame of death animation
    {
        Destroy(gameObject); // Destroy Marcus after to reach the final frame in death animation
    }
}
