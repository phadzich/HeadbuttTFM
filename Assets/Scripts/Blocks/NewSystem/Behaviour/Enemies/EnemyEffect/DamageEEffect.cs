using System.Collections.Generic;
using UnityEngine;

public class DamageEEffect : MonoBehaviour, IEnemyEffect, IElementReactive
{
    public int damage;
    public float damageCooldown;
    public float lastDamageTime;

    private EnemySFX sfx => GetComponent<EnemySFX>();

    [SerializeField] public List<InteractionSource> AllowedSources = new List<InteractionSource>();

    // Cuando NOSOTROS recibimos un impacto
    public void OnHit()
    {}

    private void DoDamage()
    {
        //Debug.Log(sfx==null);
        if (sfx!= null) sfx.PlayAttack();
        //Debug.Log("EnemyDMG "+ damage);
        PlayerManager.Instance.playerEffects.TakeDamage(damage);
    }

    // Cuando OTROS reciben un impacto
    public void OnTrigger()
    {
        Debug.Log("Contacto con enemigo!");
        float time = Time.time;
        if (Time.time - lastDamageTime >= damageCooldown) DoDamage();
        lastDamageTime = Time.time;
    }

    public bool IsAllowedForSource(InteractionSource source)
    {
        return AllowedSources.Count == 0 || AllowedSources.Contains(source);
    }

    public void OnElementInteraction(ElementType sourceElement, ElementType targetElement)
    {
        
    }
}
