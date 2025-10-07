using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(BlockNS))]
public class WallBehaviour : MonoBehaviour, IBlockBehaviour
{
    public void OnBounced(HelmetInstance _helmetInstance)
    {
    }

    public void OnHeadbutt(HelmetInstance _helmetInstance)
    {
    }

    void Start()
    {
        GetComponent<BlockNS>().isWalkable = false;
        Quaternion randomYRotation = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);
        transform.rotation = randomYRotation;
    }

    public void StartBehaviour()
    {
    }

    public void StopBehaviour()
    {
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Projectile"))
        {
            TryDestroyProjectile(other.gameObject);
        }
    }

    private void TryDestroyProjectile(GameObject _projectile)
    {
        if (_projectile.GetComponent<DestroyOnCollisionEEfect>() != null)
        {
            {
                _projectile.GetComponent<DestroyOnCollisionEEfect>().OnTrigger();
            }
        }
    }
}
