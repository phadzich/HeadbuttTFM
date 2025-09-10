using System;
using UnityEngine;

[System.Serializable]
public abstract class RequirementBase : IRequirement
{
    [SerializeField] private int customID;
    public int targetId => customID;

    public event Action<int, int> OnProgressChanged;
    public virtual void Initialize()
    {
        current = 0;

    }
    public abstract void UpdateProgress(object eventData);

    public abstract Sprite GetIcon();

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