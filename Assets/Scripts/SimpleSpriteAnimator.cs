using UnityEngine;
using UnityEngine.Tilemaps;

public class SimpleSpriteAnimator : MonoBehaviour
{
    public Tilemap tilemap1;
    public Tilemap tilemap2;
    public float switchInterval = 0.3f; // seconds per switch

    private float timer;
    private bool usingFirst = true;
    private bool isAnimating = true;

    void Start()
    {
        tilemap1.gameObject.SetActive(true);
        tilemap2.gameObject.SetActive(false);
    }

    void Update()
    {
        if (!isAnimating) return;

        timer += Time.deltaTime;

        if (timer >= switchInterval)
        {
            timer = 0f;

            if (usingFirst)
            {
                tilemap1.gameObject.SetActive(false);
                tilemap2.gameObject.SetActive(true);
            }
            else
            {
                tilemap1.gameObject.SetActive(true);
                tilemap2.gameObject.SetActive(false);
            }

            usingFirst = !usingFirst;
        }
    }

    public void StopAndHide()
    {
        isAnimating = false;

        tilemap1.gameObject.SetActive(false);
        tilemap2.gameObject.SetActive(false);
    }
}