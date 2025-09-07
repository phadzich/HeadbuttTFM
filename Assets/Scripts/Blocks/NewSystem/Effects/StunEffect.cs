using System.Collections.Generic;
using UnityEngine;

public class StunEffect : MonoBehaviour, IElementReactive, IBlockEffect
{
    public float stunTime;

    private EnemySFX sfx => GetComponent<EnemySFX>();

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

    }

    public void OnBounced(HelmetInstance _helmetInstance)
    {
        if (sfx != null) sfx.PlayAttack();
        PlayerManager.Instance.playerEffects.GetStunned(stunTime);
    }

    public void OnHeadbutt(HelmetInstance _helmetInstance)
    {
        if (sfx != null) sfx.PlayAttack();
        PlayerManager.Instance.playerEffects.GetStunned(stunTime);
    }
}
