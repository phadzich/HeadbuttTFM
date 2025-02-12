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

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        txtMaxJumps.text = "MAX: " + maxJumps.ToString();
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

    public void BreakAllHitBlocks()
    {
        foreach (BlockData _block in hitBlocks)
        {
            _block.jumpCount = 0;
            _block.gameObject.SetActive(false);
            _block.ShowHitIndicator(false);
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


}