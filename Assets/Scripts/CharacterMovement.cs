using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float speed = 17f;
    public float slowSpeed = 5f;         // Speed when z < -414.5
    private float originalSpeed;
    private bool slowedDown = false;     // Ensure speed drops only once

    [Header("Mouse Settings")]
    public float mouseSensitivity = 200f;
    public Transform playerCamera;

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
        // Reduce speed once past the z threshold
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

        // Forward movement is always "pressed down"
        Vector3 move = transform.forward; // ignore horizontal input
        controller.Move(move * speed * Time.deltaTime);

        // Apply gravity
        velocity.y += gravity * Time.deltaTime;
        float terminalVelocity = -50f;
        velocity.y = Mathf.Max(velocity.y, terminalVelocity);
        controller.Move(new Vector3(0, velocity.y, 0) * Time.deltaTime);
    }

    void HandleLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        playerCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        transform.Rotate(Vector3.up * mouseX);
    }
}