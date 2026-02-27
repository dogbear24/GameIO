using UnityEngine;
using UnityEngine.UI;

public class FloatingSmoke : MonoBehaviour
{
    [Header("Assign your 3 smoke layers")]
    public Image[] smokeLayers;

    [Header("Movement Settings")]
    public float floatAmountX = 10f; 
    public float floatAmountY = 5f;  
    public float floatSpeed = 1f; 

    private Vector2[] initialPositions;

    void Start()
    {
        if (smokeLayers == null || smokeLayers.Length == 0)
        {
            Debug.LogError("Smoke Layers not assigned!");
            return;
        }

        initialPositions = new Vector2[smokeLayers.Length];

        for (int i = 0; i < smokeLayers.Length; i++)
        {
            if (smokeLayers[i] == null)
            {
                //Debug.LogError("One of the smoke layers is null!");
                continue;
            }

            initialPositions[i] = smokeLayers[i].rectTransform.anchoredPosition;
        }
    }

    void Update()
    {
        for (int i = 0; i < smokeLayers.Length; i++)
        {
            float offsetX = Mathf.Sin(Time.time * floatSpeed + i * 10f) * floatAmountX;
        }
    }
}
