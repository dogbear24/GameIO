using UnityEngine;
using TMPro;
using System.Collections;

public class SceneStartMessage : MonoBehaviour
{
    [Header("References")]
    public TextMeshProUGUI messageText;

    [Header("Message Settings")]
    [TextArea] public string message1 = "Welcome!";
    [TextArea] public string message2 = "Enjoy your adventure!";
    public float fadeDuration = 1f;
    public float displayTime = 2f;
    public bool disableAfter = true;

    private void Start()
    {
        if (messageText == null)
        {
            Debug.LogWarning("SceneStartTwoMessages: No TextMeshProUGUI assigned!");
            return;
        }

        // Ensure TMP object is active
        messageText.gameObject.SetActive(true);

        // Set alpha to 0 initially
        SetAlpha(0f);

        // Wait one frame to ensure TMP is fully initialized
        StartCoroutine(StartMessagesNextFrame());
    }

    private IEnumerator StartMessagesNextFrame()
    {
        yield return null; // wait one frame
        StartCoroutine(ShowMessages());
    }

    private IEnumerator ShowMessages()
    {
        // Fade first message
        yield return StartCoroutine(FadeMessage(message1));

        // Fade second message
        yield return StartCoroutine(FadeMessage(message2));

        if (disableAfter)
            messageText.gameObject.SetActive(false);
    }

    private IEnumerator FadeMessage(string msg)
    {
        messageText.text = msg;

        Color c = messageText.color;
        c.a = 0f;
        messageText.color = c;

        messageText.gameObject.SetActive(true);

        float t = 0f;

        // Fade In
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

    private void SetAlpha(float alpha)
    {
        if (messageText != null)
        {
            Color c = messageText.color;
            c.a = alpha;
            messageText.color = c;
        }
    }
}