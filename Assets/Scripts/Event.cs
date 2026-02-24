using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class Event : MonoBehaviour
{
    public Transform player;
    public Transform targetObject;

    public GameObject videoCanvas;
    public GameObject whiteCanvas;   // NEW

    public VideoPlayer videoPlayer;
    public Image whiteFadeImage;

    public string sceneToLoad;

    public float triggerDistance = 3f;
    public float fadeDuration = 1f;

    private bool triggered = false;

    void Start()
    {
        videoCanvas.SetActive(false);
        whiteCanvas.SetActive(true);

        SetWhiteAlpha(0f);
    }

    void Update()
    {
        if (triggered) return;

        float distance = Vector3.Distance(player.position, targetObject.position);

        if (distance <= triggerDistance)
        {
            triggered = true;
            StartCoroutine(FadeThenPlay());
        }
    }

    IEnumerator FadeThenPlay()
    {
        float timer = 0f;
        Color color = whiteFadeImage.color;

        // Fade TO white
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            color.a = Mathf.Lerp(0f, 1f, timer / fadeDuration);
            whiteFadeImage.color = color;
            yield return null;
        }

        // Wait 1.9 seconds
        yield return new WaitForSeconds(1.9f);

        // Show video canvas
        videoCanvas.SetActive(true);

        // Hide white canvas completely
        whiteCanvas.SetActive(false);

        videoPlayer.loopPointReached += OnVideoFinished;
        videoPlayer.Play();
    }

    void SetWhiteAlpha(float alpha)
    {
        Color c = whiteFadeImage.color;
        c.a = alpha;
        whiteFadeImage.color = c;
    }

    void OnVideoFinished(VideoPlayer vp)
    {
        SceneManager.LoadScene(sceneToLoad);
    }
}