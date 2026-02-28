using UnityEngine;
using TMPro;
using System.Collections;
using System.Reflection;

public class Tutorial : MonoBehaviour
{
    [Header("Tilemap To Hide")]
    public GameObject tilemapToHide;

    [Header("Trigger Settings")]
    public string triggerTag = "SpecialTrigger";

    [Header("UI Message")]
    public TextMeshProUGUI messageText;
    public float fadeDuration = 0.5f;
    public float displayTime = 2f;

    [TextArea] public string normalTriggerMessage = "You found something interesting!";
    [TextArea] public string completedTriggerMessage = "Now that the task is complete, something has changed...";

    [Header("Task Check")]
    public MonoBehaviour taskScript; // Drag the specific script you want to check
    public string boolFieldName;      // Name of the public bool variable inside that script

    private bool hasHidden = false;
    private bool completedMessageShown = false;
    private Coroutine messageRoutine;
    private FieldInfo boolField;

    void Start()
    {
        // Get the field from the specified script type
        if (taskScript != null && !string.IsNullOrEmpty(boolFieldName))
        {
            boolField = taskScript.GetType().GetField(boolFieldName, BindingFlags.Public | BindingFlags.Instance);
            if (boolField == null)
                Debug.LogWarning($"Field '{boolFieldName}' not found on {taskScript.GetType().Name}");
        }

        if (messageText != null)
            messageText.gameObject.SetActive(false);
    }

    void Update()
    {
        // Check the boolean value dynamically
        bool isComplete = false;
        if (boolField != null)
        {
            object value = boolField.GetValue(taskScript);
            if (value is bool b)
                isComplete = b;
        }

        // Hide tilemap once the task is complete
        if (!hasHidden && isComplete)
        {
            hasHidden = true;
            if (tilemapToHide != null)
                tilemapToHide.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag(triggerTag))
            return;

        bool isComplete = false;
        if (boolField != null)
        {
            object value = boolField.GetValue(taskScript);
            if (value is bool b)
                isComplete = b;
        }

        if (isComplete)
        {
            if (!completedMessageShown)
            {
                completedMessageShown = true;

                if (messageRoutine != null)
                    StopCoroutine(messageRoutine);

                messageRoutine = StartCoroutine(FadeMessageRoutine(completedTriggerMessage));
            }
        }
        else
        {
            if (messageRoutine != null)
                StopCoroutine(messageRoutine);

            messageRoutine = StartCoroutine(FadeMessageRoutine(normalTriggerMessage));
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