using UnityEngine;

public class DamageAreaDebug : MonoBehaviour
{
    public Vector3 boxSize = new Vector3(1, 1, 1);

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, boxSize * 2);
    }
}
