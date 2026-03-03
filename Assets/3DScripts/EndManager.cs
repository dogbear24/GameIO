using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class EndManager : MonoBehaviour
{
    public EventManager eventManager;
    public GameObject videoCanvas;
    public VideoPlayer videoPlayer;
    public GameObject UICanvas;
    public GameObject UI;
    public GameObject UI2;

    private bool hasStartedEnding = false; 

    void Start()
    {
        videoCanvas.SetActive(false);
        UICanvas.SetActive(false);
        UI.SetActive(false);
        UI2.SetActive(false);
    }

    void Update()
    {
        if (eventManager.endCutScene == true && !hasStartedEnding)
        {
            StartCoroutine(EndSequenceRoutine());
        }
    }

    IEnumerator EndSequenceRoutine()
    {
        hasStartedEnding = true; 

        UICanvas.SetActive(true);
        UI.SetActive(true);
        UI2.SetActive(true);

        yield return new WaitForSeconds(8f);

        SceneManager.LoadScene("Menu");
    }

    void OnVideoFinished(VideoPlayer vp)
    {
        videoCanvas.SetActive(false);
        UICanvas.SetActive(true);
        UI.SetActive(true);
    }
}