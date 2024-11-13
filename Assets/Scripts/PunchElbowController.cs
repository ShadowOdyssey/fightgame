using UnityEngine;

public class PunchElbowController : MonoBehaviour
{
    // Reference to the animator component
    public Animator animator;

    // Function to be called when the combo punch elbow button is clicked
    public void OnComboPunchElbowButtonClick()
    {
        // Trigger the fempunchelbow animation
        animator.SetTrigger("fempunchelbow");
    }
}
