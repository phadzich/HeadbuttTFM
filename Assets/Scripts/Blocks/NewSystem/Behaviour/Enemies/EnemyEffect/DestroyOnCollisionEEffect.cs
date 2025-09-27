using System.Collections.Generic;
using UnityEngine;

public class DestroyOnCollisionEEfect : MonoBehaviour, IEnemyEffect
{
    private EnemySFX sfx => GetComponent<EnemySFX>();

    public void OnHit()
    {}

    private void SelfDestruct()
    {
        if (sfx != null) sfx.PlayDeath();
        Destroy(this.gameObject);
    }

    // Cuando OTROS reciben un impacto
    public void OnTrigger()
    {
        SelfDestruct();
    }

}
