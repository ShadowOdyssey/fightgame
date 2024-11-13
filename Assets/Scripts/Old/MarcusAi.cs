using UnityEngine;

public class MarcusAI : MonoBehaviour
{
    public Transform target; // Gabriella's transform
    public float detectionRange = 5f;
    public float attackRange = 2f;
    public float moveSpeed = 2f;
    public int health = 100;

    private Animator animator;
    private bool isAttacking = false;

    public GameObject hitEffectPrefab; // Prefab for visual effect on hit
    public float stopDistance = 1.5f; // Minimum distance to maintain between Marcus and Gabriella

    private int hitCount = 0; // Track the number of times Marcus is hit
    private Vector3 originalPosition; // Save original position to reset after getting up

    private void Start()
    {
        // Get the Animator component
        animator = GetComponent<Animator>();

        // Assume Gabriella is tagged as "Gabriella" in the scene
        //target = GameObject.FindGameObjectWithTag("Gabriella").transform; // Dont need it since the variable is public and you attached Gabriella object by inspector - Felipe

        // Save the original position
        originalPosition = transform.localPosition;
    }

    private void Update()
    {
        if (health <= 0) return; // Prevent further actions if Marcus is dead

        float distanceToTarget = Vector3.Distance(transform.localPosition, target.localPosition);

        Debug.Log("Actual distance to target from Marcus is: " + distanceToTarget);

        if (distanceToTarget <= attackRange)
        {
            Attack();
        }
        else if (distanceToTarget <= detectionRange && distanceToTarget > stopDistance)
        {
            Chase();
        }
        else
        {
            Idle();
        }
    }

    private void Idle()
    {
        animator.SetBool("isWalking", false); // Values in parameters should be low case in the first letter because is variable name - Felipe
        isAttacking = false;
    }

    private void Chase()
    {
        animator.SetBool("isWalking", true); // Values in parameters should be low case in the first letter because is variable name - Felipe
        isAttacking = false;

        Vector3 direction = (target.localPosition - transform.localPosition).normalized;
        transform.localPosition += direction * moveSpeed * Time.deltaTime;

        // Face Gabriella
        transform.LookAt(new Vector3(target.localPosition.x, transform.localPosition.y, target.localPosition.z)); // We dont need it since characters only move forward and backward, them are always facing each other
    }

    private void Attack()
    {
        if (!isAttacking)
        {
            isAttacking = true;
            animator.SetTrigger("isBoxing"); // Trigger attack animation - Values in parameters should be low case in the first letter because is variable name - Felipe

            StartCoroutine(AttackCoroutine());
        }
    }

    private System.Collections.IEnumerator AttackCoroutine()
    {
        yield return new WaitForSeconds(1.0f); // Assume 1-second attack cooldown
        isAttacking = false; // Reset to allow another attack after cooldown
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

    private void Die()
    {
        animator.SetTrigger("die"); // Values in parameters should be low case in the first letter because is variable name - Felipe
        Destroy(gameObject, 2f); // Destroy Marcus after delay to allow death animation
    }
}
