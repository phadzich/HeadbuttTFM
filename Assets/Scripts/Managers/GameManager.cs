using NUnit.Framework;
using PrimeTween;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Unity.Collections.AllocatorManager;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int globalSeed;

    [Header("REFERENCIAS")]
    [SerializeField]
    public PlayerMovement playerMovement;


    private void Awake()
    {
        Instance = this;
        PrimeTweenConfig.warnEndValueEqualsCurrent = false;
        PrimeTweenConfig.warnZeroDuration = false;
        PrimeTweenConfig.warnTweenOnDisabledTarget = false;
    }

    public void RestartScene()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void NewGame()
    {
        Debug.Log("STARTING NEW GAME");
        ResourceManager.Instance.NewGame();
        LevelManager.Instance.NewGame();

        PlayerManager.Instance.NewGame();
        ShopManager.Instance.NewGame();
        InventoryManager.Instance.NewGame();
        UIManager.Instance.NewGame();
        HelmetManager.Instance.NewGame();
    }
    public void PauseGame(bool _isPaused)
    {
        if (_isPaused)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }
}