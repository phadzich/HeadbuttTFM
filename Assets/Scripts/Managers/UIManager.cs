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
    public ShopPanel shopPanel;
    public ActiveItemHUD activeItemHUD;
    public CraftingPanel craftingPanel;
    public SublevelPanel sublevelPanel;
    public DialogueSystem dialogueSystem;
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
        SuscribeToHelmetInstances();
    }

    private void Start()
    {
        Debug.Log("UIManager START");
        //dialogueSystem.StartDialogue(LevelManager.Instance.currentSublevel.config.dialogueSequence);
        //startPanel.SetActive(true);
    }

    private void OnEnable()
    {
        Debug.Log("UIManager ENABLED");

        //HELMET EVENTS
        HelmetManager.Instance.onHelmetEquipped += OnHelmetEquipped;
        HelmetManager.Instance.onHelmetsSwapped += OnHelmetSwap;
        HelmetManager.Instance.onWearHelmetChanged += OnWearHelmetChanged;

        //LEVEL EVENTS
        LevelManager.Instance.onSublevelEntered += OnSublevelEntered;
        LevelManager.Instance.onSublevelBlocksMined += OnSublevelGoalsAdvanced;
        LevelManager.Instance.onKeysCollected += OnSublevelGoalsAdvanced;

        //PLAYER EVENTS
        PlayerManager.Instance.playerHeadbutt.onHBPointsChanged += OnHBPointsChanged;
        ResourceManager.Instance.onOwnedResourcesChanged += OnOwnedResourcesChanged;

        //PLAYER EVENTS
        InventoryManager.Instance.ItemCycled += OnEquippedItemCycled;
        InventoryManager.Instance.ItemEquipped += OnEquippedItemCycled;
        InventoryManager.Instance.ItemConsumed += OnEquippedItemCycled;
    }



    private void OnDisable()
    {
        HelmetManager.Instance.onHelmetEquipped -= OnHelmetEquipped;
        HelmetManager.Instance.onWearHelmetChanged -= OnWearHelmetChanged;
        HelmetManager.Instance.onHelmetsSwapped -= OnHelmetSwap;
        LevelManager.Instance.onSublevelEntered -= OnSublevelEntered;
        LevelManager.Instance.onSublevelBlocksMined -= OnSublevelGoalsAdvanced;
        LevelManager.Instance.onKeysCollected -= OnSublevelGoalsAdvanced;
        ResourceManager.Instance.onOwnedResourcesChanged -= OnOwnedResourcesChanged;
        PlayerManager.Instance.playerHeadbutt.onHBPointsChanged -= OnHBPointsChanged;
        InventoryManager.Instance.ItemCycled -= OnEquippedItemCycled;
        InventoryManager.Instance.ItemEquipped -= OnEquippedItemCycled;
        InventoryManager.Instance.ItemConsumed -= OnEquippedItemCycled;
    }

    public void SuscribeToHelmetInstances()
    {
        UnsuscribeToHelmetInstances();
        foreach (HelmetInstance _helmInstance in HelmetManager.Instance.allHelmets)
        {
            _helmInstance.HelmetInstanceChanged += OnHelmetInstanceDataChanged;
            //OnHelmetInstanceDataChanged(_helmInstance);
        }
    }

    private void UnsuscribeToHelmetInstances()
    {
        foreach (HelmetInstance _helmInstance in HelmetManager.Instance.allHelmets)
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

    private void OnEquippedItemCycled(Item _item, int _amount)
    {

        if (_item != null)
        {
            activeItemHUD.ChangeActiveItem(_item, _amount);
        }
        else
        {
            activeItemHUD.DisableUI();
        }

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
    }

    private void OnHelmetInstanceDataChanged(HelmetInstance _instance)
    {
        //currentHelmetsHUD.FindHUDbyInstance(_instance).UpdateDurability(_instance.currentDurability, _instance.durability);
        craftingPanel.UpdateHelmetList();
        craftingPanel.UpdateInfoCard(_instance);
    }

    private void OnHelmetSwap(int _index)
    {
        craftingPanel.UpdateHelmetList();
        var _newHelmet = HelmetManager.Instance.helmetsEquipped[_index];
        currentHelmetsHUD.equippedHelmetHUDs[_index].LoadHelmet(_newHelmet);
    }

    private void OnWearHelmetChanged(HelmetInstance _instance)
    {
        currentHelmetsHUD.WearNewHelmet(_instance);
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

    public void OpenShopUI(int _id)
    {
        shopPanel.OpenShop(ShopManager.Instance.ShopById(_id));
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
