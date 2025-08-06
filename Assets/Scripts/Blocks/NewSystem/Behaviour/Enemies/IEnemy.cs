public interface IEnemy
{
    void DoDamage();
    void ReceiveDamage(int amount);
    void Die();
    void Attack();
}