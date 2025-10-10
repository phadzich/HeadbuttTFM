using System;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using Unity.Cinemachine;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.InputSystem;

public class UIManager : MonoBehaviour
{

    public static UIManager Instance;
    public GameObject currentOpenUI;

    [Header("NPCs")]
    public GameObject NPCCraftPanel;
    public GameObject NPCTraderPanel;
    public ExchangePanelUI NPCUpgradeExchanger;
    public InventoryPanelUI InventoryPanel;
    public GameObject NPCBlasterPanel;
    public ShopPanel shopPanel;
    public CraftingPanel craftingPanel;

    [Header("FRONTEND")]
    public GameObject startPanel;
    public FrontEndFrame frontEndFrame;
    public StartMenuUI mainMenuPanel;

    [Header("HUD")]
    public GameObject HUDCanvas;
    public CurrentHelmetsHUD currentHelmetsHUD;
    public ActiveItemHUD activeItemHUD;
    public ResourcesPanel resourcesPanel;
    public SublevelObjectivesHUD sublevelObjsHUD;
    public HBPointsHUD hbPointsHUD;
    public CoinsHUD coinsHUD;
    public GameObject NPCKeyHUD;

    [Header("POPUPS")]
    public PopupUI popupUI;

    [Header("PLAYER")]
    public RemainingBlocksIndicator remainingBlockIndicator;
    public CinemachineCamera currentCam;
    public ExitFloatinIndicatorHUD exitFloatinIndicatorHUD;

    [Header("SYSTEMS")]
    public DialogueSystem dialogueSystem;

    [Header("LIBRARIES")]
    public IconsLibrary iconsLibrary;
    public List<Sprite> elementIcons;
    public List<Color> elementColors;

    [Header("GAMEOVER")]
    public GameObject gameOverPanel;

    public Action<bool> onNPCUIChanged;

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
        SuscribeToHelmetInstances();
        //InputManager.Instance.SwitchInputToPlayer();
        mainMenuPanel.OpenMainMenu();
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
        ResourceManager.Instance.coinTrader.onCoinsChanged += OnCoinsChanged;

        //INVENTORY EVENTS
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
        ResourceManager.Instance.coinTrader.onCoinsChanged -= OnCoinsChanged;
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

    private void OnCoinsChanged(int _current)
    {
        coinsHUD.UpdateAmount(_current);
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
        if (currentCam != null)
        {
            currentCam.Priority = 0;
        }
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
            exitFloatinIndicatorHUD.gameObject.SetActive(true);
            exitFloatinIndicatorHUD.exitDoor = LevelManager.Instance.currentExitDoor.transform;
        }
        else
        {
            sublevelObjsHUD.ShowCheckpoint();
            exitFloatinIndicatorHUD.gameObject.SetActive(false);
        }


    }


    private void OnHelmetEquipped(HelmetInstance _helmInstance)
    {
        currentHelmetsHUD.EquipHelmet(_helmInstance);
    }

    private void OnHelmetInstanceDataChanged(HelmetInstance _instance)
    {
        //Debug.Log(_instance);
        if (currentHelmetsHUD.FindHUDbyInstance(_instance) != null)
        {
            currentHelmetsHUD.FindHUDbyInstance(_instance).UpdateDurability(_instance.currentDurability, _instance.durability);
        }

        craftingPanel.UpdateHelmetList();
        craftingPanel.infoPanel.UpdateInfoCard(_instance);
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
        Debug.Log("OpeningNPCUI");
        HUDCanvas.SetActive(false);
        onNPCUIChanged?.Invoke(true);
        switch (_type)
        {
            case NPCType.Crafter:
                SoundManager.PlaySound(UIType.OPEN_CRAFT);
                NPCCraftPanel.SetActive(true);
                currentOpenUI = NPCCraftPanel.gameObject;
                frontEndFrame.OpenFrame("FORGER", "Craft, upgrade and equip helmets.", UIManager.Instance.iconsLibrary.npcForger);
                break;
            case NPCType.Elevator:
                SoundManager.PlaySound(UIType.OPEN_ELEVATOR);
                NPCBlasterPanel.SetActive(true);
                currentOpenUI = NPCBlasterPanel.gameObject;
                frontEndFrame.OpenFrame("BLASTER", "Launch yourself to other checkpoints.", UIManager.Instance.iconsLibrary.npcElevator);
                break;
            case NPCType.Inventory:
                SoundManager.PlaySound(UIType.OPEN_CHEST);
                InventoryPanel.gameObject.SetActive(true);
                currentOpenUI = InventoryPanel.gameObject;
                frontEndFrame.OpenFrame("STORAGE", "Add or remove items from your backpack", UIManager.Instance.iconsLibrary.npcInventory);
                break;
        }
        PlayerManager.Instance.ShowPlayerMesh(false);
        NPCKeyHUD.SetActive(false);
    }

    public void CloseCurrentOpenUI(InputAction.CallbackContext context)
    {
        //Debug.Log("E");
        onNPCUIChanged?.Invoke(false);
        if (context.performed) // ya se solt� y volvi� a presionar
        {
            //Debug.Log("Closing UI");
            if (currentOpenUI != null)
            {
                HUDCanvas.SetActive(true);
                currentOpenUI.SetActive(false);
                currentOpenUI = null;
                frontEndFrame.CloseFrame();
            }
        }
        PlayerManager.Instance.ShowPlayerMesh(true);
        NPCKeyHUD.SetActive(true);

    }
    public void OpenShopUI(int _id)
    {
        SoundManager.PlaySound(UIType.OPEN_SHOP);
        PlayerManager.Instance.ShowPlayerMesh(false);
        NPCKeyHUD.SetActive(false);
        //Debug.Log("OpeningSHOP UI");
        HUDCanvas.SetActive(false);
        Shop _currentShop = ShopManager.Instance.ShopById(_id);
        shopPanel.OpenShop(_currentShop);
        currentOpenUI = shopPanel.gameObject;
        frontEndFrame.OpenFrame("SHADY SHOP", _currentShop.shopName, UIManager.Instance.iconsLibrary.npcShop);
        onNPCUIChanged?.Invoke(true);
    }

    public void ShowNPCKey(bool _show)
    {

        NPCKeyHUD.SetActive(_show);
    }

    public void OpenSettings()
    {
        GameManager.Instance.PauseGame(true);
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

    public void ShowGameOver()
    {
        Debug.Log("GO");
        gameOverPanel.SetActive(true);
        currentOpenUI = gameOverPanel;
    }

    public void HideGameOver()
    {
        gameOverPanel.SetActive(false);
        currentOpenUI = null;
    }
}
