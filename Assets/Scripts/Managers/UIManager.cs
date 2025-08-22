using System;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class UIManager : MonoBehaviour
{

    public static UIManager Instance;
    public GameObject currentOpenUI;

    [Header("NPCs")]
    public GameObject NPCCraftPanel;
    public GameObject NPCUpgradePanel;
    public ExchangePanelUI NPCUpgradeExchanger;
    public InventoryPanelUI NPCInventoryPanel;
    public GameObject NPCElevatorPanel;
    public ShopPanel shopPanel;
    public CraftingPanel craftingPanel;

    [Header("FRONTEND")]
    public GameObject startPanel;

    [Header("HUD")]
    public CurrentHelmetsHUD currentHelmetsHUD;
    public ActiveItemHUD activeItemHUD;
    public ResourcesPanel resourcesPanel;
    public SublevelObjectivesHUD sublevelObjsHUD;
    public HBPointsHUD hbPointsHUD;
    public SpecialHeadbuttHUD specialHeadbuttHUD;


    [Header("PLAYER")]
    public RemainingBlocksIndicator remainingBlockIndicator;
    public CinemachineCamera currentCam;
    public ExitFloatinIndicatorHUD exitFloatinIndicatorHUD;

    [Header("SYSTEMS")]
    public DialogueSystem dialogueSystem;


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


        //PLAYER EVENTS
        PlayerManager.Instance.playerHeadbutt.onHBPointsChanged += OnHBPointsChanged;
        ResourceManager.Instance.onOwnedResourcesChanged += OnOwnedResourcesChanged;

        //PLAYER EVENTS
        InventoryManager.Instance.itemsInventory.ItemCycled += OnEquippedItemCycled;
        InventoryManager.Instance.itemsInventory.ItemEquipped += OnEquippedItemCycled;
        InventoryManager.Instance.itemsInventory.ItemConsumed += OnEquippedItemCycled;
    }



    private void OnDisable()
    {
        HelmetManager.Instance.onHelmetEquipped -= OnHelmetEquipped;
        HelmetManager.Instance.onWearHelmetChanged -= OnWearHelmetChanged;
        HelmetManager.Instance.onHelmetsSwapped -= OnHelmetSwap;
        LevelManager.Instance.onSublevelEntered -= OnSublevelEntered;
        ResourceManager.Instance.onOwnedResourcesChanged -= OnOwnedResourcesChanged;
        PlayerManager.Instance.playerHeadbutt.onHBPointsChanged -= OnHBPointsChanged;
        InventoryManager.Instance.itemsInventory.ItemCycled -= OnEquippedItemCycled;
        InventoryManager.Instance.itemsInventory.ItemEquipped -= OnEquippedItemCycled;
        InventoryManager.Instance.itemsInventory.ItemConsumed -= OnEquippedItemCycled;
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

    public void ActivateCam(CinemachineCamera _newCam)
    {
        currentCam = _newCam;
      _newCam.Priority = 20;
    }

    public void DeactivateCurrentCam()
    {
        currentCam.Priority = 0;
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
            sublevelObjsHUD.OnSublevelEntered(_sublevel);
        }
        else
        {
            sublevelObjsHUD.ShowCheckpoint();
        }

        exitFloatinIndicatorHUD.exitDoor = LevelManager.Instance.currentExitDoor.transform;
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
        hbPointsHUD.UpdateHBIcon(_instance);
    }

    public void OpenNPCUI(NPCType _type)
    {
        switch (_type)
        {
            case NPCType.Crafter:
                NPCCraftPanel.SetActive(true);
                currentOpenUI = NPCCraftPanel.gameObject;
                break;
            case NPCType.Upgrader:
                NPCUpgradePanel.SetActive(true);
                currentOpenUI = NPCUpgradePanel.gameObject;
                break;
            case NPCType.Elevator:
                NPCElevatorPanel.SetActive(true);
                currentOpenUI = NPCElevatorPanel.gameObject;
                break;
            case NPCType.Inventory:
                NPCInventoryPanel.gameObject.SetActive(true);
                currentOpenUI = NPCInventoryPanel.gameObject;
                InventoryManager.Instance.itemsInventory.OpenUI();
                break;
        }
    }

    public void CloseCurrentOpenUI()
    {
        currentOpenUI.SetActive(false);
        currentOpenUI = null;
    }
    public void OpenShopUI(int _id)
    {
        shopPanel.OpenShop(ShopManager.Instance.ShopById(_id));
        currentOpenUI = shopPanel.gameObject;
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
