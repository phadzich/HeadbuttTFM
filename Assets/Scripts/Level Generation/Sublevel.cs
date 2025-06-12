using UnityEngine;

public class Sublevel : MonoBehaviour { 

    public string id;
    public int depth;
    public bool isActive;
    public SublevelConfig config;

    public int currentBlocksMined;
    public int maxResourceBlocks;
    public bool isTotallyMined => currentBlocksMined == maxResourceBlocks;
    public int blocksToComplete;


    public void SetupSublevel (string _id, int _depth, bool _isActive, SublevelConfig _config)
    {
        this.id = _id;
        this.depth = _depth;
        this.isActive = _isActive;
        this.config = _config;
    }

    public void SetMiningObjectives(int _objective)
    {
        blocksToComplete = _objective;
        currentBlocksMined = 0;
    }
}
