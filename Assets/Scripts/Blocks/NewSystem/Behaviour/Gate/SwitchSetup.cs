using UnityEngine;

[RequireComponent(typeof(BlockNS))]
public class SwitchSetup : MonoBehaviour, IBlockSetup
{
    public void SetupVariant(string _variant, MapContext _context)
    {
        int _switchID = int.Parse(_variant);
        var _switchBehav = GetComponent<SwitchBehaviour>();
        _switchBehav.SetupBlock(_context, _switchID);
    }
}
