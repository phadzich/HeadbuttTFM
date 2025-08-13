using UnityEngine;

[RequireComponent(typeof(BlockNS))]
[RequireComponent(typeof(CollectibleSetup))]
public class CollectibleBehaviour : MonoBehaviour, IBlockEffect
{
    [SerializeField] private GameObject collectableObject;

    private ICollectibleEffect[] collectables;
    private bool isCollected = false;

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
        if (isCollected) return;

        foreach (var col in collectables)
        {
            col.Activate();
        }

        if (collectableObject != null)
            collectableObject.SetActive(false);
        isCollected = true;
    }

    public void OnBounced(HelmetInstance _helmetInstance)
    {
        Activate();
        MatchManager.Instance.FloorBounced();
    }

    public void OnHeadbutt(HelmetInstance _helmetInstance)
    {
        Activate(); 
        MatchManager.Instance.FloorBounced();
    }
}
