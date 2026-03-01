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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        videoCanvas.SetActive(false);
        UICanvas.SetActive(false);
        UI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (eventManager.endCutScene == true)
        {
            videoCanvas.SetActive(true);
            videoPlayer.loopPointReached += OnVideoFinished;
            videoPlayer.Play();
        }
    }

    void OnVideoFinished(VideoPlayer vp)
    {
        videoCanvas.SetActive(false);
        UICanvas.SetActive(true);
        UI.SetActive(true);
    }
}
