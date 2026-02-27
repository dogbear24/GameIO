using UnityEngine;

public class TaskTwo : MonoBehaviour
{
    [Header("Tilemaps & Animators")]
    public GameObject tilemapToDeactivate;      // Tilemap to hide
    public GameObject tilemapToActivate;        // Tilemap to show
    public SimpleSpriteAnimator[] animatorsToStop; // Drag both tilemap animators here

    [Header("Trigger Settings")]
    public string triggerTag = "TaskTwoTrigger"; // Tag of the trigger object

    [Header("Task Dependency")]
    public TaskOne taskOneComponent; // Drag the player's TaskOne component here

    private bool nearTrigger = false;
    private bool switched = false; // ensures it only triggers once

    private void Update()
    {
        if (nearTrigger && !switched && Input.GetKeyDown(KeyCode.Space))
        {
            // Only trigger if TaskOne is complete
            if (taskOneComponent != null && taskOneComponent.taskOneComplete)
            {
                switched = true;

                // Stop all specified animators
                if (animatorsToStop != null)
                {
                    foreach (var anim in animatorsToStop)
                    {
                        if (anim != null)
                            anim.StopAndHide();
                    }
                }

                // Switch tilemaps
                if (tilemapToDeactivate != null)
                    tilemapToDeactivate.SetActive(false);

                if (tilemapToActivate != null)
                    tilemapToActivate.SetActive(true);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(triggerTag))
        {
            nearTrigger = true;

            // Automatically assign TaskOne if not set
            if (taskOneComponent == null)
            {
                taskOneComponent = other.GetComponent<TaskOne>();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag(triggerTag))
        {
            nearTrigger = false;
        }
    }
}