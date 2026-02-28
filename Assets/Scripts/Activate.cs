using UnityEngine;
using System.Reflection;

public class Activate : MonoBehaviour
{
    [Header("Check Settings")]
    public MonoBehaviour checkScript;    // Script to monitor
    public string boolFieldName;         // Boolean variable to watch

    [Header("Animations to Activate")]
    public SimpleSpriteAnimator[] animatorsToActivate;

    private bool hasTriggered = false;
    private FieldInfo boolField;

    void Start()
    {
        if (checkScript != null && !string.IsNullOrEmpty(boolFieldName))
        {
            boolField = checkScript.GetType().GetField(boolFieldName, BindingFlags.Public | BindingFlags.Instance);
            if (boolField == null)
                Debug.LogWarning($"Field '{boolFieldName}' not found on {checkScript.GetType().Name}");
        }
        else
        {
            Debug.LogWarning("CheckScript or BoolFieldName is not set!");
        }
    }

    void Update()
    {
        if (hasTriggered || boolField == null)
            return;

        // Check the boolean value
        object value = boolField.GetValue(checkScript);
        if (value is bool b && b)
        {
            hasTriggered = true;

            // Activate and restart all specified animations
            if (animatorsToActivate != null)
            {
                foreach (var anim in animatorsToActivate)
                {
                    if (anim != null)
                    {
                        anim.gameObject.SetActive(true); // Reactivate GameObject
                        anim.Play();                     // Restart the animation
                    }
                }
            }
        }
    }
}