using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Unity.Collections.AllocatorManager;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public LevelMovement levelMovement;

    [SerializeField]
    private List<BlockData> hitBlocks;

    public string currentComboBlock;
    public int currentComboCount;
    public int levelJumpCount;
    public int maxJumps;


    public TextMeshProUGUI txtJumpCounts;
    public TextMeshProUGUI txtMaxJumps;
    public TextMeshProUGUI txtRemainingJumps;

    public int levelHBCount;
    public int maxHB;
    public bool hasHeadbutts;

    public TextMeshProUGUI txtHBCounts;
    public TextMeshProUGUI txtMaxHB;
    public TextMeshProUGUI txtRemainingHB;

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        txtMaxJumps.text = "MAX: " + maxJumps.ToString();
        txtMaxHB.text = "MAX: " + maxHB.ToString();
    }

    public void AddBlockToHitBlocks(BlockData _newBlock)
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
        foreach (BlockData _block in hitBlocks)
        {
            _block.ShowHitIndicator(false);
        }
        hitBlocks.Clear();
        currentComboBlock = null;
        currentComboCount = 0;


    }

    public void MineAllHitBlocks()
    {
        foreach (BlockData _block in hitBlocks)
        {
            _block.jumpCount = 0;
            _block.GetMined();
            
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




}