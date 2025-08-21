using System;
using UnityEngine;

public interface IRequirement
{
    void Initialize();
    void UpdateProgress(object _eventData);
    void Reset()
    {
        current = 0;
    }

    Sprite GetIcon();
    int targetId { get; }
    bool isCompleted { get; }
    float progress { get; }
    int current { get; set; }
    int goal { get; set; }

    event Action<int, int> OnProgressChanged;
}
