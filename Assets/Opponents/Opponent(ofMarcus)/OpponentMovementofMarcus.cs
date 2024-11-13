using UnityEngine;

public class OpponentMovementofMarcus : MonoBehaviour
{
    public float moveSpeed = 2f;       // Speed of the opponent
    public float attackRange = 1.5f;   // Range within which the opponent can attack
    public Transform target;            // Reference to the Marcus GameObject
    public int attackDamage = 20;       // Damage dealt by the opponent
    private Animator opponentAnimator;  // Reference to the Animator component

    private int hitCount = 0;           // Counter for how many times Marcus has been hit
    private const int maxHits = 4;      // Number of hits required to trigger the hurt effect

    private void Start()
    {
        // Get the Animator component from the opponent
        opponentAnimator = GetComponent<Animator>();

        // Check if the Animator is available
        if (opponentAnimator == null)
        {
            Debug.LogError("Animator component missing from Opponent! Please attach the Animator component.");
        }

        // Find the target (Marcus) in the scene
        target = GameObject.FindWithTag("Player").transform; // Ensure Marcus has the tag "Player"
    }

    private void Update()
    {
        // Check the distance to the target (Marcus)
        float distanceToTarget = Vector3.Distance(transform.position, target.position);

        if (distanceToTarget > attackRange)
        {
            // Move toward Marcus if not in attack range
            Vector3 direction = (target.position - transform.position).normalized;
            transform.position += direction * moveSpeed * Time.deltaTime;

            // Trigger the OpponentForward animation by setting the Forward parameter
            opponentAnimator.SetBool("IsWalking", true);   // Set walking animation
            opponentAnimator.SetFloat("Forward", 1f);      // Set Forward parameter to trigger OpponentForward animation
        }
        else
        {
            // Stop walking and attack Marcus if in range
            opponentAnimator.SetBool("IsWalking", false);
            opponentAnimator.SetFloat("Forward", 0f);      // Reset Forward parameter when not moving forward
            Attack();  // Perform an attack
        }
    }

    private void Attack()
    {
        // Randomly decide whether to Punch or Kick
        int randomAttack = Random.Range(0, 2); // 0 for Punch, 1 for Kick

        if (randomAttack == 0)
        {
            opponentAnimator.SetTrigger("Punch"); // Trigger Punch animation
            Debug.Log("Opponent performs Combo Punch on Marcus!"); // Debug log
        }
        else
        {
            opponentAnimator.SetTrigger("Kick"); // Trigger Kick animation
            Debug.Log("Opponent performs Combo Kick on Marcus!"); // Debug log
        }

        // Call the TakeDamage method on MarcusMovement
        MarcusMovement marcus = target.GetComponent<MarcusMovement>();
        if (marcus != null)
        {
            hitCount++; // Increment the hit count
            marcus.TakeDamage(attackDamage); // Inflict damage to Marcus

            // Check if the hit count has reached the maximum
            if (hitCount >= maxHits)
            {
                // Display hurt effect (assuming you have a method in MarcusMovement to handle this)
                marcus.DisplayHurtEffect(); 
                hitCount = 0; // Reset hit count after displaying the effect
            }
        }
    }
}
