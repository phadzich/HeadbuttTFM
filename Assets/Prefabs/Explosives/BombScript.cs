using UnityEngine;
using System.Collections;

public class BombScript : MonoBehaviour
{
    public GameObject explosionVFX;                 // VFX prefab para la explosión de manera publica
    
    public LayerMask layerObjetosADestruir;         // Aqui lo asigno desde el inspector de manera publica
    
    public float radioDeExplosion = 3f;             // Radio efectivo de la bomba

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(DetonarConDelay());
        }
    }

    IEnumerator DetonarConDelay()
    {
        yield return new WaitForSeconds(2f);
        Detonar();
    }

    void Detonar()
    {
        // Efecto visual
        if (explosionVFX != null)
            Instantiate(explosionVFX, transform.position, Quaternion.identity);

        // Buscar objetos en el radio que estén en la capa objetivo
        Collider[] objetosEnRadio = Physics.OverlapSphere(transform.position, radioDeExplosion, layerObjetosADestruir);

        foreach (Collider objeto in objetosEnRadio)
        {
            if (objeto != null)
                Destroy(objeto.gameObject);
        }

        // Destruye la bomba
        Destroy(gameObject);
    }

    // Aqui quiero ver el radio de destruccion en el editor de color rojo
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radioDeExplosion);
    }
}