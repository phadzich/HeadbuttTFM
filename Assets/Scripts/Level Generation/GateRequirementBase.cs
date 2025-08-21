[System.Serializable]
public abstract class GateRequirementBase : IGateRequirement
{
    public abstract void Initialize();
    public abstract void UpdateProgress(object eventData);
    public abstract bool isCompleted { get; }
    public abstract float progress { get; }
    public int current { get; set; }
    public int goal { get; set; }
}