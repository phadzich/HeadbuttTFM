using System.Collections.Generic;
using UnityEngine;

public class DestroyOnCollisionEEfect : MonoBehaviour, IEnemyEffect
{

    public void OnHit()
    {}

    private void SelfDestruct()
    {
        Destroy(this.gameObject);
    }

    // Cuando OTROS reciben un impacto
    public void OnTrigger()
    {
        SelfDestruct();
    }

}
