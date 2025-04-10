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

        txtJumpCounts.text = "JUMPS: " + levelJumpCount.ToString();
        txtHBCounts.text = "HEADBUTTS: " + levelHBCount.ToString();


        txtMaxJumps.text = "MAX: " + HelmetManager.Instance.currentHelmet.baseHelmet.bounces.ToString();
        txtMaxHB.text = "MAX: " + HelmetManager.Instance.currentHelmet.baseHelmet.headbutts.ToString();

        txtRemainingJumps.text = "QUEDAN: " + HelmetManager.Instance.currentHelmet.remainingBounces.ToString();
        txtRemainingHB.text = "QUEDAN: " + HelmetManager.Instance.currentHelmet.remainingHeadbutts.ToString();

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

        HelmetManager.Instance.currentHelmet.UseBounce();

        txtRemainingJumps.text = "QUEDAN: " + HelmetManager.Instance.currentHelmet.remainingBounces.ToString();

        if (HelmetManager.Instance.currentHelmet.remainingBounces == 0)
        {
            SceneManager.LoadScene("SampleScene");
        }

    }

    public void IncreaseLevelHBCount(int _jumps)
    {
        levelHBCount += _jumps;
        txtHBCounts.text = "HEADBUTTS: " + levelHBCount.ToString();

        HelmetManager.Instance.currentHelmet.UseHeadbutt();

        txtRemainingHB.text = "QUEDAN: " + HelmetManager.Instance.currentHelmet.remainingHeadbutts.ToString();

        if (HelmetManager.Instance.currentHelmet.remainingHeadbutts == 0)
        {
            //levelHBCount = HelmetManager.Instance.currentHelmet.baseHelmet.headbutts;
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
        HelmetManager.Instance.ResetHelmetsStats();
        hasHeadbutts = true;

        // Actualizar UI TEMPORAL

        txtJumpCounts.text = "JUMPS: " + levelJumpCount.ToString();
        txtHBCounts.text = "HEADBUTTS: " + levelHBCount.ToString();


        txtMaxJumps.text = "MAX: " + HelmetManager.Instance.currentHelmet.baseHelmet.bounces.ToString();
        txtMaxHB.text = "MAX: " + HelmetManager.Instance.currentHelmet.baseHelmet.headbutts.ToString();

        txtRemainingJumps.text = "QUEDAN: " + HelmetManager.Instance.currentHelmet.remainingBounces.ToString();
        txtRemainingHB.text = "QUEDAN: " + HelmetManager.Instance.currentHelmet.remainingHeadbutts.ToString();
    }


}