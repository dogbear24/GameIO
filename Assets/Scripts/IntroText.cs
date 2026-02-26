using UnityEngine;
using TMPro;
using System.Collections;

public class IntroText : MonoBehaviour
{
    [Header("Message Settings")]
    public TextMeshProUGUI messageText; // Assign your UI TextMeshPro object
    [TextArea] public string message = "Hello!";
    public float fadeDuration = 1f;
    public float displayTime = 2f; // How long to keep the text before fading out

    [Header("Trigger Settings")]
    public string triggerTag = "intro"; // Tag of the tilemap trigger
    public bool onlyOnce = true;            // Trigger only once

    private bool hasTriggered = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (hasTriggered && onlyOnce) return;

        if (other.CompareTag(triggerTag))
        {
            hasTriggered = true;
            StartCoroutine(FadeTextRoutine());
        }
    }

    private IEnumerator FadeTextRoutine()
    {
        // Ensure text is active and transparent
        messageText.gameObject.SetActive(true);
        messageText.text = message;

        Color c = messageText.color;
        c.a = 0f;
        messageText.color = c;

        // Fade in
        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            c.a = Mathf.Lerp(0f, 1f, t / fadeDuration);
            messageText.color = c;
            yield return null;
        }

        // Ensure fully visible
        c.a = 1f;
        messageText.color = c;

        // Wait for displayTime
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

        // Make fully invisible at the end
        c.a = 0f;
        messageText.color = c;
        messageText.gameObject.SetActive(false);
    }
}