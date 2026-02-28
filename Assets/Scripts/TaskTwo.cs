using UnityEngine;
using System.Collections;
using TMPro;

public class TaskTwo : MonoBehaviour
{
    [Header("Tilemaps & Animators")]
    public GameObject tilemapToDeactivate;
    public GameObject tilemapToActivate;
    public SimpleSpriteAnimator[] animatorsToStop;

    [Header("Trigger Settings")]
    public string triggerTag = "TaskTwoTrigger";

    [Header("Task Dependency")]
    public TaskOne taskOneComponent;

    [Header("UI Messages")]
    public TextMeshProUGUI messageText;
    public string incompleteMessage;    // Leave blank for now
    public string completionMessage;    // Leave blank for now
    public float messageDuration = 2f;
    public float fadeDuration = 0.5f;

    [Header("Task Status")]
    public bool taskOneComplete = false;  // This gets set to true when TaskTwo is completed

    private bool nearTrigger = false;
    private bool switched = false;
    private bool completionMessageShown = false;

    private void Start()
    {
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
            if (taskOneComponent != null && taskOneComponent.taskOneComplete)
            {
                switched = true;
                taskOneComplete = true; // <-- Set this to true when the task is completed

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

                // Show completion message once if assigned
                if (!completionMessageShown && !string.IsNullOrEmpty(completionMessage))
                {
                    completionMessageShown = true;
                    ShowMessage(completionMessage);
                }
            }
            else
            {
                // Show incomplete message if assigned
                if (!string.IsNullOrEmpty(incompleteMessage))
                {
                    ShowMessage(incompleteMessage);
                }
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

        // Fade In
        yield return StartCoroutine(Fade(0f, 1f));

        // Wait
        yield return new WaitForSeconds(messageDuration);

        // Fade Out
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
        {
            nearTrigger = true;

            if (taskOneComponent == null)
            {
                taskOneComponent = other.GetComponent<TaskOne>();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag(triggerTag))
        {
            nearTrigger = false;
        }
    }
}