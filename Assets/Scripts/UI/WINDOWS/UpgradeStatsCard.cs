using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class UpgradeStatsCard : MonoBehaviour
{
    [Header("UI Elements")]
    public TextMeshProUGUI helmetNameText;
    public Image helmetIcon;
    public Button[] buttons;

    private HelmetInstance helmet;

    [Header("Stat bars")]
    public StatBar durabilityStat;
    public StatBar headbuttStat;
    public StatBar bounceHeightStat;
    public StatBar hbForceStat;
    public StatBar hbCooldownStat;
    public StatBar knockbackChanceStat;

    // Se crea el prefab con la información del helmet
    public void SetUp(HelmetInstance _helmetI)
    {
        helmet = _helmetI;
        helmetNameText.text = _helmetI.currentInfo.name;
        helmetIcon.sprite = _helmetI.currentInfo.icon;

    }
}
