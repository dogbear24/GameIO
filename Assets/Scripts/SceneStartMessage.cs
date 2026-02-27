using UnityEngine;
using TMPro;
using System.Collections;

public class SceneStartMessage : MonoBehaviour
{
    [Header("References")]
    public TextMeshProUGUI messageText;

    [Header("Message Settings")]
    [TextArea] public string message = "Welcome!";
    public float fadeDuration = 1f;
    public float displayTime = 2f;
    public bool disableAfter = true;

    private void Start()
    {
        StartCoroutine(ShowMessage());
    }

    private IEnumerator ShowMessage()
    {
        messageText.gameObject.SetActive(true);
        messageText.text = message;

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

        if (disableAfter)
            messageText.gameObject.SetActive(false);
    }
}