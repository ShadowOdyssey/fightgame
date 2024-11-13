using UnityEngine;

public class ComboPunchController : MonoBehaviour
{
    // Reference to the animator component
    public Animator animator;

    // Function to be called when the combo punch button is clicked
    public void OnComboPunchButtonClick()
    {
        // Trigger the femcombopunch animation
        animator.SetTrigger("femcombopunch");
    }
}
