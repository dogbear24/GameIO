using UnityEngine;

public class CharacterMovement2 : MonoBehaviour
{
    [Header("Movement Settings")]
    public float speed = 5f;   // Always walking speed

    [Header("Mouse Settings")]
    [Range(5f, 150f)]
    public float mouseSensitivity = 50f;
    public Transform playerCamera;
    public float maxMouseDelta = 2f;

    [Header("Gravity Settings")]
    public float gravity = -9.81f;

    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;
    private float xRotation = 0f;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        HandleMovement();
        HandleLook();
    }

    void HandleMovement()
    {
        isGrounded = controller.isGrounded;

        if (isGrounded && velocity.y < 0)
            velocity.y = -2f;

        // Always move forward at constant slow speed
        Vector3 move = transform.forward;
        controller.Move(move * speed * Time.deltaTime);

        velocity.y += gravity * Time.deltaTime;
        float terminalVelocity = -50f;
        velocity.y = Mathf.Max(velocity.y, terminalVelocity);

        controller.Move(new Vector3(0, velocity.y, 0) * Time.deltaTime);
    }

    void HandleLook()
    {
        float rawMouseX = Mathf.Clamp(Input.GetAxis("Mouse X"), -maxMouseDelta, maxMouseDelta);
        float rawMouseY = Mathf.Clamp(Input.GetAxis("Mouse Y"), -maxMouseDelta, maxMouseDelta);

        float mouseX = rawMouseX * mouseSensitivity * Time.deltaTime;
        float mouseY = rawMouseY * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        playerCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }
}