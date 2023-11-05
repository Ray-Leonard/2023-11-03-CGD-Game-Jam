using UnityEngine;

public class Pause : MonoBehaviour
{
    private bool isPaused = false;

    public GameObject pausemenu;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetButtonDown("PauseButton"))
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        isPaused = !isPaused;

        if (isPaused)
        {
            Time.timeScale = 0f; // Set time scale to 0 to pause the game.
            pausemenu.SetActive(true);
        }
        else
        {
            Time.timeScale = 1f; // Set time scale back to 1 to unpause the game.
            pausemenu.SetActive(false);
        }
    }
}
