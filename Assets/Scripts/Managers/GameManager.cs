using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Unity.Collections.AllocatorManager;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [Header("REFERENCIAS")]
    [SerializeField]
    public PlayerMovement playerMovement;
    [Header("HELMET")]
    public int maxJumps;
    public int levelJumpCount;
    public int maxHB;
    public int levelHBCount;

    [Header("COMBO ACTUAL")]
    [SerializeField]
    private List<ResourceBlock> hitBlocks;
    public ResourceData currentComboResource;
    public int currentComboCount;
    public int currentComboRequirement;

    private void Awake()
    {
        Instance = this;
    }

    public void CheckIfNewCombo(ResourceData _resourceData, ResourceBlock _resourceBlock)
    {
        //Si se salta sobre un recurso diferente, rompemos el combo
        if (currentComboResource != _resourceData)
        {
            ClearAllHitBlocks();
            currentComboResource = _resourceData;
            currentComboRequirement = _resourceData.hardness;
        }
        //Sea diferente o no, lo agregamos a la lista del combo
        AddBlockToHitBlocks(_resourceBlock);
    }

    public void CheckIfComboCompleted()
    {
        if (currentComboCount == currentComboResource.hardness)
        {
            MineAllHitBlocks();
        }

    }

    public void AddBlockToHitBlocks(ResourceBlock _newBlock)
    {
        if(!hitBlocks.Contains(_newBlock))
        {
            hitBlocks.Add(_newBlock);

            currentComboCount++;
            UpdateComboBlockIndicator(currentComboRequirement - currentComboCount);
        }
        IncreaseLevelJumpCount(1);
    }

    public void UpdateComboBlockIndicator(int _comboDifference)
    {
        foreach (ResourceBlock _block in hitBlocks)
        {
            _block.UpdateBounceIndicator(_comboDifference);

        }
    }

    public void ClearAllHitBlocks()
    {
        foreach (ResourceBlock _block in hitBlocks)
        {
            _block.ShowHitIndicator(false);
        }
        hitBlocks.Clear();
        currentComboResource= null;
        currentComboCount = 0;

    }

    public void MineAllHitBlocks()
    {
        foreach (ResourceBlock _block in hitBlocks)
        {
            _block.Activate();
            
        }
        ClearAllHitBlocks();
    }

    public void IncreaseLevelJumpCount(int _jumps)
    {
        levelJumpCount += _jumps;


        HelmetManager.Instance.currentHelmet.UseBounce();


        if (HelmetManager.Instance.currentHelmet.isWornOut)
        {
            if (HelmetManager.Instance.HasHelmetsLeft)
            {
                HelmetManager.Instance.WearNextAvailableHelmet();
            } else
            {
                SceneManager.LoadScene("SampleScene");
            }
           
        }

    }

    public void IncreaseLevelHBCount(int _jumps)
    {
        levelHBCount += _jumps;

        HelmetManager.Instance.currentHelmet.UseHeadbutt();

    }

    public void RestartSublevelStats()
    {
        currentComboResource = null;
        currentComboCount = 0;
        levelJumpCount = 0;
        levelHBCount = 0;
        HelmetManager.Instance.ResetHelmetsStats();
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