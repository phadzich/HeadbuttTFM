using UnityEngine;

public interface ISublevelObjective
{
    void Initialize();
    void UpdateProgress(object _eventData);
    bool isCompleted { get; }
    float progress { get; }
    int current { get; set; }
    int goal { get; set; }
}
