using UnityEngine;

public class Fragment : MonoBehaviour
{
    void Start()
    {
        // Find the manager and tell it to track this object
        EventManager manager = Object.FindFirstObjectByType<EventManager>();
        if (manager != null)
        {
            manager.RegisterFragment(this.gameObject);
        }
    }
}