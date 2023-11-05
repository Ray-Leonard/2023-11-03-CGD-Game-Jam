using UnityEngine;
using UnityEngine.Events;

public class EventTriggerHandler : MonoBehaviour
{
    public UnityEvent onPlayerEnter;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the entering collider is the player
        if (other.CompareTag("Player"))
        {
            // Trigger the event when the player enters the trigger area
            onPlayerEnter.Invoke();
            Debug.Log("pleyr entered");
        }
    }
}