using UnityEngine;

public class HealthBehaviour : MonoBehaviour, IBlockBehaviour
{
    public int maxHealth;
    public int currentHealth;
    public bool isDead => currentHealth <= 0;

    void RecieveDamage(int _amount)
    {
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
}
