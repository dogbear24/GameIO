using UnityEngine;
using UnityEngine.UI;

public class FloatingSmoke : MonoBehaviour
{
    [Header("Assign your 3 smoke layers")]
    public Image[] smokeLayers;

    [Header("Movement Settings")]
    public float floatAmountX = 10f; // max horizontal shift
    public float floatAmountY = 5f;  // max vertical shift
    public float floatSpeed = 1f;    // speed of floating

    private Vector2[] initialPositions;

    void Start()
    {


        // Store starting positions
        initialPositions = new Vector2[smokeLayers.Length];
        for (int i = 0; i < smokeLayers.Length; i++)
        {
            initialPositions[i] = smokeLayers[i].rectTransform.anchoredPosition;
        }
    }

    void Update()
    {
        for (int i = 0; i < smokeLayers.Length; i++)
        {
            // Offset based on sine and cosine for independent movement
            float offsetX = Mathf.Sin(Time.time * floatSpeed + i * 10f) * floatAmountX;
        }
    }
}
