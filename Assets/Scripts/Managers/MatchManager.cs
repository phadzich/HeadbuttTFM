using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MatchManager : MonoBehaviour
{
    public static MatchManager Instance;

    [Header("COMBO ACTUAL")]
    [SerializeField]
    private List<ResourceBlock> currentChainBlocks;
    public ResourceData currentChainResource;
    public ResourceData bouncedResource;
    public ResourceBlock bouncedResourceBlock;

    public int currentStreak;
    public bool lastBounceChained;

    [Header("SFX")]
    public AudioClip chainFailSound;
    public AudioClip floorBlockSound;
    private AudioSource audioSource;

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

    private void Start()
    {
        Debug.Log("MatchManager START");
        RestartMatches();
        audioSource = GetComponent<AudioSource>();
    }

    public void RestartMatches()
    {
        EndStreak();
        EndCurrentChain();

    }
    public void ResourceBounced(ResourceBlock _resBlock)
    {
        bouncedResourceBlock = _resBlock;
        bouncedResource = bouncedResourceBlock.resourceData;
        if (currentChainResource == null)
        {
            StartNewChain();

        }
        else
        {
            CompareChainResources();
        }
    }

    public void FloorBounced()
    {
        bouncedResource = null;
        bouncedResourceBlock = null;
        EndStreak();
        UIManager.Instance.currentMatchPanel.EndCurrentCombo();
        if (!lastBounceChained && currentChainResource != null)
        {
            FailCurrentChain();
        }
        audioSource.PlayOneShot(floorBlockSound, 0.7f);
    }

    private void StartNewChain()
    {
        //Debug.Log("New Chain Started");

        currentChainBlocks.Add(bouncedResourceBlock);
        currentChainResource = bouncedResource;
        lastBounceChained = false;
        UIManager.Instance.currentMatchPanel.StartNewCombo(currentChainResource,currentChainBlocks.Count);
        UIManager.Instance.remainingBlockIndicator.ToggleIndicator(true);
        UIManager.Instance.remainingBlockIndicator.UpdateIndicator(bouncedResource, bouncedResource.hardness-1);
    }

    private void CompareChainResources()
    {
        if (bouncedResource != currentChainResource)
        {
            FailCurrentChain();
            StartNewChain();
        }
        else
        {
            TryToAddToChain();
        }
    }

    private void EndCurrentChain()
    {
        //Debug.Log("Ended Current Chain");
        currentChainResource = null;
        currentChainBlocks.Clear();
        UIManager.Instance.remainingBlockIndicator.ToggleIndicator(false);
    }
    private void FailCurrentChain()
    {
        PlayerManager.Instance.playerEmojis.FailedEmoji();
        //Debug.Log("Failed Current Chain");
        HelmetManager.Instance.currentHelmet.TakeDamage(1);
        //UIManager.Instance.damageTakenIndicator.AnimateDamage();
        FlashFailedBlocks();
        ClearAllHitBlocks();
        lastBounceChained = false;
        EndStreak();

        EndCurrentChain();

        UIManager.Instance.currentMatchPanel.EndCurrentCombo();
        UIManager.Instance.remainingBlockIndicator.ToggleIndicator(false);
        //UIManager.Instance.currentMatchPanel.ChangeCurrentCombo();

        audioSource.PlayOneShot(chainFailSound, 0.5f);
    }

    public void FlashFailedBlocks()
    {
        foreach (var block in currentChainBlocks)
        {
            block.AnimateFailed();
        }
    }

    private void TryToAddToChain()
    {
        //Debug.Log("Trying to add to chain");
        if (!currentChainBlocks.Contains(bouncedResourceBlock))
        {
            AddResBlockToChain();
        }
    }

    private void AddResBlockToChain()
    {

        currentChainBlocks.Add(bouncedResourceBlock);
        lastBounceChained = false;
        bouncedResourceBlock.ShowHitIndicator(true);

        //UI VISUALS
        UIManager.Instance.remainingBlockIndicator.UpdateIndicatorCount(bouncedResource.hardness - currentChainBlocks.Count);
        UIManager.Instance.currentMatchPanel.IncreaseCurrentCombo(currentChainBlocks.Count);

        if (isChainCompleted())
        {
            CompleteCurrentChain();
        }

    }

    private bool isChainCompleted()
    {
        if (currentChainBlocks.Count == currentChainResource.hardness)
        {
            lastBounceChained = true;
            return true;
        }
        lastBounceChained = false;
        return false;
    }

    private void CompleteCurrentChain()
    {
        //Debug.Log("Chain COMPLETED!");
        RewardPlayer();
        IncreaseStreak();
        PlayerManager.Instance.playerEmojis.CompletedEmoji();
        foreach (ResourceBlock _block in currentChainBlocks)
        {
            _block.Activate();
        }
        ClearAllHitBlocks();
        EndCurrentChain();

        UIManager.Instance.currentMatchPanel.CompleteCurrentCombo();
        UIManager.Instance.remainingBlockIndicator.ToggleIndicator(false);

    }
    public void ClearAllHitBlocks()
    {
        foreach (ResourceBlock _block in currentChainBlocks)
        {
            _block.ShowHitIndicator(false);
        }
        currentChainBlocks.Clear();
    }
    private void RewardPlayer()
    {
        //Debug.Log("Rewarding Player!");
        RewardSublevelBlocks();
        RewardResources();
        RewardHelmetXP();
    }

    private void IncreaseStreak()
    {
        //Debug.Log("Streak Increased");
        currentStreak++;
    }

    private void EndStreak()
    {
        //Debug.Log("Streak Restarted");
        currentStreak =1;
    }

    private void RewardSublevelBlocks()
    {
        int _totalBlocks = currentChainBlocks.Count;
        //Debug.Log("REW Blocks " + _totalBlocks);
        LevelManager.Instance.IncreaseMinedBlocks(_totalBlocks);
    }

    private void RewardResources()
    {
        int _totalRes = currentChainBlocks.Count * currentStreak;
        //Debug.Log("REW Resources " + _totalRes);
        ResourceManager.Instance.AddResource(currentChainResource, _totalRes);
    }

    private void RewardHelmetXP()
    {
        int _totalXP = currentChainBlocks.Count * currentStreak;
        //Debug.Log("REW Helmet XP " + _totalXP);
        HelmetManager.Instance.currentHelmet.helmetXP.AddXP(_totalXP);
    }


}
