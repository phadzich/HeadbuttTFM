using System.Collections.Generic;
using UnityEngine;

public class StunEEffect : MonoBehaviour, IEnemyEffect, IElementReactive
{
    public float stunTime;

    [SerializeField] public List<InteractionSource> AllowedSources = new List<InteractionSource>();

    public bool IsAllowedForSource(InteractionSource source)
    {
        return AllowedSources.Count == 0 || AllowedSources.Contains(source);
    }

    public void OnElementInteraction(ElementType sourceElement, ElementType targetElement)
    {
    }

    public void OnHit(){}

    public void OnTrigger()
    {
        PlayerManager.Instance.playerEffects.GetStunned(stunTime);
    }

}
