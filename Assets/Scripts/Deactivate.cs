using UnityEngine;

public class Deactivate : MonoBehaviour
{
    [Header("Animations to Deactivate")]
    public SimpleSpriteAnimator[] animatorsToDeactivate;

    private void Start()
    {
        if (animatorsToDeactivate == null || animatorsToDeactivate.Length == 0)
            return;

        foreach (var animator in animatorsToDeactivate)
        {
            if (animator != null)
            {
                animator.StopAndHide(); // stops the animation and hides it
            }
        }
    }
}