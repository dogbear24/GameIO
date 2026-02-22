using UnityEngine;

public class TriggerAnimationOnPosition : MonoBehaviour
{
    [Header("Animator Settings")]
    public Animator animator;             // Assign your character's Animator here
    public string animationTrigger = "Playwalk"; // Name of the trigger in Animator

    [Header("Position Settings")]
    public float zThreshold = -414.5f;      // Z position at which to trigger animation

    private bool triggered = false;       // Make sure it only triggers once

    void Update()
    {
        if (animator == null) return;

        // Trigger animation once if past the threshold
        if (!triggered && transform.position.z < zThreshold)
        {
            animator.SetTrigger(animationTrigger);
            triggered = true;
        }
    }
}