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
    public LevelMovement levelMovement;

    [Header("HELMET")]
    public int maxJumps;
    public int levelJumpCount;
    public int maxHB;
    public int levelHBCount;
    public bool hasHeadbutts;

    [Header("COMBO ACTUAL")]
    [SerializeField]
    private List<ResourceBlock> hitBlocks;
    public ResourceData currentComboResource;
    public int currentComboCount;

    [Header("TEMP UI")]
    public TextMeshProUGUI txtJumpCounts;
    public TextMeshProUGUI txtMaxJumps;
    public TextMeshProUGUI txtRemainingJumps;
    public TextMeshProUGUI txtHBCounts;
    public TextMeshProUGUI txtMaxHB;
    public TextMeshProUGUI txtRemainingHB;

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        txtMaxJumps.text = "MAX: " + maxJumps.ToString();
        txtMaxHB.text = "MAX: " + maxHB.ToString();
    }

    public void CheckIfNewCombo(ResourceData _resourceData, ResourceBlock _resourceBlock)
    {
        //Si se salta sobre un recurso diferente, rompemos el combo
        if (currentComboResource != _resourceData)
        {
            ClearAllHitBlocks();
            currentComboResource = _resourceData;
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
        }
        IncreaseLevelJumpCount(1);
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
        txtJumpCounts.text = "JUMPS: " + levelJumpCount.ToString();

        var _remaining = maxJumps - levelJumpCount;

        txtRemainingJumps.text = "QUEDAN: " + _remaining.ToString();

        if (levelJumpCount == maxJumps)
        {
            SceneManager.LoadScene("SampleScene");
        }

    }

    public void IncreaseLevelHBCount(int _jumps)
    {
        levelHBCount += _jumps;
        txtHBCounts.text = "HEADBUTTS: " + levelHBCount.ToString();

        var _remaining = maxHB - levelHBCount;

        txtRemainingHB.text = "QUEDAN: " + _remaining.ToString();

        if (levelHBCount >= maxHB)
        {
            levelHBCount = maxHB;
            hasHeadbutts = false;
        }
        else
        {
            hasHeadbutts = true;
        }

    }

    public void RestartSublevelStats()
    {
               currentComboResource = null;
    currentComboCount = 0;
    levelJumpCount = 0;
        levelHBCount = 0;
        hasHeadbutts = true;
}


}