using System;
using UnityEngine;

[System.Serializable]
public abstract class SublevelObjectiveBase : ISublevelObjective
{
    public event Action<int, int> OnProgressChanged;
    public abstract Sprite GetIcon();
    public abstract void Initialize();
    public abstract void UpdateProgress(object eventData);
    public abstract bool isCompleted { get; }
    public abstract float progress { get; }
    private int _current;
    public int current
    {
        get => _current;
        set
        {
            _current = value;
            OnProgressChanged?.Invoke(_current, goal);
        }
    }
    public int goal { get; set; }
}
