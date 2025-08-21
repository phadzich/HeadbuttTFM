using System;

[System.Serializable]
public abstract class RequirementBase : IRequirement
{
    public event Action<int, int> OnProgressChanged;

    public abstract void Initialize(int _id);
    public abstract void UpdateProgress(object eventData);
    public abstract bool isCompleted { get; }
    public abstract float progress { get; }
    public int current { get; set; }
    public int goal { get; set; }
    public int targetId { get; set; }
}