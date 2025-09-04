using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MatchManager : MonoBehaviour
{
    public static MatchManager Instance;

    [Header("COMBO ACTUAL")]
    [SerializeField]
    private List<ResourceEffect> currentChainBlocks;
    public ResourceData currentChainResource;
    public ResourceData bouncedResource;
    public ResourceEffect bouncedResourceBlock;

    public int currentStreak;
    public int maxStreak;
    public bool lastBounceChained;
    public float HBRewardRatio;
    public float streakRewardRatio;

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
        //RestartMatches();
    }

    public void RestartMatches()
    {
        EndStreak();
        EndCurrentChain();
    }
    public void ResourceBounced(ResourceEffect _resBlock)
    {
        bouncedResourceBlock = _resBlock;
        bouncedResource = bouncedResourceBlock.resourceData;

        if (currentChainResource == null)
        {
            //StartNewChain();
            currentChainResource = bouncedResource;
        }
        CompareChainResources();
        
    }

    public void FloorBounced()
    {
        bouncedResource = null;
        bouncedResourceBlock = null;
        EndStreak();
        if (!lastBounceChained && currentChainResource != null)
        {
            FailCurrentChain();
        }
        SoundManager.PlaySound(SoundType.FLOORBOUNCE, 1f);
    }

    public void EnemyBounced()
    {
        FloorBounced();
    }

    private void StartNewChain()
    {
        //Debug.Log("New Chain Started");

        currentChainBlocks.Add(bouncedResourceBlock);
        bouncedResourceBlock.isSelected = true;
        currentChainResource = bouncedResource;
        lastBounceChained = false;
        UIManager.Instance.remainingBlockIndicator.ToggleIndicator(true);
        UIManager.Instance.remainingBlockIndicator.UpdateIndicator(bouncedResource, currentChainBlocks.Count, bouncedResource.hardness);
    }

    private void CompareChainResources()
    {
        if (bouncedResource != currentChainResource)
        {
            FailCurrentChain();
            //Debug.Log("Starting new chain");
            StartNewChain();
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

        UIManager.Instance.remainingBlockIndicator.ToggleIndicator(false);
        //UIManager.Instance.currentMatchPanel.ChangeCurrentCombo();

        SoundManager.PlaySound(SoundType.COMBOFAIL, 0.7f);
    }

    public void FlashFailedBlocks()
    {
        foreach (var block in currentChainBlocks)
        {
            block.AnimateFailed();
        }
    }

    public void TryToAddToChain()
    {   
        if(currentChainBlocks.Count == 0)
        {
            StartNewChain();
        }
        else
        {
            if (!currentChainBlocks.Contains(bouncedResourceBlock))
            {
                AddResBlockToChain();
            }
        }

    }

    private void AddResBlockToChain()
    {

        currentChainBlocks.Add(bouncedResourceBlock);
        bouncedResourceBlock.isSelected = true;
        lastBounceChained = false;
        //bouncedResourceBlock.ShowHitIndicator(true);

        //UI VISUALS
        UIManager.Instance.remainingBlockIndicator.UpdateIndicator(bouncedResource, currentChainBlocks.Count, bouncedResource.hardness);


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
        foreach (ResourceEffect _block in currentChainBlocks)
        {
            _block.Activate();
        }
        ClearAllHitBlocks();
        EndCurrentChain();

        UIManager.Instance.remainingBlockIndicator.ToggleIndicator(false);

    }



    public void ClearAllHitBlocks()
    {
        foreach (ResourceEffect _block in currentChainBlocks)
        {
            _block.ToggleHitIndicator(false);
            _block.isSelected = false;
            _block.helmetPowerMultiplier = 1;
        }
        currentChainBlocks.Clear();
    }

    private void RewardPlayer()
    {
        //Debug.Log("Rewarding Player!");
        RewardResources();
        RewardHBPoints();
    }

    private void IncreaseStreak()
    {
        //Debug.Log("Streak Increased");
        if (currentStreak < maxStreak)
        {
            currentStreak++;
        }
        DispatchStreakEvent();

        UIManager.Instance.hbPointsHUD.UpdateStreak(currentStreak);
    }

    private void EndStreak()
    {
        //Debug.Log("Streak Restarted");

        currentStreak =1;
        UIManager.Instance.hbPointsHUD.UpdateStreak(currentStreak);

    }

    private void DispatchStreakEvent()
    {
        var _streakEvent = new MatchStreakEvent { currentStreak = currentStreak };
        LevelManager.Instance.currentSublevel.DispatchObjectiveEvent(_streakEvent);
    }
    private void RewardResources()
    {
        int _totalRes = 0;

        foreach (ResourceEffect _block in currentChainBlocks)
        {

            _totalRes += _block.helmetPowerMultiplier;

            var _resourceEvent = new CollectResourceEvent { resData = _block.resourceData, amount = _block.helmetPowerMultiplier };
            LevelManager.Instance.currentSublevel.DispatchObjectiveEvent(_resourceEvent);
        }

        //Debug.Log("REW Resources " + _totalRes);
        ResourceManager.Instance.AddResource(currentChainResource, _totalRes);
    }

    private void RewardHBPoints()
    {
        float _totalPoints = currentChainBlocks.Count * currentStreak*streakRewardRatio * HBRewardRatio;
        //Debug.Log("REW Resources " + _totalRes);
        PlayerManager.Instance.playerHeadbutt.AddHBPoints((float)_totalPoints);
    }


}
