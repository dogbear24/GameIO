using UnityEngine;

public class Hide : MonoBehaviour
{
    [Header("Tilemap to hide")]
    public GameObject tilemapToHide;

    void Start()
    {
        if (tilemapToHide != null)
        {
            tilemapToHide.SetActive(false);
        }
        else
        {
            Debug.LogWarning("No tilemap assigned to HideTilemapOnStart!");
        }
    }
}