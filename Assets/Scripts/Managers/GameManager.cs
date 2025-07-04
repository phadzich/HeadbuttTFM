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
    public void RestartSublevelStats()
    {
        //MatchManager.Instance.ResetComboStats();

        //HelmetManager.Instance.ResetHelmetsStats();
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