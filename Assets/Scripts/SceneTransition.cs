using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneTransition : MonoBehaviour
{
    public string sceneToLoad;
    public bool requiredCondition; // REPLACE
    public float fadeDuration = 1f;

    private Image fadePanel;

    private void Awake()
    {
        // Create a full-screen Canvas
        Canvas canvas = new GameObject("FadeCanvas").AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.gameObject.AddComponent<CanvasScaler>();
        canvas.gameObject.AddComponent<GraphicRaycaster>();

        // Create the fade panel
        GameObject panelObj = new GameObject("FadePanel");
        panelObj.transform.SetParent(canvas.transform, false);
        fadePanel = panelObj.AddComponent<Image>();
        fadePanel.color = new Color(0, 0, 0, 0); // black, transparent

        // Stretch panel to full screen
        RectTransform rt = panelObj.GetComponent<RectTransform>();
        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.one;
        rt.offsetMin = Vector2.zero;
        rt.offsetMax = Vector2.zero;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Transition") && requiredCondition)
        {
            StartCoroutine(FadeAndLoadScene());
        }
    }

    private IEnumerator FadeAndLoadScene()
    {
        fadePanel.gameObject.SetActive(true);
        Color color = fadePanel.color;
        float t = 0f;

        // Fade to black
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            color.a = Mathf.Lerp(0, 1, t / fadeDuration);
            fadePanel.color = color;
            yield return null;
        }

        SceneManager.LoadScene(sceneToLoad);

        // Optional fade back in
        t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            color.a = Mathf.Lerp(1, 0, t / fadeDuration);
            fadePanel.color = color;
            yield return null;
        }
    }
}