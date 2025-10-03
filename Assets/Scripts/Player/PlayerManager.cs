using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;

    [Header("Componentes")]
    public PlayerStates playerStates;
    public PlayerMovement playerMovement;
    public PlayerCamera playerCamera;
    public PlayerAnimations playerAnimations;
    public PlayerBounce playerBounce;
    public PlayerEmojis playerEmojis;
    public PlayerHeadbutt playerHeadbutt;
    public PlayerEffects playerEffects;

    public int maxPlayerLives;
    public int currentPlayerLives;
    public DamageTakenIndicator damageTakenIndicator;
    public Action<int, int> PlayerLivesChanged;
    public GameObject shieldPrefab;
    public GameObject activeShield;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            Debug.Log("PlayerManager Awake");
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        Debug.Log("PlayerManager START");
    }
    public void MaxUpLives()
    {
        currentPlayerLives = maxPlayerLives;
        PlayerLivesChanged?.Invoke(currentPlayerLives, maxPlayerLives);
    }

    public void AddMaxLives(int _amount)
    {
        maxPlayerLives += _amount;
        AddPlayerLives(1);
        PlayerLivesChanged?.Invoke(currentPlayerLives, maxPlayerLives);
    }

    public void AddPlayerLives(int _amount)
    {
        currentPlayerLives += _amount;
        PlayerLivesChanged?.Invoke(currentPlayerLives, maxPlayerLives);
    }

    public void RemovePlayerLives(int _amount)
    {
        CombatLogHUD.Instance.AddLog(UIManager.Instance.iconsLibrary.npcForger, $"{HelmetManager.Instance.currentHelmet.baseHelmet.helmetName} <b>BROKEN</b>!");

        currentPlayerLives -= _amount;
        PlayerLivesChanged?.Invoke(currentPlayerLives, maxPlayerLives);
        playerEmojis.DamagedEmoji();
        if (currentPlayerLives <= 0)
        {
            playerStates.ChangeState(PlayerMainStateEnum.Dead);
        }
        
    }

    public void ActivateShield()
    {
        GameObject shieldGO = Instantiate(shieldPrefab, transform.GetChild(0));
        shieldGO.transform.localPosition = Vector3.zero;
        activeShield = shieldGO;
    }

    public void DeactivateShield()
    {
        Destroy(activeShield);
    }

    
    public void EnterMiningLevel()
    {
        playerAnimations.RotateBody(180);
        playerStates.ChangeState(PlayerMainStateEnum.FallingIntoMINE);
        playerHeadbutt.ChangeHBpoints(0);
    }

    public void ShowPlayerMesh(bool _value)
    {
        playerStates.gameObject.SetActive(_value);
    }

    public void EnterNPCLevel()
    {
        playerAnimations.RotateBody(0);
        MaxUpLives();
        playerStates.ChangeState(PlayerMainStateEnum.FallingIntoNPC);
    }

    public void EnterNewLevel()
    {
        playerMovement.RespawnPlayer();
    }
}


