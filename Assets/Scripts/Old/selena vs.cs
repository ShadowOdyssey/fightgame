using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SelenaMovement : MonoBehaviour
{
    public Button buttonForward;  // Button to move right
    public Button buttonBackward; // Button to move left
    
    public Button skillButton1;   // Button for skill 1
    public Button skillButton2;   // Button for skill 2
    public Button skillButton3;   // Button for skill 3

    public float moveSpeed = 2f;  // Reduced speed of movement for small steps
    private float moveDirection = 0f;  // Direction of movement (right or left)

    public float stepSize = 0.1f; // Set the size of each small step (this can be fine-tuned)

    // Add AnimationClip references for each skill and movement
    public AnimationClip forwardAnimation;
    public AnimationClip backwardAnimation;
    public AnimationClip skill1Animation;
    public AnimationClip skill2Animation;
    public AnimationClip skill3Animation;
    
    private Animator selenaAnimator;  // Reference to the Animator component

    private void Start()
    {
        // Get the Animator component from Selena
        selenaAnimator = GetComponent<Animator>();

        // Check if the Animator is available
        if (selenaAnimator == null)
        {
            Debug.LogError("Animator component missing from Selena! Please attach the Animator component.");
        }

        // Add EventTrigger listeners for forward button
        if (buttonForward != null)
        {
            AddEventTrigger(buttonForward.gameObject, EventTriggerType.PointerDown, (eventData) => { MoveRight(); });
            AddEventTrigger(buttonForward.gameObject, EventTriggerType.PointerUp, (eventData) => { StopMoving(); });
        }
        else
        {
            Debug.LogError("ButtonForward is not assigned in the Inspector!");
        }

        // Add EventTrigger listeners for backward button
        if (buttonBackward != null)
        {
            AddEventTrigger(buttonBackward.gameObject, EventTriggerType.PointerDown, (eventData) => { MoveLeft(); });
            AddEventTrigger(buttonBackward.gameObject, EventTriggerType.PointerUp, (eventData) => { StopMoving(); });
        }
        else
        {
            Debug.LogError("ButtonBackward is not assigned in the Inspector!");
        }

        // Add EventTrigger listeners for skill buttons
        if (skillButton1 != null)
        {
            AddEventTrigger(skillButton1.gameObject, EventTriggerType.PointerDown, (eventData) => { UseSkill1(); });
        }
        else
        {
            Debug.LogError("SkillButton1 is not assigned in the Inspector!");
        }

        if (skillButton2 != null)
        {
            AddEventTrigger(skillButton2.gameObject, EventTriggerType.PointerDown, (eventData) => { UseSkill2(); });
        }
        else
        {
            Debug.LogError("SkillButton2 is not assigned in the Inspector!");
        }

        if (skillButton3 != null)
        {
            AddEventTrigger(skillButton3.gameObject, EventTriggerType.PointerDown, (eventData) => { UseSkill3(); });
        }
        else
        {
            Debug.LogError("SkillButton3 is not assigned in the Inspector!");
        }
    }

    // Update is called once per frame
    private void Update()
    {
        // Move Selena along the X-axis only when the buttons are clicked
        if (moveDirection != 0)
        {
            // Calculate new position with controlled step size
            Vector3 newPosition = transform.position + Vector3.right * moveDirection * stepSize;
            transform.position = newPosition; // Move along the X-axis in small increments
            
            // Update animation states based on movement
            selenaAnimator.SetBool("IsSelenaForward", true); // Set IsSelenaForward to true if moving
            selenaAnimator.SetBool("IsSelenaBackwards", moveDirection < 0); // Set IsSelenaBackwards based on direction
        }
        else
        {
            selenaAnimator.SetBool("IsSelenaForward", false); // Set IsSelenaForward to false if not moving
        }
    }

    // Method to start moving right
    private void MoveRight()
    {
        moveDirection = 1f; // Set movement direction to the right (positive X)
        PlayAnimation(forwardAnimation);
    }

    // Method to start moving left
    private void MoveLeft()
    {
        moveDirection = -1f; // Set movement direction to the left (negative X)
        PlayAnimation(backwardAnimation);
    }

    // Method to stop movement
    private void StopMoving()
    {
        moveDirection = 0f; // Stop movement
        selenaAnimator.SetBool("IsSelenaBackwards", false); // Reset IsSelenaBackwards parameter
    }

    // Skill methods
    private void UseSkill1()
    {
        Debug.Log("Skill 1 used!");
        PlayAnimation(skill1Animation);
    }

    private void UseSkill2()
    {
        Debug.Log("Skill 2 used!");
        PlayAnimation(skill2Animation);
    }

    private void UseSkill3()
    {
        Debug.Log("Skill 3 used!");
        PlayAnimation(skill3Animation);
    }

    // Helper method to play animation clips
    private void PlayAnimation(AnimationClip clip)
    {
        if (clip != null && selenaAnimator != null)
        {
            selenaAnimator.Play(clip.name);
        }
    }

    // Helper method to add event triggers to buttons
    private void AddEventTrigger(GameObject target, EventTriggerType eventType, System.Action<BaseEventData> action)
    {
        EventTrigger trigger = target.GetComponent<EventTrigger>();
        if (trigger == null)
        {
            trigger = target.AddComponent<EventTrigger>();
        }

        EventTrigger.Entry entry = new EventTrigger.Entry { eventID = eventType };
        entry.callback.AddListener((eventData) => action(eventData));
        trigger.triggers.Add(entry);
    }
}
