using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class Menu : MonoBehaviour
{
    public TextMeshProUGUI titleText;
    public Image fadeImage;
    public float spacingAmount = 50f;
    public float animationDuration = 1.5f;

    public void PlayGame()
    {
        StartCoroutine(PlayAnimation());
    }

    IEnumerator PlayAnimation()
    {
        float timer = 0f;

        float startSpacing = titleText.characterSpacing;
        float startAlpha = 0f;

        Color fadeColor = fadeImage.color;

        while (timer < animationDuration)
        {
            timer += Time.deltaTime;
            float t = Mathf.SmoothStep(0f, 1f, timer / animationDuration);

            // Increase character spacing
            titleText.characterSpacing = Mathf.Lerp(startSpacing, spacingAmount, t);

            // Fade to black
            fadeColor.a = Mathf.Lerp(startAlpha, 1f, t);
            fadeImage.color = fadeColor;

            yield return null;
        }

        SceneManager.LoadScene("Intro");
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit!");
    }
}