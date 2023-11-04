using UnityEngine;

public class Collectable : MonoBehaviour
{
    
    public int pointValue = 1; 
    
    public AudioClip collectSound; 

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StatManager.instance.AddPoints(pointValue);

            if (collectSound != null)
            {
                AudioSource.PlayClipAtPoint(collectSound, transform.position);
            }

            Destroy(gameObject);
        }
    }
}