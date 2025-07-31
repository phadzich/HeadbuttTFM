using UnityEngine;

public class PauseController : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenuPanel;
    [SerializeField] private KeyCode pauseKey = KeyCode.Escape;

    private bool isPaused = false;

    void Update()
    {
        if (Input.GetKeyDown(pauseKey))
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
