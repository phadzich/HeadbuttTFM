using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnterLevelUI : MonoBehaviour
{
    private LevelConfig config;
    public Image progressFill;
    public Image levelIcon;
    public TextMeshProUGUI levelName;
    public TextMeshProUGUI levelProgress;

    public void Setup(LevelConfig _levelConfig)
    {
        config = _levelConfig;
        levelName.text = config.levelName;
        levelProgress.text = $"EXPLORED: {LevelManager.Instance.LevelMaxDepth(config)}/{LevelManager.Instance.LevelTotalDepth(config)}";
        progressFill.fillAmount = LevelManager.Instance.LevelProgress(config);
        levelName.color = config.levelColor;
        levelIcon.sprite = config.levelIcon;
    }
}
