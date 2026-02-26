using UnityEngine;

public class SimpleSpriteAnimator : MonoBehaviour
{
    public Sprite sprite1;
    public Sprite sprite2;
    public float switchInterval = 0.3f; // seconds per frame

    private SpriteRenderer spriteRenderer;
    private float timer;
    private bool usingFirst = true;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = sprite1;
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= switchInterval)
        {
            timer = 0f;

            // Toggle between sprite1 and sprite2
            if (usingFirst)
                spriteRenderer.sprite = sprite2;
            else
                spriteRenderer.sprite = sprite1;

            usingFirst = !usingFirst;
        }
    }
}