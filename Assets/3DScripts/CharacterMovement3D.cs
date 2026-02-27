using UnityEngine;

public class CharacterMovement3D : MonoBehaviour
{
    [Header("Movement Settings")]
    public float speed = 5f;
    public float sprintMultiplier = 2f;

    [Header("Animation")]
    public Animator animator;

    [Header("Mouse Settings")]
    [Range(5f, 150f)]
    public float mouseSensitivity = 50f;
    public Transform playerCamera;
    public float maxMouseDelta = 2f;

    [Header("Gravity Settings")]
    public float gravity = -9.81f;

    [Header("Control")]
    public bool canMove = true;

    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;
    private float xRotation = 0f;
    private bool isSprinting = false;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        
        // Error Checks: Make sure everything is assigned in the Inspector
        if (controller == null) Debug.LogError("CharacterController is missing from " + gameObject.name);
        if (playerCamera == null) Debug.LogError("Player Camera is not assigned in the Inspector!");
        if (animator == null) Debug.LogWarning("Animator is not assigned. Animations will not play.");

        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        if (canMove)
        {
            HandleMovement();
            HandleLook();
            if (animator != null)
            {
                animator.SetBool("IsRunning", isSprinting);
            }
        }
    }

    void HandleMovement()
    {
        isGrounded = controller.isGrounded;

        if (isGrounded && velocity.y < 0)
            velocity.y = -2f;

        // --- TOGGLE LOGIC WITH DEBUGGING ---
        if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift))
        {
            isSprinting = !isSprinting;
            Debug.Log("Shift Pressed! Sprinting is now: " + isSprinting);
            
            if (animator != null) 
            {
                animator.SetBool("IsRunning", isSprinting);
            }
        }
        if (Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.RightControl))
        {
            isSprinting = !isSprinting;
            Debug.Log("Shift Pressed! Sprinting is now: " + isSprinting);
            
            if (animator != null) 
            {
                animator.SetBool("IsRunning", isSprinting);
            }
        }
        Debug.Log(isSprinting);
        // Calculate speed
        float currentSpeed = isSprinting ? (speed * sprintMultiplier) : speed;

        // Move forward logic
        Vector3 move = transform.forward;
        
        // Debug check: Is the speed actually being applied?
        if (move.magnitude > 0)
        {
            controller.Move(move * currentSpeed * Time.deltaTime);
        }
        else
        {
            Debug.LogWarning("Movement vector is zero. Check transform.forward.");
        }

        // Gravity
        velocity.y += gravity * Time.deltaTime;
        float terminalVelocity = -50f;
        velocity.y = Mathf.Max(velocity.y, terminalVelocity);

        controller.Move(new Vector3(0, velocity.y, 0) * Time.deltaTime);
    }

    void HandleLook()
    {
        // Simple Mouse Look logic
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        if (playerCamera != null)
        {
            playerCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
            transform.Rotate(Vector3.up * mouseX);
        }
    }
}