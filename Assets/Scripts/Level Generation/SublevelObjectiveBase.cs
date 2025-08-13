[System.Serializable]
public abstract class SublevelObjectiveBase : ISublevelObjective
{
    public abstract void Initialize();
    public abstract void UpdateProgress(object eventData);
    public abstract bool isCompleted { get; }
    public abstract float progress { get; }
    public int current { get; set; }
    public int goal { get; set; }
}