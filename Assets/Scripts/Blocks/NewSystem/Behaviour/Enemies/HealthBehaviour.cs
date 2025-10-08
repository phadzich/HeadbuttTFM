using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HealthBehaviour : MonoBehaviour, IBlockBehaviour, IElementReactive
{
    public int maxHealth;
    public int currentHealth;
    public bool isDead => currentHealth <= 0;

    [SerializeField] public List<InteractionSource> AllowedSources = new List<InteractionSource>();

    void RecieveDamage(int _amount)
    {
        SoundManager.PlayeJomaSound(JomaType.ATTACK);
        Debug.Log($"DAMAGE: {_amount}");
        currentHealth -= _amount;
        if (isDead) Die();
    }

    void Heal(int _amount)
    {
        currentHealth = currentHealth + _amount >= maxHealth ? maxHealth : currentHealth + _amount;
    }

    void Die()
    {
        Debug.Log("DEAD");
        GetComponent<BlockNS>().StopBehaviours();
    }

    public void RestoreHealth()
    {
        currentHealth = maxHealth;
    }

    public void StartBehaviour()
    {
        RestoreHealth();
    }

    public void OnBounced(HelmetInstance _helmetInstance)
    {
    }

    public void OnHeadbutt(HelmetInstance _helmetInstance)
    {
        Debug.Log("Headbutt");
        RecieveDamage(((int)_helmetInstance.baseHelmet.miningPower) + 1);
    }

    public void StopBehaviour(){}

    public void OnElementInteraction(ElementType sourceElement, ElementType targetElement)
    {
        if (targetElement == ElementType.Fire)
        {
            switch (sourceElement)
            {
                case ElementType.Water:
                    Debug.Log("InstantDead");
                    break;
            }
        }

        if (targetElement == ElementType.Grass)
        {
            switch (sourceElement)
            {
                case ElementType.Fire:
                    Debug.Log("InstantDead");
                    break;
                case ElementType.Water:
                    Debug.Log("BOOST");
                    break;
            }
        }

        if (targetElement == ElementType.Electric)
        {
            switch (sourceElement)
            {
                case ElementType.Fire:
                    Debug.Log("Resist");
                    break;
                case ElementType.Water:
                    Debug.Log("Resist");
                    break;
            }
        }

    }

    public bool IsAllowedForSource(InteractionSource source)
    {
        return AllowedSources.Count == 0 || AllowedSources.Contains(source);
    }
}
