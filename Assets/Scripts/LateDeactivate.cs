using UnityEngine;
using System.Reflection;

public class LateDeactivate : MonoBehaviour
{
    [Header("Check Settings")]
    public MonoBehaviour scriptToCheck;   // Script containing the bool
    public string boolFieldName;          // Name of the boolean variable

    [Header("Animations to Deactivate")]
    public SimpleSpriteAnimator[] animatorsToDeactivate;

    private FieldInfo boolField;
    private bool hasTriggered = false;

    void Start()
    {
        // Setup reflection
        if (scriptToCheck != null && !string.IsNullOrEmpty(boolFieldName))
        {
            boolField = scriptToCheck.GetType().GetField(
                boolFieldName,
                BindingFlags.Public | BindingFlags.Instance
            );

            if (boolField == null)
            {
                Debug.LogWarning(
                    $"Field '{boolFieldName}' not found on {scriptToCheck.GetType().Name}"
                );
            }
        }
        else
        {
            Debug.LogWarning("DeactivateAnimationsOnBool: Script or field name not set!");
        }
    }

    void Update()
    {
        if (hasTriggered || boolField == null || scriptToCheck == null)
            return;

        object value = boolField.GetValue(scriptToCheck);

        if (value is bool b && b)
        {
            hasTriggered = true;

            if (animatorsToDeactivate != null)
            {
                foreach (var anim in animatorsToDeactivate)
                {
                    if (anim != null)
                        anim.StopAndHide();
                }
            }
        }
    }
}