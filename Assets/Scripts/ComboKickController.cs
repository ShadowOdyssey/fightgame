using UnityEngine;

public class ComboKickController : MonoBehaviour
{
    // Reference to the animator component
    public Animator animator;

    // Function to be called when the combo kick button is clicked
    public void OnComboKickButtonClick()
    {
        // Trigger the femcombokick animation
        animator.SetTrigger("femcombokick");
    }
}
