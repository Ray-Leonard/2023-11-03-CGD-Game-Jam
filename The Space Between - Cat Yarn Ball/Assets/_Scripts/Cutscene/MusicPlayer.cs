using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    private static MusicPlayer instance;

    // This function will run before the Start function
    private void Awake()
    {
        // Check if an instance of this script already exists
        if (instance == null)
        {
            // If not, set this GameObject as the instance
            instance = this;

            // Don't destroy this GameObject when loading a new scene
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

}