using System;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class UIManager : MonoBehaviour
{
    // WIP, HAY QUE CREAR EVENTOS Y SUSCRIBIRNOS A ELLOS


    public CurrentHelmetsHUD currentHelmetsHUD;


    public static UIManager Instance;
    public ResourcesPanel resourcesPanel;
    public XPPanel experiencePanel;
    public SublevelPanel sublevelPanel;
    public GameObject craftButton;
    public LivesPanel livesPanel;
    public RemainingBlocksIndicator remainingBlockIndicator;
    public GameObject NPCCraftPanel;
    public GameObject NPCUpgradePanel;
    public ExchangePanelUI NPCUpgradeExchanger;
    public GameObject NPCElevatorPanel;
    public GameObject startPanel;
    public TextMeshProUGUI totalBouncesTXT;
    public HBPointsHUD hbPointsHUD;
    public SpecialHeadbuttHUD specialHeadbuttHUD;
    public ExitFloatinIndicatorHUD exitFloatinIndicatorHUD;

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
        //startPanel.SetActive(true);
    }

    private void OnEnable()
    {
        Debug.Log("UIManager ENABLED");

        //HELMET EVENTS
        HelmetManager.Instance.onHelmetEquipped += OnHelmetEquipped;
        HelmetManager.Instance.onWearHelmetChanged += OnWearHelmetChanged;
        HelmetManager.Instance.onHelmetReplaced += OnHelmetReplaced;


        //XP EVENTS
        XPManager.Instance.LeveledUp += OnLevelUp;

        //LEVEL EVENTS
        LevelManager.Instance.onSublevelEntered += OnSublevelEntered;
        LevelManager.Instance.onSublevelBlocksMined += OnSublevelGoalsAdvanced;
        LevelManager.Instance.onKeysCollected += OnSublevelGoalsAdvanced;

        //PLAYER EVENTS
        PlayerManager.Instance.PlayerLivesChanged += OnPlayerLivesChanged;
        PlayerManager.Instance.playerHeadbutt.onHBPointsChanged += OnHBPointsChanged;

        ResourceManager.Instance.onOwnedResourcesChanged += OnOwnedResourcesChanged;
    }



    private void OnDisable()
    {
        //HELMET EVENTS
        HelmetManager.Instance.onHelmetEquipped -= OnHelmetEquipped;
        HelmetManager.Instance.onWearHelmetChanged -= OnWearHelmetChanged;
        HelmetManager.Instance.onHelmetReplaced -= OnHelmetReplaced;

        //XP EVENTS
        XPManager.Instance.LeveledUp -= OnLevelUp;

        //LEVEL EVENTS
        LevelManager.Instance.onSublevelEntered -= OnSublevelEntered;
        LevelManager.Instance.onSublevelBlocksMined -= OnSublevelGoalsAdvanced;
        LevelManager.Instance.onKeysCollected -= OnSublevelGoalsAdvanced;

        //PLAYER EVENTS
        PlayerManager.Instance.PlayerLivesChanged -= OnPlayerLivesChanged;
        PlayerManager.Instance.playerHeadbutt.onHBPointsChanged -= OnHBPointsChanged;

        ResourceManager.Instance.onOwnedResourcesChanged -= OnOwnedResourcesChanged;
    }

    private void SuscribeToHelmetInstances()
    {
        UnsuscribeToHelmetInstances();
        foreach (HelmetInstance _helmInstance in HelmetManager.Instance.helmetsEquipped)
        {
            _helmInstance.HelmetInstanceChanged += OnHelmetInstanceDataChanged;
            OnHelmetInstanceDataChanged(_helmInstance);
        }
    }

    private void UnsuscribeToHelmetInstances()
    {
        foreach (HelmetInstance _helmInstance in HelmetManager.Instance.helmetsEquipped)
        {
            _helmInstance.HelmetInstanceChanged -= OnHelmetInstanceDataChanged;
        }
    }

    private void OnHBPointsChanged(float _current, float _max)
    {
        hbPointsHUD.UpdateFill(_current, _max);
    }

    private void OnOwnedResourcesChanged()
    {
        //NPCUpgradeExchanger.PopulateButtons();
        currentHelmetsHUD.UpdateUpgradePanels();

    }

    private void OnLevelUp(int _currentLVL)
    {
        experiencePanel.UpdateLVL(_currentLVL);
        //craftButton.SetActive(true);
    }

    private void OnSublevelEntered(Sublevel _sublevel)
    {
        if (_sublevel.config is MiningSublevelConfig)
        {
            MiningSublevelConfig _config = _sublevel.config as MiningSublevelConfig;
            sublevelPanel.ChangeGoalType(_config.goalType);
            sublevelPanel.UpdateSublevel();
        }
        else
        {
            sublevelPanel.ShowCheckpoint();
        }

                
        exitFloatinIndicatorHUD.exitDoor = LevelManager.Instance.currentExitDoor.transform;
    }

    private void OnSublevelGoalsAdvanced()
    {
        sublevelPanel.UpdateGoals();
    }

    private void OnHelmetEquipped(HelmetInstance _helmInstance)
    {
        currentHelmetsHUD.EquipHelmet(_helmInstance);
        SuscribeToHelmetInstances();
    }

    private void OnHelmetInstanceDataChanged(HelmetInstance _instance)
    {
        currentHelmetsHUD.FindHUDbyInstance(_instance).UpdateDurability(_instance.currentDurability, _instance.durability);

    }

    private void OnWearHelmetChanged(HelmetInstance _instance)
    {
        currentHelmetsHUD.WearNewHelmet(_instance);
    }

    private void OnHelmetReplaced(HelmetInstance _instance, int _index)
    {
        currentHelmetsHUD.ReplaceHelmet(_instance, _index);
        SuscribeToHelmetInstances();
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

    public void ToggleExtraInfo(InputAction.CallbackContext context)
    {
        //CUANDO EL INPUT ESTA PERFORMED
        if (context.phase == InputActionPhase.Started)
        {
            currentHelmetsHUD.ToggleExtraInfo(true);
            return;
        }

        if (context.phase == InputActionPhase.Canceled)
        {
            currentHelmetsHUD.ToggleExtraInfo(false);
            return;
        }

    }
}
