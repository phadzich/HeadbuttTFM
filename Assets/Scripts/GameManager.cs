using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using static Unity.Collections.AllocatorManager;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField]
    private List<BlockData> hitBlocks;
    [SerializeField]
    public string currentComboBlock;
    public int currentComboCount;


    private void Awake()
    {
        instance = this;
    }

    public void AddBlockToHitBlocks(BlockData _newBlock)
    {
        hitBlocks.Add(_newBlock);
        currentComboCount++;
    }

    public void ClearAllHitBlocks()
    {
        foreach (BlockData _block in hitBlocks)
        {
            _block.ShowHitIndicator(false);
        }
        hitBlocks.Clear();
        currentComboBlock = null;
        currentComboCount=0;


    }

    public void BreakAllHitBlocks() {
    foreach(BlockData _block in  hitBlocks)
        {
            _block.jumpCount = 0;
            _block.gameObject.SetActive(false);
            _block.ShowHitIndicator(false);
        }
        ClearAllHitBlocks();
    }

}
