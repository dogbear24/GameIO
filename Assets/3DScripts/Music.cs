using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Music : MonoBehaviour
{
    [Header("Music Settings")]
    public AudioClip musicClip;   // Drag your MP3 here
    public float volume = 1f;
    public bool loop = true;

    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        if (musicClip == null)
        {
            Debug.LogWarning("No music clip assigned!");
            return;
        }

        audioSource.clip = musicClip;
        audioSource.volume = volume;
        audioSource.loop = loop;
        audioSource.playOnAwake = false;

        audioSource.Play();
    }
}