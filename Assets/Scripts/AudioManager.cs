using UnityEngine;

public class ZPositionAudioController : MonoBehaviour
{
    public Transform player;

    public AudioSource musicA; 
    public AudioSource musicB; 

    public float upperThreshold = -410f;
    public float lowerThreshold = -414.5f;

    private enum MusicState { None, A, B }
    private MusicState currentState = MusicState.None;

    void Update()
    {
        float z = player.position.z;

        if (z < upperThreshold) {
            musicA.Stop();
        }



        if (z > upperThreshold && currentState != MusicState.A)
        {
            PlayMusicA();
        }
        else if (z < lowerThreshold && currentState != MusicState.B)
        {
            PlayMusicB();
        }
    }

    void PlayMusicA()
    {
        musicB.Stop();

        if (!musicA.isPlaying)
            musicA.Play();

        currentState = MusicState.A;
        Debug.Log("Playing Music A");
    }

    void PlayMusicB()
    {
        musicA.Stop();

        if (!musicB.isPlaying)
            musicB.Play();

        currentState = MusicState.B;
        Debug.Log("Playing Music B");
    }
}