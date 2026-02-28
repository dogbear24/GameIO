using UnityEngine;
using System.Collections;
using System.Reflection;
using TMPro;

public class TaskFour : MonoBehaviour
{
    [Header("Tilemaps & Animators")]
    public GameObject tilemapToDeactivate;
    public GameObject tilemapToActivate;
    public SimpleSpriteAnimator[] animatorsToStop;

    [Header("Trigger Settings")]
    public string triggerTag = "TaskTwoTrigger";

    [Header("Task Dependency (Dynamic)")]
    public MonoBehaviour dependencyScript;   
    public string boolFieldName;             

    [Header("UI Messages")]
    public TextMeshProUGUI messageText;
    public string incompleteMessage;
    public string completionMessage;
    public float messageDuration = 2f;
    public float fadeDuration = 0.5f;

    [Header("Task Status")]
    public bool taskComplete = false;
    public bool taskOneComplete = false;   // NEW variable

    private bool nearTrigger = false;
    private bool switched = false;
    private bool completionMessageShown = false;

    private FieldInfo boolField;

    private void Start()
    {
        // Setup reflection
        if (dependencyScript != null && !string.IsNullOrEmpty(boolFieldName))
        {
            boolField = dependencyScript.GetType().GetField(
                boolFieldName,
                BindingFlags.Public | BindingFlags.Instance
            );

            if (boolField == null)
                Debug.LogWarning($"Field '{boolFieldName}' not found on {dependencyScript.GetType().Name}");
        }
        else
        {
            Debug.LogWarning("Dependency script or boolFieldName not set!");
        }

        if (messageText != null)
        {
            SetAlpha(0f);
            messageText.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (nearTrigger && !switched && Input.GetKeyDown(KeyCode.Space))
        {
            bool dependencyMet = false;

            if (boolField != null && dependencyScript != null)
            {
                object value = boolField.GetValue(dependencyScript);
                if (value is bool b && b)
                    dependencyMet = true;
            }

            if (dependencyMet)
            {
                switched = true;

                // Mark task as complete
                taskComplete = true;
                taskOneComplete = true;   // <-- Now gets set here

                // Stop animators
                if (animatorsToStop != null)
                {
                    foreach (var anim in animatorsToStop)
                    {
                        if (anim != null)
                            anim.StopAndHide();
                    }
                }

                // Switch tilemaps
                if (tilemapToDeactivate != null)
                    tilemapToDeactivate.SetActive(false);

                if (tilemapToActivate != null)
                    tilemapToActivate.SetActive(true);

                // Show completion message once
                if (!completionMessageShown && !string.IsNullOrEmpty(completionMessage))
                {
                    completionMessageShown = true;
                    ShowMessage(completionMessage);
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(incompleteMessage))
                    ShowMessage(incompleteMessage);
            }
        }
    }

    private void ShowMessage(string msg)
    {
        if (messageText != null)
        {
            StopAllCoroutines();
            messageText.text = msg;
            StartCoroutine(FadeMessageRoutine());
        }
    }

    private IEnumerator FadeMessageRoutine()
    {
        messageText.gameObject.SetActive(true);

        yield return StartCoroutine(Fade(0f, 1f));
        yield return new WaitForSeconds(messageDuration);
        yield return StartCoroutine(Fade(1f, 0f));

        messageText.gameObject.SetActive(false);
    }

    private IEnumerator Fade(float start, float end)
    {
        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(start, end, elapsed / fadeDuration);
            SetAlpha(alpha);
            yield return null;
        }

        SetAlpha(end);
    }

    private void SetAlpha(float alpha)
    {
        if (messageText != null)
        {
            Color c = messageText.color;
            c.a = alpha;
            messageText.color = c;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(triggerTag))
            nearTrigger = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag(triggerTag))
            nearTrigger = false;
    }
}