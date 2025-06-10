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

        UpdateStats();
    }

    public void UpdateStats()
    {
        UpdateDurability();
        UpdateHeadbutts();
        UpdateBounceH();
        UpdateHbForce();
        UpdateHbCooldown();
        UpdateKnockbackChance();
    }

    // Para activar o desactivar los botones dependiendo de si tiene suficientes puntos para hacer upgrade
    public void EnableButtons(bool _enable)
    {
        foreach(var btn in buttons)
        {
            btn.interactable = _enable;
        }
    }

    /* Funciones para actualizar las barras de cada stat*/

    public void UpdateDurability()
    {

            durabilityStat.UpdateBar(helmet.GetUpgradeCount(HelmetStatTypeEnum.Durability), helmet.durability);

    }

    public void UpdateHeadbutts()
    {
        headbuttStat.UpdateBar(helmet.GetUpgradeCount(HelmetStatTypeEnum.Headbutts), helmet.maxHeadbutts);
    }

    private void UpdateBounceH()
    {
        bounceHeightStat.UpdateBar(helmet.GetUpgradeCount(HelmetStatTypeEnum.BounceHeight), helmet.bounceHeight);
    }

    private void UpdateHbForce()
    {
        hbForceStat.UpdateBar(helmet.GetUpgradeCount(HelmetStatTypeEnum.HeadBForce), helmet.headBForce);
    }

    private void UpdateHbCooldown()
    {
        hbCooldownStat.UpdateBar(helmet.GetUpgradeCount(HelmetStatTypeEnum.HeadBCooldown, true), helmet.headBCooldown);
    }

    private void UpdateKnockbackChance()
    {
        knockbackChanceStat.UpdateBar(helmet.GetUpgradeCount(HelmetStatTypeEnum.KnockbackChance, true), helmet.knockbackChance);
    }

    /* Funciones para el boton de cada stat*/

    public void OnClickAddDurabilityBtn()
    {
        if (ResourceManager.Instance.resourceTrader.CanSpendUpgradePoints(1))
        {
            helmet.UpgradeDurability(((int)HelmetManager.Instance.GetUpgradeIncrement(HelmetStatTypeEnum.Durability)));
            UpdateDurability();
        }
    }

    public void OnClickAddHeadbuttsBtn()
    {
        if (ResourceManager.Instance.resourceTrader.CanSpendUpgradePoints(1))
        {
            helmet.UpgradeHeadbutt(((int)HelmetManager.Instance.GetUpgradeIncrement(HelmetStatTypeEnum.Headbutts)));
            UpdateHeadbutts();
        }
    }

    public void OnClickAddBounceHeightBtn()
    {
        if (ResourceManager.Instance.resourceTrader.CanSpendUpgradePoints(1))
        {
            helmet.UpgradeBounceHeight(HelmetManager.Instance.GetUpgradeIncrement(HelmetStatTypeEnum.BounceHeight));
            UpdateBounceH();
        }
    }

    public void OnClickAddHbForceBtn()
    {
        if (ResourceManager.Instance.resourceTrader.CanSpendUpgradePoints(1))
        {
            helmet.UpgradeHeadBForce(HelmetManager.Instance.GetUpgradeIncrement(HelmetStatTypeEnum.HeadBForce));
            UpdateHbForce();
        }
    }

    public void OnClickAddHbCooldownBtn()
    {
        if (ResourceManager.Instance.resourceTrader.CanSpendUpgradePoints(1))
        {
            helmet.UpgradeHeadBCooldown(HelmetManager.Instance.GetUpgradeIncrement(HelmetStatTypeEnum.HeadBCooldown));
            UpdateHbCooldown();
        }
    }

    public void OnClickAddKnockbackChanceBtn()
    {
        if (ResourceManager.Instance.resourceTrader.CanSpendUpgradePoints(1))
        {
            helmet.UpgradeKnockbackChance(((int)HelmetManager.Instance.GetUpgradeIncrement(HelmetStatTypeEnum.KnockbackChance)));
            UpdateKnockbackChance();
        }
    }
}
