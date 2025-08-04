using UnityEngine;

[RequireComponent(typeof(NPCBehaviour))]
public class NPCSetup : MonoBehaviour, IBlockSetup
{
    public void SetupVariant(string _variant, MapContext _context)
    {
        GetComponent<NPCBehaviour>().SetupBlock(_context);
    }
}
