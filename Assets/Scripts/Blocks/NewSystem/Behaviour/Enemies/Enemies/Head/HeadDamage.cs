using UnityEngine;

public class HeadDamage : MonoBehaviour
{

    //public DamageBlock headDmg;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Contacto!");
            //headDmg.DoDamage();
        }        
    }
}
