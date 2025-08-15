using System;
using UnityEngine;

[System.Serializable]
public class Enemy: MonoBehaviour, IElemental
{
    private IEnemyEffect[] effects;
    [SerializeField] private IEnemyBehaviour[] behaviours;

    [SerializeField] private ElementType elementType;

    public ElementType Element
    {
        get => elementType;
        set => elementType = value;
    }

    private void Awake()
    {
        effects = GetComponents<IEnemyEffect>();
        behaviours = GetComponents<IEnemyBehaviour>();
    }

    public void Start()
    {
        StartBehaviours();
    }

    public void StartBehaviours()
    {
        foreach (var behaviour in behaviours)
            behaviour.StartBehaviour();

    }

    public void StopBehaviours()
    {
        foreach (var behaviour in behaviours)
            behaviour.StopBehaviour();
    }

    private void HandleInteraction(InteractionSource _source)
    {
        var handler = GetComponent<ElementInteractionComponent>();
        if (handler != null)
        {
            handler.HandleInteraction(HelmetManager.Instance.currentHelmet.Element, _source);
        }
    }

    public void OnHit(){
        HandleInteraction(InteractionSource.PlayerAttack);

        foreach (var behaviour in behaviours)
        {
            behaviour.OnHit();
        }
    }

    public void OnTrigger()
    {
        HandleInteraction(InteractionSource.EnemyCollision);

        foreach (var effect in effects)
        {
            effect.OnTrigger();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            OnTrigger();
        }
    }


}
