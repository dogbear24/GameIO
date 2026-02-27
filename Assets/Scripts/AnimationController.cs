using UnityEngine;

public class TriggerAnimationOnPosition : MonoBehaviour
{
    [Header("Animator Settings")]
    public Animator animator;          
    public string animationTrigger = "Playwalk"; 

    [Header("Position Settings")]
    public float zThreshold = -414.5f;

    [Header("Control")]
    public bool canTrigger = true;

    private bool triggered = false;      

    void Update()
    {
        if (!canTrigger) return;
        if (animator == null) return;

        if (!triggered && transform.position.z < zThreshold)
        {
            animator.SetTrigger(animationTrigger);
            triggered = true;
        }
    }
}