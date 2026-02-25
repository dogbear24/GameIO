using UnityEngine;

public class PlayerFall : MonoBehaviour
{
    public float fallSpeed = 5f;
    private bool isFalling = false;
    private PlayerMovement movementScript;

    void Start()
    {
        movementScript = GetComponent<PlayerMovement>();
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if(other.CompareTag("FallZone"))
        {
            transform.position += Vector3.down * fallSpeed * Time.fixedDeltaTime;
        }
    }

    void FixedUpdate()
    {
        if(isFalling)
        {
            transform.position += Vector3.down * fallSpeed * Time.fixedDeltaTime;
        }
    }
}