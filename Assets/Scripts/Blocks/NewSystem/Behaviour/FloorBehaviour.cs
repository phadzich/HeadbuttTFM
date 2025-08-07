using UnityEngine;

[RequireComponent(typeof(BlockNS))]
public class FloorBehaviour : MonoBehaviour, IBlockEffect
{
    public Transform blockMeshParent;

    [Header("SFX")]
    public AudioClip headbuttSound;
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        GetComponent<BlockNS>().isWalkable = true;
        SetRandomRotation();
    }

    public void OnBounced(HelmetInstance _helmetInstance)
    {
        MatchManager.Instance.FloorBounced();
    }

    public void OnHeadbutt(HelmetInstance _helmetInstance)
    {
        MatchManager.Instance.FloorBounced();
        audioSource.PlayOneShot(headbuttSound, 0.7f);
    }

    public void Activate()
    {
         
    }

    private void SetRandomRotation()
    {
        int _randomRotation = Random.Range(0, 4);
        transform.Rotate(0, (float)_randomRotation * 90, 0);
    }
}
