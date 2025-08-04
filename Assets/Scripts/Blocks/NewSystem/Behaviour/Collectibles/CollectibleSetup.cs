using UnityEngine;

[RequireComponent(typeof(BlockNS))]
public class CollectibleSetup : MonoBehaviour, IBlockSetup
{
    public void SetupVariant(string _variant, MapContext _context)
    {
        GetComponent<CollectibleBehaviour>().SetupBlock(_variant, _context);
    }
}
