using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Rigidbody2D rb;
    private Vector2 movement;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Get raw input
        float vertical = -Input.GetAxisRaw("Horizontal");
        float horizontal = Input.GetAxisRaw("Vertical");

        // Round input to -1, 0, 1 to prevent tiny drift
        horizontal = Mathf.Round(horizontal);
        vertical = Mathf.Round(vertical);

        // Only allow movement in one direction at a time
        if (horizontal != 0 && vertical != 0)
        {
            // Prioritize the axis with larger absolute input
            if (Mathf.Abs(horizontal) > Mathf.Abs(vertical))
                vertical = 0;
            else
                horizontal = 0;
        }

        // Map to isometric axes
        movement = new Vector2(horizontal - vertical, (horizontal + vertical) / 2f);
    }

    void FixedUpdate()
    {
        if (movement != Vector2.zero)
        {
            rb.MovePosition(rb.position + movement.normalized * moveSpeed * Time.fixedDeltaTime);
        }
    }
}