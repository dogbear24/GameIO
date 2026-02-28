using UnityEngine;
using TMPro;
using System.Collections;
using System.Reflection;

[System.Serializable]
public class ScriptBoolPair
{
    public MonoBehaviour script;  // Script to check
    public string boolName;       // Boolean variable name
}

public class Finish : MonoBehaviour
{
    [Header("Check Settings")]
    public ScriptBoolPair[] checkScripts; // Array of scripts & bools to check

    [Header("Message Settings")]
    public TextMeshProUGUI messageText;
    [TextArea] public string message = "All tasks complete!";
    public float fadeDuration = 0.5f;
    public float displayTime = 2f;

    [Header("Set Other Script Variable")]
    public MonoBehaviour targetScript;
    public string targetBoolName;

    private bool hasTriggered = false;
    private FieldInfo targetField;

    void Start()
    {
        if (targetScript != null && !string.IsNullOrEmpty(targetBoolName))
        {
            targetField = targetScript.GetType().GetField(targetBoolName, BindingFlags.Public | BindingFlags.Instance);
            if (targetField == null)
                Debug.LogWarning($"Target field '{targetBoolName}' not found on {targetScript.GetType().Name}");
        }

        if (messageText != null)
            messageText.gameObject.SetActive(false);
    }

    void Update()
    {
        if (hasTriggered)
            return;

        bool allTrue = true;

        // Check all scripts
        foreach (var pair in checkScripts)
        {
            if (pair.script == null || string.IsNullOrEmpty(pair.boolName))
            {
                allTrue = false;
                break;
            }

            var field = pair.script.GetType().GetField(pair.boolName, BindingFlags.Public | BindingFlags.Instance);
            if (field == null)
            {
                Debug.LogWarning($"Field '{pair.boolName}' not found on {pair.script.GetType().Name}");
                allTrue = false;
                break;
            }

            object value = field.GetValue(pair.script);
            if (!(value is bool b) || !b)
            {
                allTrue = false;
                break;
            }
        }

        if (allTrue)
        {
            hasTriggered = true;

            // Set target variable
            if (targetField != null)
                targetField.SetValue(targetScript, true);

            // Show message
            if (messageText != null)
                StartCoroutine(FadeMessageRoutine(message));
        }
    }

    private IEnumerator FadeMessageRoutine(string msg)
    {
        messageText.text = msg;
        messageText.gameObject.SetActive(true);

        Color c = messageText.color;
        c.a = 0f;
        messageText.color = c;

        float t = 0f;
        // Fade in
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            c.a = Mathf.Lerp(0f, 1f, t / fadeDuration);
            messageText.color = c;
            yield return null;
        }

        yield return new WaitForSeconds(displayTime);

        t = 0f;
        // Fade out
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            c.a = Mathf.Lerp(1f, 0f, t / fadeDuration);
            messageText.color = c;
            yield return null;
        }

        messageText.gameObject.SetActive(false);
    }
}