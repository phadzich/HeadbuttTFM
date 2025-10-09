using UnityEngine;

public class GameOverPanelUI : MonoBehaviour
{
    public GameObject HUBButton;

    private void OnEnable()
    {
        if (LevelManager.Instance.currentLevel.config == LevelManager.Instance.levelsList[0])
        {
            HUBButton.SetActive(false);
        }
        else
        {
            HUBButton.SetActive(true);
        }
    }
}
