using UnityEngine;

public class CameraFogController : MonoBehaviour
{
    [Header("Target Settings")]
    public Transform player;        
    public float zThreshold = -414.5f;

    [Header("Fog Settings")]
    public float startFogDensity = 0.272f;
    public float targetFogDensity = 0.02f;
    public float fogDuration = 2f;

    [Header("Third Person Settings")]
    public Vector3 thirdPersonOffset = new Vector3(0f, 2f, -5f);
    public float cameraMoveDuration = 0.6f;

    [Header("Mouse Sensitivity")]
    public float mouseSensitivity = 50f;   // LOWER THIS to slow rotation

    [Header("Collapsing Hallway Shake")]
    public float shakeAmount = 0.15f;
    public float rotationShakeAmount = 3f;
    public float minShakeInterval = 0.01f;
    public float maxShakeInterval = 0.06f;

    private bool transitionDone = false;
    private float fogTimer = 0f;
    private float cameraTimer = 0f;

    private Vector3 originalLocalPos;
    private Vector3 currentBasePosition;

    private float shakeTimer = 0f;
    private float yRotation = 0f;

    void Start()
    {
        RenderSettings.fog = true;
        RenderSettings.fogDensity = startFogDensity;

        originalLocalPos = transform.localPosition;
        currentBasePosition = originalLocalPos;

        yRotation = transform.eulerAngles.y;
    }

    void Update()
    {
        if (player == null) return;

        HandleMouseRotation();

        // 🔥 SHAKE BEFORE THRESHOLD
        if (!transitionDone && player.position.z > zThreshold)
        {
            shakeTimer -= Time.deltaTime;

            if (shakeTimer <= 0f)
            {
                shakeTimer = Random.Range(minShakeInterval, maxShakeInterval);

                Vector3 randomOffset = new Vector3(
                    Random.Range(-1f, 1f),
                    Random.Range(-1f, 1f),
                    0f
                ) * shakeAmount;

                transform.localPosition = currentBasePosition + randomOffset;

                float randomZ = Random.Range(-rotationShakeAmount, rotationShakeAmount);
                transform.localRotation = Quaternion.Euler(0f, yRotation, randomZ);
            }
        }

        // 🚀 TRANSITION
        if (!transitionDone && player.position.z < zThreshold)
        {
            transform.localRotation = Quaternion.Euler(0f, yRotation, 0f);

            fogTimer += Time.deltaTime;
            float fogT = Mathf.Clamp01(fogTimer / fogDuration);
            RenderSettings.fogDensity =
                Mathf.Lerp(startFogDensity, targetFogDensity, fogT);

            cameraTimer += Time.deltaTime;
            float camT = Mathf.Clamp01(cameraTimer / cameraMoveDuration);
            camT = camT * camT * (3f - 2f * camT);

            currentBasePosition =
                Vector3.Lerp(originalLocalPos, thirdPersonOffset, camT);

            transform.localPosition = currentBasePosition;

            if (fogT >= 1f && camT >= 1f)
            {
                RenderSettings.fogDensity = targetFogDensity;
                transform.localPosition = thirdPersonOffset;
                currentBasePosition = thirdPersonOffset;
                transitionDone = true;
            }
        }
    }

    void HandleMouseRotation()
    {
        float mouseX = Input.GetAxis("Mouse X");

        yRotation += mouseX * mouseSensitivity * Time.deltaTime;

        transform.rotation = Quaternion.Euler(0f, yRotation, 0f);
    }
}