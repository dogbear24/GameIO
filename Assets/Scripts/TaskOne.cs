using UnityEngine;
using TMPro;
using System.Collections;

public class TaskOne : MonoBehaviour
{
    [Header("References")]
    public SimpleSpriteAnimator[] animators;
    public TextMeshProUGUI messageText;

    [Header("Settings")]
    public string interactableTag = "TilemapEvent";

    [TextArea] public string interactPrompt = "Press SPACE to interact";
    [TextArea] public string message1 = "Tilemaps hidden!";
    [TextArea] public string message2 = "Task One complete!";

    public float fadeDuration = 1f;
    public float displayTime = 2f;

    private bool nearInteractable = false;
    public bool taskOneComplete = false;

    private Coroutine messageRoutine;
    private Coroutine promptRoutine;

    void Start()
    {
        messageText.gameObject.SetActive(false);
    }

    void Update()
    {
        if (!taskOneComplete && nearInteractable)
        {
            // If player presses space
            if (Input.GetKeyDown(KeyCode.Space))
            {
                taskOneComplete = true;

                foreach (var animator in animators)
                {
                    animator.StopAndHide();
                }

                if (promptRoutine != null)
                    StopCoroutine(promptRoutine);

                if (messageRoutine != null)
                    StopCoroutine(messageRoutine);

                messageRoutine = StartCoroutine(ShowMessagesRoutine());
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(interactableTag) && !taskOneComplete)
        {
            nearInteractable = true;

            if (promptRoutine != null)
                StopCoroutine(promptRoutine);

            promptRoutine = StartCoroutine(FadePrompt(true));
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag(interactableTag) && !taskOneComplete)
        {
            nearInteractable = false;

            if (promptRoutine != null)
                StopCoroutine(promptRoutine);

            promptRoutine = StartCoroutine(FadePrompt(false));
        }
    }

    private IEnumerator FadePrompt(bool fadeIn)
    {
        messageText.gameObject.SetActive(true);
        messageText.text = interactPrompt;

        Color c = messageText.color;
        float start = fadeIn ? 0f : 1f;
        float end = fadeIn ? 1f : 0f;

        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            c.a = Mathf.Lerp(start, end, t / fadeDuration);
            messageText.color = c;
            yield return null;
        }

        c.a = end;
        messageText.color = c;

        if (!fadeIn)
            messageText.gameObject.SetActive(false);
    }

    private IEnumerator ShowMessagesRoutine()
    {
        yield return StartCoroutine(FadeMessage(message1));
        yield return StartCoroutine(FadeMessage(message2));

        messageText.gameObject.SetActive(false);
    }

    private IEnumerator FadeMessage(string msg)
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

        // Fade out
        t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            c.a = Mathf.Lerp(1f, 0f, t / fadeDuration);
            messageText.color = c;
            yield return null;
        }
    }
}