using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 0.5f;
    private Rigidbody2D rb;
    private Vector2 movement;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        float vertical = -Input.GetAxisRaw("Horizontal");
        float horizontal = Input.GetAxisRaw("Vertical");

        horizontal = Mathf.Round(horizontal);
        vertical = Mathf.Round(vertical);

        if (horizontal != 0 && vertical != 0)
        {
            if (Mathf.Abs(horizontal) > Mathf.Abs(vertical))
                vertical = 0;
            else
                horizontal = 0;
        }

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