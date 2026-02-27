using UnityEngine;
using TMPro;
using System.Collections;

public class RedirectMessage : MonoBehaviour
{
    [Header("Message Settings")]
    public TextMeshProUGUI messageText;

    [TextArea] public string message1 = "Hello!";
    [TextArea] public string message2 = "Welcome to the game!";

    public float fadeDuration = 1f;
    public float displayTime = 2f;

    [Header("Trigger Settings")]
    public string triggerTag = "intro";
    public bool onlyOnce = true;

    private bool hasTriggered = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (hasTriggered && onlyOnce) return;

        if (other.CompareTag(triggerTag))
        {
            hasTriggered = true;
            StartCoroutine(MessageSequence());
        }
    }

    private IEnumerator MessageSequence()
    {
        messageText.gameObject.SetActive(true);

        // Show first message
        yield return StartCoroutine(FadeMessage(message1));

        // Show second message
        yield return StartCoroutine(FadeMessage(message2));

        messageText.gameObject.SetActive(false);
    }

    private IEnumerator FadeMessage(string msg)
    {
        messageText.text = msg;

        Color c = messageText.color;
        c.a = 0f;
        messageText.color = c;

        // Fade In
        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            c.a = Mathf.Lerp(0f, 1f, t / fadeDuration);
            messageText.color = c;
            yield return null;
        }

        c.a = 1f;
        messageText.color = c;

        // Hold
        yield return new WaitForSeconds(displayTime);

        // Fade Out
        t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            c.a = Mathf.Lerp(1f, 0f, t / fadeDuration);
            messageText.color = c;
            yield return null;
        }

        c.a = 0f;
        messageText.color = c;
    }
}