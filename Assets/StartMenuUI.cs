using UnityEngine;

public class StartMenuUI : MonoBehaviour
{
    public GameObject settingsPanel;



    public string feedbackURL = "https://docs.google.com/forms/d/e/1FAIpQLSdsJLWgR-lpDkXwi1nI8cCZrx_JCQej7lv6SVlOTgM-TK53tw/viewform"; // Set your desired URL here

    public void OpenFeedback()
    {
        Application.OpenURL(feedbackURL);
    }


    private void Start()
    {
        OpenMainMenu();
    }

    public void OpenMainMenu()
    {
        this.gameObject.SetActive(true);
        InputManager.Instance.SwitchInputToUI();
        UIManager.Instance.currentOpenUI = this.gameObject;
    }

    public void CloseMainMenu()
    {
        this.gameObject.SetActive(false);
        SettingsManager.instance.pauseHandler.ResumeGame();
        //InputManager.Instance.SwitchInputToPlayer();
        LevelManager.Instance.StartGame();
    }

    public void OpenSettings()
    {
        settingsPanel.SetActive(true);
    }
}
