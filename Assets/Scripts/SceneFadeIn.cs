using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SceneFadeIn : MonoBehaviour
{
    public float fadeDuration = 1f;

    private Image fadePanel;

    private void Awake()
    {
        // Create Canvas
        Canvas canvas = new GameObject("FadeCanvas").AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.gameObject.AddComponent<CanvasScaler>();
        canvas.gameObject.AddComponent<GraphicRaycaster>();

        // Create black panel
        GameObject panelObj = new GameObject("FadePanel");
        panelObj.transform.SetParent(canvas.transform, false);
        fadePanel = panelObj.AddComponent<Image>();
        fadePanel.color = new Color(0, 0, 0, 1); // fully black at start

        // Stretch to full screen
        RectTransform rt = panelObj.GetComponent<RectTransform>();
        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.one;
        rt.offsetMin = Vector2.zero;
        rt.offsetMax = Vector2.zero;
    }

    private void Start()
    {
        StartCoroutine(FadeIn());
    }

    private IEnumerator FadeIn()
    {
        float t = 0f;
        Color color = fadePanel.color;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            color.a = Mathf.Lerp(1, 0, t / fadeDuration);
            fadePanel.color = color;
            yield return null;
        }

        fadePanel.color = new Color(0, 0, 0, 0);
    }
}