using UnityEngine;

public class HeadDamage : MonoBehaviour
{

    public HeadDmg_Fire headDmg;

    private void OnTriggerEnter(Collider other)
    {
        headDmg.DoDamage(other);
    }
}
