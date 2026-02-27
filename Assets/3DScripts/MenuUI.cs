using UnityEngine;

public class MenuUI : MonoBehaviour
{

    public GameObject UI;
    public CameraCon2 cameraController;
    public CharacterMovement3D characterMovement;
    public TriggerAnimationOnPosition triggerAnimationScript;
    public Animator playerAnimator;
    private bool isOpen = false;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            isOpen = !isOpen;
            UI.SetActive(isOpen);
            
            if (cameraController != null)
            {
                cameraController.canMoveCamera = !isOpen;
            }
            if (characterMovement != null)
            {
                characterMovement.canMove = !isOpen;
            }

            if (triggerAnimationScript != null)
            {
                triggerAnimationScript.canTrigger = !isOpen;
            }

            if (isOpen)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = false;
            }
            playerAnimator.speed = isOpen ? 0f : 1f;
        }
    }
}
