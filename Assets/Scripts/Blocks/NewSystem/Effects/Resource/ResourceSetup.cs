using UnityEngine;
using System.Linq;

[RequireComponent(typeof(BlockNS))]
public class ResourceSetup : MonoBehaviour, IBlockSetup
{
    public void SetupVariant(string _variant, MapContext _context)
    {
        ResourceData _resourceData = GetResourceFromID(int.Parse(_variant));
        ResourceEffect resBlock = GetComponent<ResourceEffect>();
        resBlock.SetupBlock(_context, _resourceData);
    }

    ResourceData GetResourceFromID(int resID) =>
    ResourceManager.Instance.allAvailableResources.FirstOrDefault(r => r.id == resID);
}