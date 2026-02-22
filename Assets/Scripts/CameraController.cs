using UnityEngine;

public class CameraFogController : MonoBehaviour
{
    [Header("Target Settings")]
    public Transform player;        // Assign your player here
    public float zThreshold = -414.5f;

    [Header("Fog Settings")]
    public float startFogDensity = 0.272f;  // Original fog density
    public float targetFogDensity = 0.02f;  // Target fog density
    public float duration = 2f;             // Duration of fade

    private bool transitionDone = false;    // Ensure it only runs once
    private float transitionTimer = 0f;

    void Start()
    {
        // Make sure fog is enabled
        RenderSettings.fog = true;
        RenderSettings.fogDensity = startFogDensity;
    }

    void Update()
    {
        if (player == null || transitionDone) return;

        if (player.position.z < zThreshold)
        {
            transitionTimer += Time.deltaTime;
            float t = Mathf.Clamp01(transitionTimer / duration);

            // Smoothly update fog density
            RenderSettings.fogDensity = Mathf.Lerp(startFogDensity, targetFogDensity, t);

            if (t >= 1f)
            {
                RenderSettings.fogDensity = targetFogDensity; // ensure exact value
                transitionDone = true;
            }
        }
    }
}