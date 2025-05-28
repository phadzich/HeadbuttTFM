using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MatchManager : MonoBehaviour
{
    public static MatchManager Instance;
    public bool lastBounceNeutral=true;
    public bool lastBounceCombo;
    [Header("COMBO ACTUAL")]
    [SerializeField]
    private List<ResourceBlock> hitBlocks;
    public ResourceData currentComboResource;
    public int currentComboCount;
    public int currentComboRequirement;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            Debug.Log("MatchManager Awake");
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ResetComboStats()
    {
        currentComboResource = null;
        currentComboCount = 0;
        lastBounceNeutral = true;
    }

    public void BouncedOnNeutralBlock()
    {
        Debug.Log("NEUTRAL BLOCK BOUNCE");
        //SI NO VIENE DE UN SALTO NEUTRAL, VIENE DE UN RESOURCE. ROMPEMOS COMBO
        if (!lastBounceNeutral)
        {
            if (!lastBounceCombo)
            {
                IncreaseDamageCount(1);
            }
            //Debug.Log("VIENE DE UN RESOURCE");
            ClearAllHitBlocks();
            currentComboResource = null;
            currentComboRequirement = 0;
            //Debug.Log("DAMAGE");


            UIManager.Instance.currentMatchPanel.EndCurrentCombo();
            UIManager.Instance.remainingBlockIndicator.ToggleIndicator(false);
        }
        else
        {
            //Debug.Log("VIENE DE OTRO NEUTRAL");
        }
        lastBounceNeutral = true;
    }

    public void BouncedOnResourceBlock(ResourceData _resourceData, ResourceBlock _resourceBlock)
    {
        //Debug.Log("RES BLOCK BOUNCE");
        CheckIfNewCombo(_resourceData,_resourceBlock);
        AddBlockToHitBlocks(_resourceBlock);
    }

    public void CheckIfNewCombo(ResourceData _resourceData, ResourceBlock _resourceBlock)
    {
        //SALTA SOBRE UN RECURSO DIFERENTE
        if (currentComboResource != _resourceData)
        {
            //Debug.Log("NEW RESBLOCK");
            UIManager.Instance.currentMatchPanel.StartNewCombo(_resourceData,1);

            ClearAllHitBlocks();
            currentComboResource = _resourceData;
            currentComboRequirement = _resourceData.hardness;
            UIManager.Instance.remainingBlockIndicator.ToggleIndicator(true);
            UIManager.Instance.remainingBlockIndicator.UpdateIndicator(_resourceData, _resourceData.hardness);
            if (!lastBounceCombo&&!lastBounceNeutral)
            {
                //Debug.Log(lastBounceNeutral);
                //Debug.Log("DAMAGE!");
                IncreaseDamageCount(1);
                UIManager.Instance.currentMatchPanel.ChangeCurrentCombo();
            }
            lastBounceNeutral = false;
            lastBounceCombo = false;
        }

    }

    public void CheckIfComboCompleted()
    {
        if (currentComboCount == currentComboResource.hardness)
        {
            MineAllHitBlocks();
            lastBounceCombo = true;
            UIManager.Instance.currentMatchPanel.CompleteCurrentCombo();
            UIManager.Instance.remainingBlockIndicator.ToggleIndicator(false);
        }

    }

    public void AddBlockToHitBlocks(ResourceBlock _newBlock)
    {
        if (!hitBlocks.Contains(_newBlock))
        {
            hitBlocks.Add(_newBlock);

            currentComboCount++;
            UpdateComboBlockIndicator(currentComboRequirement - currentComboCount);
            UIManager.Instance.remainingBlockIndicator.ToggleIndicator(true);
            UIManager.Instance.remainingBlockIndicator.UpdateIndicatorCount(currentComboRequirement - currentComboCount);
            UIManager.Instance.currentMatchPanel.IncreaseCurrentCombo(currentComboCount);
        }
        lastBounceCombo = false;
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
        //currentComboResource = null;
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

    public void IncreaseDamageCount(int _damage)
    {

        HelmetManager.Instance.currentHelmet.TakeDamage();
        UIManager.Instance.damageTakenIndicator.AnimateDamage();

        if (HelmetManager.Instance.currentHelmet.isWornOut)
        {
            if (HelmetManager.Instance.HasHelmetsLeft)
            {
                HelmetManager.Instance.WearNextAvailableHelmet();
            }
            else
            {
                SceneManager.LoadScene("SampleScene");
            }

        }

    }
}
