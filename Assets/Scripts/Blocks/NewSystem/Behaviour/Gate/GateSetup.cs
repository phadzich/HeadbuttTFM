using UnityEngine;

[RequireComponent(typeof(GateBehaviour))]
[RequireComponent(typeof(BlockNS))]
public class GateSetup : MonoBehaviour, IBlockSetup
{
    public void SetupVariant(string _variant, MapContext _context)
    {
        int _gateIndex = int.Parse(_variant);
        var _gateRequirement = _context.miningConfig.gateRequirements[_gateIndex];

        var gateBehav = GetComponent<GateBehaviour>();
        gateBehav.SetupBlock(_context.sublevel, _gateIndex, _gateRequirement);

        _context.sublevel.gateBlocks.Add(gateBehav);
    }
}
