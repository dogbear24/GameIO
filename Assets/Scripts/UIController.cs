using UnityEngine;
using UnityEngine.UI;

public class UIHideOnZ : MonoBehaviour
{
    [Header("References")]
    public Transform player;
    public Image uiImage;  

    [Header("Settings")]
    public float zThreshold = -414.5f;
    public float fadeDuration = 2f;

    private bool fading = false;
    private float fadeTimer = 0f;
    private Color originalColor;

    void Start()
    {
        if (uiImage == null)
        {
            Debug.LogError("UI Image not assigned!");
            return;
        }

        originalColor = uiImage.color;
    }

    void Update()
    {
        if (player == null || uiImage == null)
            return;

        if (!fading && player.position.z < zThreshold)
        {
            fading = true;
        }

        if (fading)
        {
            fadeTimer += Time.deltaTime;
            float t = Mathf.Clamp01(fadeTimer / fadeDuration);

            Color newColor = originalColor;
            newColor.a = Mathf.Lerp(1f, 0f, t);
            uiImage.color = newColor;

            if (t >= 1f)
            {
                uiImage.gameObject.SetActive(false);
                fading = false;
            }
        }
    }
}