using System;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    // WIP, HAY QUE CREAR EVENTOS Y SUSCRIBIRNOS A ELLOS


    public CurrentHelmetHUD currentHelmetHUD;


    public static UIManager Instance;
    public ResourcesPanel resourcesPanel;
    public XPPanel experiencePanel;
    public SublevelPanel sublevelPanel;
    public GameObject craftButton;
    public CurrentMatchPanel currentMatchPanel;
    public LivesPanel livesPanel;
    public RemainingBlocksIndicator remainingBlockIndicator;
    public GameObject NPCCraftPanel;
    public GameObject NPCUpgradePanel;
    public GameObject NPCElevatorPanel;

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
        Debug.Log("UIManager START");
        //POR AHORA HARDCODED
        //headbuttsPanel.UpdateUsedHeadbutts(HelmetManager.Instance.helmetsEquipped[0]);

        //equippedHelmetsPanel.InstanceEquippedIndicators(HelmetManager.Instance.helmetsEquipped);
        //equippedHelmetsPanel.UpdateWearingHelmet(HelmetManager.Instance.helmetsEquipped[0]);

        //craftButton.SetActive(false);
    }

    private void OnEnable()
    {
        Debug.Log("UIManager ENABLED");

        //HELMET EVENTS
        HelmetManager.Instance.onHelmetEquipped += OnHelmetEquipped;
        HelmetManager.Instance.onWearHelmetChanged += OnWearHelmetChanged;

        //XP EVENTS
        XPManager.Instance.XPChanged += OnXPChanged;
        XPManager.Instance.LeveledUp += OnLevelUp;

        //LEVEL EVENTS
        LevelManager.Instance.onSublevelEntered += OnSublevelEntered;
        LevelManager.Instance.onSublevelBlocksMined += OnSublevelBlocksMined;

        //PLAYER EVENTS
        PlayerManager.Instance.PlayerLivesChanged += OnPlayerLivesChanged;
    }
    private void OnDisable()
    {
        HelmetManager.Instance.onHelmetEquipped -= OnHelmetEquipped;
        HelmetManager.Instance.onWearHelmetChanged -= OnWearHelmetChanged;
        XPManager.Instance.XPChanged -= OnXPChanged;
        XPManager.Instance.LeveledUp -= OnLevelUp;
        LevelManager.Instance.onSublevelEntered -= OnSublevelEntered;
        LevelManager.Instance.onSublevelBlocksMined -= OnSublevelBlocksMined;
        PlayerManager.Instance.PlayerLivesChanged -= OnPlayerLivesChanged;
    }

    private void SuscribeToHelmetInstances()
    {
        UnsuscribeToHelmetInstances();
        foreach (HelmetInstance _helmInstance in HelmetManager.Instance.helmetsEquipped)
        {
            _helmInstance.HelmetInstanceChanged += OnHelmetInstanceDataChanged;
            _helmInstance.helmetXP.XPChanged += OnHelmetXPChanged;
            OnHelmetInstanceDataChanged(_helmInstance);
        }
    }

    private void UnsuscribeToHelmetInstances()
    {
        foreach (HelmetInstance _helmInstance in HelmetManager.Instance.helmetsEquipped)
        {
            _helmInstance.HelmetInstanceChanged -= OnHelmetInstanceDataChanged;
            _helmInstance.helmetXP.XPChanged -= OnHelmetXPChanged;
        }
    }

    private void OnHelmetXPChanged(HelmetXP _xpComp, HelmetInstance _instance)
    {
        //Debug.Log("OnHelmetXPChanged");
        currentHelmetHUD.UpdateLVLInfo(_instance);
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

    private void OnSublevelEntered()
    {
        sublevelPanel.UpdateSublevel();
    }

    private void OnSublevelBlocksMined()
    {
        sublevelPanel.UpdateGoals();
    }

    private void OnHelmetEquipped(HelmetInstance _helmInstance)
    {
        currentHelmetHUD.EquipHelmet(_helmInstance);
        SuscribeToHelmetInstances();
    }

    private void OnHelmetInstanceDataChanged(HelmetInstance _instance)
    {
        currentHelmetHUD.UpdateCurrentHelmetStats();

    }

    private void OnWearHelmetChanged(HelmetInstance _instance)
    {
        currentHelmetHUD.WearNewHelmet(_instance);
    }


    private void OnPlayerLivesChanged(int _current, int _max)
    {
        //livesPanel.UpdateLivesInfo(_current);
    }

    public void OpenNPCUI(NPCType _type)
    {
        switch (_type)
        {
            case NPCType.Crafter:
                NPCCraftPanel.SetActive(true);
                break;
            case NPCType.Upgrader:
                NPCUpgradePanel.SetActive(true);
                break;
            case NPCType.Elevator:
                NPCElevatorPanel.SetActive(true);
                break;

        }
    }
}
