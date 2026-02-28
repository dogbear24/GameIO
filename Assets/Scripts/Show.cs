using UnityEngine;
using System.Reflection;

public class Show : MonoBehaviour
{
    [Header("Check Settings")]
    public MonoBehaviour scriptToCheck;   // The script containing the bool
    public string boolFieldName;          // Name of the boolean variable to watch

    [Header("Tilemap to Activate")]
    public GameObject tilemapToActivate;  // Tilemap to activate once true

    private bool hasTriggered = false;    // Ensures it only triggers once
    private FieldInfo boolField;          // Reflection reference to the bool field

    void Start()
    {
        if (scriptToCheck != null && !string.IsNullOrEmpty(boolFieldName))
        {
            // Use reflection to get the field
            boolField = scriptToCheck.GetType().GetField(boolFieldName, 
                BindingFlags.Public | BindingFlags.Instance);

            if (boolField == null)
            {
                Debug.LogWarning($"Field '{boolFieldName}' not found on {scriptToCheck.GetType().Name}");
            }
        }
        else
        {
            Debug.LogWarning("ActivateTilemapOnBool: scriptToCheck or boolFieldName is not set!");
        }

        // Optionally hide the tilemap initially
        if (tilemapToActivate != null)
            tilemapToActivate.SetActive(false);
    }

    void Update()
    {
        if (hasTriggered || boolField == null || scriptToCheck == null)
            return;

        // Read the boolean value
        object value = boolField.GetValue(scriptToCheck);

        if (value is bool b && b)
        {
            hasTriggered = true;

            // Activate the tilemap
            if (tilemapToActivate != null)
                tilemapToActivate.SetActive(true);
        }
    }
}