public interface IGateRequirement
{
    void Initialize();
    void UpdateProgress(object _eventData);
    bool isCompleted { get; }
    float progress { get; }
    int current { get; set; }
    int goal { get; set; }
}
