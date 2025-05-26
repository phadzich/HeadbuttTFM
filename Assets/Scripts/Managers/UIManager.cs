using System;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    // WIP, HAY QUE CREAR EVENTOS Y SUSCRIBIRNOS A ELLOS


    public EquippedHelmetsPanel equippedHelmetsPanel;
    public HeadbuttsPanel headbuttsPanel;

    public static UIManager Instance;
    public ResourcesPanel resourcesPanel;
    public XPPanel experiencePanel;
    public SublevelPanel sublevelPanel;
    public GameObject craftButton;
    public CurrentMatchPanel currentMatchPanel;
    public LivesPanel livesPanel;
    public RemainingBlocksIndicator remainingBlockIndicator;
    public DamageTakenIndicator damageTakenIndicator;

    public TextMeshProUGUI totalBouncesTXT;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            Debug.Log("UIManager Awake");
        }
        else
        {
            Destroy(gameObject);
        }

    }

    private void Start()
    {
        //POR AHORA HARDCODED
        headbuttsPanel.UpdateUsedHeadbutts(HelmetManager.Instance.helmetsEquipped[0]);

        equippedHelmetsPanel.InstanceEquippedIndicators(HelmetManager.Instance.helmetsEquipped);
        equippedHelmetsPanel.UpdateWearingHelmet(HelmetManager.Instance.helmetsEquipped[0]);

        //craftButton.SetActive(false);
    }

    private void OnEnable()
    {
        HelmetManager.Instance.onHelmetsEquipped += OnHelmetsEquipped;
        ResourceManager.Instance.onOwnedResourcesChanged += OnOwnedResourcesChanged;
        //HelmetManager.Instance.onHelmetInstanceDataChanged += OnHelmetInstanceDataChanged;
        SuscribeToHelmetInstances();
        HelmetManager.Instance.onWearHelmetChanged += OnWearHelmetChanged;
        XPManager.Instance.XPChanged += OnXPChanged;
        XPManager.Instance.LeveledUp += OnLevelUp;
        LevelManager.Instance.onSublevelEntered += OnSublevelEntered;
        LevelManager.Instance.onSublevelBlocksMined += OnSublevelBlocksMined;
    }
    private void OnDisable()
    {
        HelmetManager.Instance.onHelmetsEquipped -= OnHelmetsEquipped;
        ResourceManager.Instance.onOwnedResourcesChanged -= OnOwnedResourcesChanged;
        //HelmetManager.Instance.onHelmetInstanceDataChanged -= OnHelmetInstanceDataChanged;
        UnsuscribeToHelmetInstances();
        HelmetManager.Instance.onWearHelmetChanged -= OnWearHelmetChanged;
        XPManager.Instance.XPChanged -= OnXPChanged;
        XPManager.Instance.LeveledUp -= OnLevelUp;
        LevelManager.Instance.onSublevelEntered -= OnSublevelEntered;
        LevelManager.Instance.onSublevelBlocksMined -= OnSublevelBlocksMined;
    }

    private void SuscribeToHelmetInstances()
    {
        foreach(HelmetInstance _helmInstance in HelmetManager.Instance.helmetsEquipped)
        {
            _helmInstance.HelmetInstanceChanged += OnHelmetInstanceDataChanged;
        }
    }

    private void UnsuscribeToHelmetInstances()
    {
        foreach (HelmetInstance _helmInstance in HelmetManager.Instance.helmetsEquipped)
        {
            _helmInstance.HelmetInstanceChanged -= OnHelmetInstanceDataChanged;
        }
    }

    private void OnXPChanged(int _current, int _max)
    {
        experiencePanel.UpdateXP(_current, _max);
    }

    private void OnLevelUp(int _currentLVL)
    {
        experiencePanel.UpdateLVL(_currentLVL);
        //craftButton.SetActive(true);
    }

    private void OnOwnedResourcesChanged()
    {
        equippedHelmetsPanel.CheckIfUpgradesAvailable();
    }
    private void OnSublevelEntered()
    {
        Debug.Log($"UPDATING PANL");
        sublevelPanel.UpdateSublevel();
    }

    private void OnSublevelBlocksMined()
    {
        sublevelPanel.UpdateGoals();
    }

    private void OnHelmetsEquipped(List<HelmetInstance> _helmetList)
    {
        UnsuscribeToHelmetInstances();
        SuscribeToHelmetInstances();
        equippedHelmetsPanel.InstanceEquippedIndicators(_helmetList);
    }

    private void OnHelmetInstanceDataChanged(HelmetInstance _instance)
    {
        //Debug.Log("INSTANCE DATA CHANGED"+ _instance.id);
            equippedHelmetsPanel.UpdateHelmetInstanceInfo(_instance);

        if(_instance == HelmetManager.Instance.currentHelmet)
        {
            if (_instance.maxHeadbutts > 0)
            {
                headbuttsPanel.UpdateUsedHeadbutts(_instance);
            }
        }

        livesPanel.UpdateLivesInfo(CalculateTotalBounces());

    }

    private void OnWearHelmetChanged(HelmetInstance _instance)
    {
        equippedHelmetsPanel.UpdateWearingHelmet(_instance);
        headbuttsPanel.UpdateUsedHeadbutts(_instance);

    }

    int CalculateTotalBounces()
    {
        int addedBounces = 0;
        foreach(HelmetInstance _helmetInstance in HelmetManager.Instance.helmetsEquipped)
        {
            addedBounces += _helmetInstance.remainingBounces;
        }

        return addedBounces;
    }

}
