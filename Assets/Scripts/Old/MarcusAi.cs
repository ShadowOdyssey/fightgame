using UnityEngine;

public class MarcusAI : MonoBehaviour
{
    public RoundManager roundSystem;

    public GameObject hitEffectPrefab; // Prefab for visual effect on hit
    private Animator animator;
    public Transform target; // Gabriella's transform
    public int health = 100;
    public float detectionRange = 5f;
    public float attackRange = 19f;
    public float moveSpeed = 2f;
    public float stopDistance = 1.5f; // Minimum distance to maintain between Marcus and Gabriella

    private Vector3 originalPosition; // Save original position to reset after getting up
    private int hitCount = 0; // Track the number of times Marcus is hit
    private float distanceToTarget = 0f;
    private bool isAttacking = false;
    private bool canFight = false;

    private void Start()
    {
        distanceToTarget = Vector3.Distance(transform.position, target.position);

        // Get the Animator component
        animator = GetComponent<Animator>();

        // Assume Gabriella is tagged as "Gabriella" in the scene
        //target = GameObject.FindGameObjectWithTag("Gabriella").transform; // Dont need it since the variable is public and you attached Gabriella object by inspector - Felipe

        // Save the original position
        originalPosition = transform.localPosition;
    }

    private void Update()
    {
        if (canFight == false && roundSystem.roundStarted == true)
        {
            Debug.Log("Round Started");

            canFight = true;
        }

        if (canFight == true && roundSystem.roundStarted == false)
        {
            Idle();
            canFight = false;
        }
    }

    private void FixedUpdate()
    {
        if (health <= 0) return; // Prevent further actions if Marcus is dead

        if (canFight == true)
        {
            distanceToTarget = Vector3.Distance(transform.position, target.position);

            Debug.Log("Actual distance to target from Marcus is: " + distanceToTarget);

            if (distanceToTarget < attackRange)
            {
                Attack();
            }
            else
            {
                Chase();
            }
        }
    }

    private void Idle()
    {
        StartIdleAnimation();
    }

    private void Chase()
    {
        StartWalkAnimation();

        Vector3 direction = (target.localPosition - transform.localPosition).normalized;
        transform.localPosition += direction * moveSpeed * Time.deltaTime;

        // Face Gabriella
        transform.LookAt(new Vector3(target.localPosition.x, transform.localPosition.y, target.localPosition.z)); // We dont need it since characters only move forward and backward, them are always facing each other
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

    private void ShowHitEffect()
    {
        if (hitEffectPrefab != null)
        {
            GameObject hitEffect = Instantiate(hitEffectPrefab, transform.localPosition, Quaternion.identity);
            Destroy(hitEffect, 0.5f); // Remove effect after a short delay
        }
    }

    private void StartIdleAnimation()
    {
        animator.SetBool("isIdle", true); // Values in parameters should be low case in the first letter because is variable name - Felipe
        animator.SetBool("isWalking", false); // Values in parameters should be low case in the first letter because is variable name - Felipe
        animator.SetBool("isAttacking", false); // Values in parameters should be low case in the first letter because is variable name - Felipe
        isAttacking = false;
    }

    private void StartAttackAnimation()
    {
        animator.SetBool("isIdle", false); // Values in parameters should be low case in the first letter because is variable name - Felipe
        animator.SetBool("isWalking", false); // Values in parameters should be low case in the first letter because is variable name - Felipe
        animator.SetBool("isAttacking", true); // Values in parameters should be low case in the first letter because is variable name - Felipe
        isAttacking = true;
    }

    private void StartWalkAnimation()
    {
        animator.SetBool("isIdle", false); // Values in parameters should be low case in the first letter because is variable name - Felipe
        animator.SetBool("isWalking", true); // Values in parameters should be low case in the first letter because is variable name - Felipe
        animator.SetBool("isAttacking", false); // Values in parameters should be low case in the first letter because is variable name - Felipe
    }

    private void Attack()
    {
        if (!isAttacking) // AVOID .NET WAY
        {
            StartAttackAnimation();
        }

        if (isAttacking == false) // Correct way
        {
            StartAttackAnimation();
        }
    }

    private void Die()
    {
        animator.SetTrigger("die"); // Values in parameters should be low case in the first letter because is variable name - Felipe
        Destroy(gameObject, 2f); // Destroy Marcus after delay to allow death animation
    }

    public void CharacterFinishedAttack()
    {
        isAttacking = false; // Reset to allow another attack after cooldown
    }

    public void CanFight()
    {
        canFight = true;
    }
}
