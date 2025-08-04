using UnityEngine;

[RequireComponent(typeof(BlockNS))]
[RequireComponent(typeof(CollectibleSetup))]
public class CollectibleBehaviour : MonoBehaviour, IBlockEffect
{
    [SerializeField] private GameObject collectableObject;

    private ICollectibleEffect[] collectables;

    private void Awake()
    {
        collectables = GetComponentsInChildren<ICollectibleEffect>();
    }

    public void SetupBlock(string _variant, MapContext _context)
    {
        foreach (var col in collectables)
        {
            col.SetupBlock(_variant, _context);
        }
    }

    public void Activate()
    {
        foreach (var col in collectables)
        {
            col.Activate();
        }

        if (collectableObject != null)
            collectableObject.SetActive(false);
    }

    // Llamado por el hijo
    public void OnCollected()
    {
        Activate();
    }

    public void OnBounced(HelmetInstance _helmetInstance) => MatchManager.Instance.FloorBounced();

    public void OnHeadbutt(HelmetInstance _helmetInstance) => MatchManager.Instance.FloorBounced();
}
