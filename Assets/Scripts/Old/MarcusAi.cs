using UnityEngine;

public class MarcusAI : MonoBehaviour
{
    public RoundManager roundSystem;
    public GabriellaMovementPlay gabriellaSystem;

    public GameObject hitEffectPrefab; // Prefab for visual effect on hit
    public Transform target; // Gabriella's transform

    public Animator animator;

    public int health = 100;
    public float attackRange = 13f;
    public float moveSpeed = 2f;

    private Vector3 originalPosition; // Save original position to reset after getting up
    private int hitCount = 0; // Track the number of times Marcus is hit
    private float distanceToTarget = 0f;
    private bool isWalking = false;
    private bool isAttacking = false;
    private bool canFight = false;
    private bool checkDamage = false;
    private bool changedAnimDirectionToForward = false;
    private bool changedAnimDirectionToBackward = false;

    private void Start()
    {
        distanceToTarget = Vector3.Distance(transform.position, target.position); // Get initial position from Gabriella to get the first distance measure only once
    }

    private void Update()
    {
        if (canFight == false && roundSystem.roundStarted == true) // Only can execute commands in FixedUpdate if round started, but...
        {
            Debug.Log("Round Started");

            canFight = true;
        }

        if (canFight == true && roundSystem.roundStarted == false) // If round not started and is 2nd or 3rd round, load Idle animation till round start again!
        {
            Idle();
            canFight = false;
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

    private void Idle()
    {
        StartIdleAnimation();
    }

    private void Move()
    {
        StartWalkAnimation();

        if (isWalking == true)
        {
            if (changedAnimDirectionToForward == true)
            {
                transform.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - moveSpeed * Time.deltaTime);
            }
            
            if (changedAnimDirectionToBackward == true)
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
            hitCount++; // Increment hit count

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
        if (animator.GetBool("isForward") == false && changedAnimDirectionToForward == true) // Prevents to execute animation call many times, this way we only call 1 time the correct animation
        {
            animator.SetBool("isIdle", false); // Values in parameters should be low case in the first letter because is variable name - Felipe
            animator.SetBool("isForward", true); // Values in parameters should be low case in the first letter because is variable name - Felipe
            animator.SetBool("isBackward", false); // Values in parameters should be low case in the first letter because is variable name - Felipe
            animator.SetBool("isAttacking", false); // Values in parameters should be low case in the first letter because is variable name - Felipe
            isWalking = true;
        }

        if (animator.GetBool("isBackward") == false && changedAnimDirectionToBackward == true) // Prevents to execute animation call many times, this way we only call 1 time the correct animation
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
            isAttacking = true;
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
            GameObject hitEffect = Instantiate(hitEffectPrefab, transform.localPosition, Quaternion.identity);
            Destroy(hitEffect, 0.5f); // Remove effect after a short delay
        }
    }

    private void Attack()
    {
        if (isAttacking == false) // Correct way
        {
            StartAttackAnimation();
        }
    }

    private void Die()
    {
        if (animator.GetBool("isDead") == false) // Prevents to execute animation call many times, this way we only call 1 time the correct animation
        {
            animator.SetBool("isDead", true); // Values in parameters should be low case in the first letter because is variable name - Felipe
        }
    }

    public void CharacterFinishedAttack() // Called in the final frame of attack animation
    {
        isAttacking = false; // Reset to allow another attack after cooldown
        checkDamage = true;
        StartIdleAnimation(); // Reset animation to repeat the attack if player is inside range yet
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
