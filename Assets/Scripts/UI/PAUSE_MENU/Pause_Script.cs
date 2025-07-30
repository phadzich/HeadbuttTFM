using UnityEngine;

public class PauseManager : MonoBehaviour
{
    public GameObject pauseMenuPanel;
    public GameObject optionsMenuPrefab;       // Aqui va el Options_Menu
    public Transform uiCanvasTransform;        // Aqui el canvas

    private bool isPaused = false;
    private GameObject currentOptionsMenu;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                ResumeGame();
            else
                PauseGame();
        }
    }

    void PauseGame()
    {
        pauseMenuPanel.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void ResumeGame()
    {
        pauseMenuPanel.SetActive(false);
        CloseOptionsMenu(); // Por si está abierto
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void OpenOptionsMenu()
    {
        if (currentOptionsMenu == null)
        {
            currentOptionsMenu = Instantiate(optionsMenuPrefab, uiCanvasTransform, false);
        }
    }

    public void CloseOptionsMenu()
    {
        if (currentOptionsMenu != null)
        {
            Destroy(currentOptionsMenu);
            currentOptionsMenu = null;
        }
    }
}
