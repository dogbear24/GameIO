using UnityEngine;
using TMPro;
using System.Collections;

public class FragOne : MonoBehaviour
{
    [Header("References")]
    public SimpleSpriteAnimator[] animators; // Drag both tilemap animators here
    public TextMeshProUGUI messageText;      // Drag a TMP Text object here
    public SceneTransition sceneTransition;  // Drag the SceneTransition component here

    [Header("Settings")]
    public string interactableTag = "TilemapEvent";
    [TextArea] public string message1 = "Tilemaps hidden!";
    [TextArea] public string message2 = "Task One complete!";
    public float fadeDuration = 1f;
    public float displayTime = 2f;

    private bool nearInteractable = false;
    public bool taskOneComplete = false; // Only trigger once

    void Update()
    {
        // Only trigger once
        if (!taskOneComplete && nearInteractable && Input.GetKeyDown(KeyCode.Space))
        {
            taskOneComplete = true;

            // Stop and hide all animators
            foreach (var animator in animators)
            {
                animator.StopAndHide();
            }

            // Set the SceneTransition's requiredCondition to true
            if (sceneTransition != null)
            {
                sceneTransition.requiredCondition = true;
            }

            // Show messages sequentially
            StartCoroutine(ShowMessagesRoutine());
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(interactableTag))
        {
            nearInteractable = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag(interactableTag))
        {
            nearInteractable = false;
        }
    }

    private IEnumerator ShowMessagesRoutine()
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

        // Fade in
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

        // Wait
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

        c.a = 0f;
        messageText.color = c;
    }
}