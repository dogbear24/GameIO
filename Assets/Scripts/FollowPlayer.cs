using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;        // Drag your player here in the inspector
    public float smoothSpeed = 0.125f; // How smoothly the camera follows
    public Vector3 offset;          // Offset from the player, usually z=-10 for 2D

    void LateUpdate()
    {
        if (target == null) return;

        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;
    }
}