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
            isWalking = false;
            changedAnimDirectionToBackward = false;
            changedAnimDirectionToForward = false;
            Idle(); // Round finished, trigger Idle animation - We can change it later to defeat animation or victory animation on each round based in remaning life - Felipe
        }

        if (canRandomize == true)
        {
            randomizeTimer = randomizeTimer + Time.deltaTime;

            if (randomizeTimer >= 3f)
            {
                actualRandomValue = Random.Range(1, randomMaxValue);

                switch (enemyDifficulty) // Check sucess in the random number generated
                {
                    case 0: // Less agressive and less defensive

                        if (actualRandomValue <= 1 && actualRandomValue <= 80)
                        {
                            successRandom = false; // Continue to do what is doing
                        }

                        if (actualRandomValue >= 81 &&  actualRandomValue <= 100)
                        {
                            successRandom = true; // No agression and no defense - Call for more Idle and stop to move in the middle of the combate and dont attack
                        }

                        break;
                    
                    case 1: // less agressive and more defensive

                        if (actualRandomValue <= 1 && actualRandomValue <= 40)
                        {
                            successRandom = false; // Continue to do what is doing
                        }

                        if (actualRandomValue >= 41 && actualRandomValue <= 100)
                        {
                            successRandom = true; // Move more far from player or use defensive skills if possible
                        }

                        break;
                    
                    case 2: // More agressive and less defensive

                        if (actualRandomValue <= 1 && actualRandomValue <= 80)
                        {
                            successRandom = false; // Continue to do what is doing
                        }

                        if (actualRandomValue >= 81 && actualRandomValue <= 100)
                        {
                            successRandom = true; // Move more near from player or use aggressive skills if possible
                        }

                        break;
                    
                    case 3: // More agressive and more defensive

                        if (actualRandomValue <= 1 && actualRandomValue <= 80)
                        {
                            successRandom = false; // Continue to do what is doing
                        }

                        if (actualRandomValue >= 81 && actualRandomValue <= 100)
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

                isResetRandom = true;
                randomizeTimer = 0f;
                canRandomize = false;
            }
        }

        if (isResetRandom == true)
        {
            randomizeTimer = randomizeTimer + Time.deltaTime;

            if (randomizeTimer > 2f)
            {
                randomizeTimer = 0f;

                if (canFight == false)
                {
                    canFight = true;
                }

                if (successRandom == true)
                {
                    successRandom = false;
                }

                isResetRandom = false;
                canRandomize = true;
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
                if (checkDamage == true) // Only apply damage if player is inside attack area
                {
                    gabriellaSystem.TakeHit(15);
                    checkDamage = false;
                }

                Attack(); // Attack if player is inside attack range
            }
            
            if (distanceToTarget > attackRange && isAttacking == false)
            {
                FixWalkAnimDirectionToForward();
                Move(); // Follow player if is outside attack range
            }

            if (distanceToTarget < attackRange && distanceToTarget < attackRange - 1f && isAttacking == false)
            {
                FixWalkAnimDirectionToBackward();
                Move();
            }
        }
    }

    private void Move()
    {
        StartWalkAnimation();

        if (isWalking == true && enemyDifficulty > 0 || isWalking == true && enemyDifficulty == 0 && successRandom == false)
        {
            if (changedAnimDirectionToForward == true || successRandom == true && enemyDifficulty == 2 || successRandom == true && enemyDifficulty == 3)
            {
                transform.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - moveSpeed * Time.deltaTime);
            }
            
            if (changedAnimDirectionToBackward == true || successRandom == true && enemyDifficulty == 1 || successRandom == true && enemyDifficulty == 3)
            {
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
        if (hitEffectPrefab != null)
        {
            hitEffect.SetActive(true);
        }
    }

    public void CharacterFinishedAttack() // Called in the final frame of attack animation
    {
        isAttacking = false; // Reset to allow another attack after cooldown
        isWalking = true;
        canFight = true;
        checkDamage = true;
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
        StartIdleAnimation();
    }

    private void Attack()
    {
        if (isAttacking == false) // Correct way
        {
            isAttacking = true;
            StartAttackAnimation();
        }
    }

    public void IsDead() // Called in the final frame of death animation
    {
        Destroy(gameObject); // Destroy Marcus after to reach the final frame in death animation
    }

    public void CanFight()
    {
        canFight = true;
    }
}
