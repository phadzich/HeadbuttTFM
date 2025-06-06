using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CurrentHelmetHUD : MonoBehaviour
{
    [Header("Current Helmet Info")]
    [SerializeField] public Image currentIcon;
    [SerializeField] public TextMeshProUGUI currentLevelTXT;
    [SerializeField] public Image currentLevelSlider;
    [SerializeField] public Image currentCooldownSlider;
    [SerializeField] public HelmetInstance currentHelmetInstance;

    [Header("EquippedHelmets")]
    [SerializeField] Transform countersContainer;
    [SerializeField] EquippedHelmetHUDCounter equippedCounter;
    [SerializeField] List<EquippedHelmetHUDCounter> equippedCounterList;

    [Header("Stats")]
    [SerializeField] Transform durabilityCountersContainer; 
    [SerializeField] HelmetDurabilityHUDCounter durabilityCounter;
    [SerializeField] List<HelmetDurabilityHUDCounter> durabilityCounterList;
    [SerializeField] Transform HBcountersContainer;
    [SerializeField] HelmetHeadbuttHUDCounter hbCounter;
    [SerializeField] List<HelmetHeadbuttHUDCounter> hbCounterList;

    public void EquipHelmet(HelmetInstance _helmInstance)
    {
        var _newCounterPF = Instantiate(equippedCounter, countersContainer);
        EquippedHelmetHUDCounter _newCounter = _newCounterPF.GetComponent<EquippedHelmetHUDCounter>();
        _newCounter.EquipHelmet(_helmInstance);
        equippedCounterList.Add(_newCounter);
    }
    public void WearNewHelmet(HelmetInstance _helmIntance)
    {
        HelmetInstance _prevHelmet = currentHelmetInstance;
        EquippedHelmetHUDCounter _prevCounter = FindCounterByInstance(_prevHelmet);
        //UNWEAR o DISABLE EL ANTERIOR
        if (_prevCounter != null)
        {
            if (_prevHelmet.isWornOut)
            {
                _prevCounter.WornOut();
            }
            else
            {
                _prevCounter.UnWearHelmet();
            }

        }
        ClearPreviousStatCounters();

        //WEAR EL NUEVO
        currentHelmetInstance = _helmIntance;
        currentIcon.sprite = _helmIntance.baseHelmet.icon;
        currentCooldownSlider.sprite = _helmIntance.baseHelmet.icon;
        currentLevelTXT.text = $"LVL. {_helmIntance.helmetXP.currentSublevel}";
        currentLevelSlider.fillAmount = (float)_helmIntance.helmetXP.currentXP / (float)_helmIntance.helmetXP.XPForNextSublevel();
        FindCounterByInstance(_helmIntance).WearHelmet();
        InstanceNewStatsCounter();
    }

    public void RestartEquippedCounters()
    {
        foreach(EquippedHelmetHUDCounter _counter in equippedCounterList)
        {
            if (_counter.isWornOut)
            {
                _counter.isWornOut = false;
                _counter.UnWearHelmet();
            }
        }
    }

    public void UpdateLVLInfo(HelmetInstance _helmInstance)
    {
        currentLevelTXT.text = $"LVL. {_helmInstance.helmetXP.currentSublevel}";
        currentLevelSlider.fillAmount = (float)_helmInstance.helmetXP.currentXP / (float)_helmInstance.helmetXP.XPForNextSublevel();
    }
    public void UpdateCurrentHelmetStats()
    {
        ClearPreviousStatCounters();
        InstanceNewStatsCounter();
    }

    private void InstanceNewStatsCounter()
    {
        //INSTANCE DURABILITY
        for (int i = 0; i < currentHelmetInstance.durability; i++)
        {
            var _newCounterPF = Instantiate(durabilityCounter, durabilityCountersContainer);
            HelmetDurabilityHUDCounter _newCounter = _newCounterPF.GetComponent<HelmetDurabilityHUDCounter>();
            if(i< currentHelmetInstance.currentDurability)
            {
                _newCounter.MakeAvailable();
            }
            else
            {
                _newCounter.MakeUnavailable();
            }
                durabilityCounterList.Add(_newCounter);
        }

        //INSTANCE HBS
        for (int i = 0; i < currentHelmetInstance.maxHeadbutts; i++)
        {
            var _newCounterPF = Instantiate(hbCounter, HBcountersContainer);
            HelmetHeadbuttHUDCounter _newCounter = _newCounterPF.GetComponent<HelmetHeadbuttHUDCounter>();
            if (i < currentHelmetInstance.remainingHeadbutts)
            {
                _newCounter.MakeAvailable();
            }
            else
            {
                _newCounter.MakeUnavailable();
            }
            hbCounterList.Add(_newCounter);
        }
    }

    private void ClearPreviousStatCounters()
    {
        //DURABILITY
        foreach (var _counter in durabilityCounterList)
        {
            Destroy(_counter.gameObject);
        }
        durabilityCounterList.Clear();

        //HEADBUTTS
        foreach (var _counter in hbCounterList)
        {
            Destroy(_counter.gameObject);
        }
        hbCounterList.Clear();
    }


    public EquippedHelmetHUDCounter FindCounterByInstance(HelmetInstance _helmInstance)
    {
        EquippedHelmetHUDCounter _foundCounter = null;
        foreach(EquippedHelmetHUDCounter _counter in equippedCounterList)
        {
            if (_helmInstance == _counter.helmetInstance)
            {
                _foundCounter = _counter;
            }
        }
        return _foundCounter;
    }
}
