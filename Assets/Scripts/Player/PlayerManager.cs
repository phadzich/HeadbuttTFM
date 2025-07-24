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
    //public PlayerHeadbutt playerHeadbutt;
    public int maxPlayerLives;
    public int currentPlayerLives;
    public DamageTakenIndicator damageTakenIndicator;
    public Action<int, int> PlayerLivesChanged;
    public bool onWaterShield = false;
    public GameObject shieldMesh;

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
        PlayerLivesChanged?.Invoke(currentPlayerLives, maxPlayerLives);
    }

    public void AddPlayerLives(int _amount)
    {
        currentPlayerLives += _amount;
        PlayerLivesChanged?.Invoke(currentPlayerLives, maxPlayerLives);
    }

    public void RemovePlayerLives(int _amount)
    {
        currentPlayerLives -= _amount;
        PlayerLivesChanged?.Invoke(currentPlayerLives, maxPlayerLives);
        playerEmojis.DamagedEmoji();
        if (currentPlayerLives <= 0)
        {
            LevelManager.Instance.checkpointSystem.RestoreToLastCheckpoint();

        }
    }

    public void ActivateShield(float _duration)
    {
        if (onWaterShield) return;

        onWaterShield = true;
        shieldMesh.SetActive(true);

        // Empieza corrutina con duracion
        var cor = StartTimer(_duration);
        StartCoroutine(cor);

        Debug.Log("SHIELD ACTIVO");
    }

    IEnumerator StartTimer(float duration)
    {
        Debug.Log("START");
        yield return new WaitForSeconds(duration);
        Debug.Log("END");
        DeactivateShield();
    }

    public void DeactivateShield()
    {
        onWaterShield = false;
        shieldMesh.SetActive(false);
        Debug.Log("SHIELD DESACTIVADO");
    }

    public void EnterMiningLevel()
    {
        playerAnimations.RotateBody(180);
        playerBounce.enabled = true;
    }

    public void EnterNPCLevel()
    {
        playerAnimations.RotateBody(0);
        playerBounce.enabled = false;
    }
}


