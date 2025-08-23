using System.Collections.Generic;
using UnityEngine;

public class DamageEEffect : MonoBehaviour, IEnemyEffect, IElementReactive
{
    public int damage;
    public float damageCooldown;
    public float lastDamageTime;

    public EnemySFX sfx;

    [SerializeField] public List<InteractionSource> AllowedSources = new List<InteractionSource>();

    private void Awake()
    {
        sfx = GetComponent<EnemySFX>();
    }

    // Cuando NOSOTROS recibimos un impacto
    public void OnHit()
    {}

    private void DoDamage()
    {
        if (sfx!= null) sfx.PlayAttack();
        Debug.Log("EnemyDMG "+ damage);
        HelmetManager.Instance.currentHelmet.TakeDamage(damage, true);
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
