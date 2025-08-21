using System;

public interface IRequirement
{
    void Initialize(int gateId);
    void UpdateProgress(object _eventData);
    void Reset()
    {
        current = 0;
    }
    int targetId { get; set; }
    bool isCompleted { get; }
    float progress { get; }
    int current { get; set; }
    int goal { get; set; }

    event Action<int, int> OnProgressChanged;
}
