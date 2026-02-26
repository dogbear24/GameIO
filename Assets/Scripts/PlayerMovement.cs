using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 0.5f;

    public Sprite playerUp;      // 2d_player1
    public Sprite playerDown;    // 2d_player2
    public Sprite playerLeft;    // 2d_player3
    public Sprite playerRight;   // 2d_player4

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Vector2 movement;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
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

        // Update sprite direction whenever the player moves
        UpdateSpriteDirection(horizontal, vertical);
    }

    void FixedUpdate()
    {
        if (movement != Vector2.zero)
        {
            rb.MovePosition(rb.position + movement.normalized * moveSpeed * Time.fixedDeltaTime);
        }
    }

    void UpdateSpriteDirection(float horizontal, float vertical)
    {
        if (vertical > 0)
            spriteRenderer.sprite = playerLeft;
        else if (vertical < 0)
            spriteRenderer.sprite = playerRight;
        else if (horizontal > 0)
            spriteRenderer.sprite = playerUp;
        else if (horizontal < 0)
            spriteRenderer.sprite = playerDown;
    }
}