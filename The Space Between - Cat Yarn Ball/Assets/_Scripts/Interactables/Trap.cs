using UnityEngine;

public class Trap : MonoBehaviour
{

    public int damage = 1;

    public AudioClip sound;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StatManager.instance.DeductHealth(damage);

            if (sound != null)
            {
                AudioSource.PlayClipAtPoint(sound, transform.position);
            }

        }
    }
}
