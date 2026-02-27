using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Plant : MonoBehaviour
{
    [Header("What to spawn")]
    public GameObject objectToPlace;
    public GameObject objectToReplace;

    [Header("Reference to player")]
    public Transform player;

    [Header("Distance in front of player")]
    public float distance = 2f;

    [Header("Upright rotation offset")]
    public Vector3 uprightRotation = new Vector3(90f, 0f, 0f); // X = 90° makes it upright

    public TextureSwitcher textureSwitcher;
    
    private List<GameObject> spawnedPlants = new List<GameObject>();
    private bool lastUsingSetA;


    private void Start()
    {
        if (textureSwitcher == null)
        {
            Debug.LogError("TextureSwitcher reference not set!");
        }

        lastUsingSetA = textureSwitcher.usingSetA;
    }

    private void Update()
    {
        if (textureSwitcher == null) return;

        // Check if usingSetA has changed since last frame
        if (textureSwitcher.usingSetA != lastUsingSetA)
        {
            ReplaceSpawnedPlants();
            lastUsingSetA = textureSwitcher.usingSetA;
        }
    }

    // This method must be public to link to a UI Button
    public void PlaceObjectInFront()
    {
        if (objectToPlace == null || player == null)
        {
            Debug.LogWarning("Missing objectToPlace or player reference!");
            return;
        }

        
        if (textureSwitcher != null && !textureSwitcher.usingSetA) // check bool
        {
            // Position in front of the player, same Y level
            Vector3 spawnPosition = player.position + player.forward * distance;
            spawnPosition.y = player.position.y;

            // Rotation
            Quaternion spawnRotation = Quaternion.Euler(uprightRotation);

            // Instantiate the object
            GameObject newPlant = Instantiate(objectToPlace, spawnPosition, spawnRotation);
            spawnedPlants.Add(newPlant);
        }
    }

    public void ReplaceSpawnedPlants()
    {
        if (spawnedPlants.Count == 0) return;

        // Decide which prefab to replace with
        GameObject replacementPrefab = textureSwitcher.usingSetA ? objectToReplace : objectToPlace;

        for (int i = 0; i < spawnedPlants.Count; i++)
        {
            if (spawnedPlants[i] != null)
            {
                Vector3 pos = spawnedPlants[i].transform.position;
                Quaternion rot = spawnedPlants[i].transform.rotation;

                Destroy(spawnedPlants[i]); // remove old

                // Rotation
                Quaternion spawnRotation = Quaternion.Euler(uprightRotation);
                spawnedPlants[i] = Instantiate(replacementPrefab, pos, rot); // replace
            }
        }
        Debug.Log("Plants replaced!");
    }
        
        
}