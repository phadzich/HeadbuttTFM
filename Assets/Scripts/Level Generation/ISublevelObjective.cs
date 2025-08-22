using System;
using UnityEngine;

public interface ISublevelObjective
{
    Sprite GetIcon();
    void Initialize();
    void UpdateProgress(object _eventData);
    bool isCompleted { get; }
    float progress { get; }
    int current { get; set; }
    int goal { get; set; }

    event Action<int, int> OnProgressChanged;
}
