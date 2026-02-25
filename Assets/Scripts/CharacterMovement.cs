using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float speed = 17f;
    public float slowSpeed = 5f;
    private float originalSpeed;
    private bool slowedDown = false;

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
        originalSpeed = speed;
    }

    void Update()
    {
        if (!slowedDown && transform.position.z < -414.5f)
        {
            speed = slowSpeed;
            slowedDown = true;
        }

        HandleMovement();
        HandleLook();
    }

    void HandleMovement()
    {
        isGrounded = controller.isGrounded;

        if (isGrounded && velocity.y < 0)
            velocity.y = -2f;

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