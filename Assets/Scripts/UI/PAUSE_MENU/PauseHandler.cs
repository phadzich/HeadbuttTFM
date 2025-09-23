using UnityEngine;
using UnityEngine.InputSystem;

public class PauseHandler : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenuPanel;

    private bool isPaused = false;

    public void PauseKey(InputAction.CallbackContext context)
    {
        if (context.started)
        {

            TogglePause();
        }
    }
    public void TogglePause()
    {
        isPaused = !isPaused;
        Time.timeScale = isPaused ? 0f : 1f;
        pauseMenuPanel.SetActive(isPaused);
    }

    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f;
        pauseMenuPanel.SetActive(false);
    }
}
