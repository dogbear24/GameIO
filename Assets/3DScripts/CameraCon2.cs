using UnityEngine;

public class CameraCon2 : MonoBehaviour
{
    [Header("References")]
    public Transform player;

    [Header("Fog Settings")]
    public float fogDensity = 0.272f;

    [Header("Third Person Settings")]
    public Vector3 thirdPersonOffset = new Vector3(0f, 2f, -5f);

    [Header("Mouse Sensitivity")]
    public float mouseSensitivity = 50f;

    private float yRotation = 0f;

    void Start()
    {
        // Keep fog constant
        RenderSettings.fog = true;
        RenderSettings.fogDensity = fogDensity;

        // Force camera to third-person position
        transform.localPosition = thirdPersonOffset;

        yRotation = transform.eulerAngles.y;
    }

    void Update()
    {
        if (player == null) return;

        HandleMouseRotation();

        // Keep camera locked at third-person offset
        transform.localPosition = thirdPersonOffset;
    }

    void HandleMouseRotation()
    {
        float mouseX = Input.GetAxis("Mouse X");

        yRotation += mouseX * mouseSensitivity * Time.deltaTime;

        transform.rotation = Quaternion.Euler(0f, yRotation, 0f);
    }
}