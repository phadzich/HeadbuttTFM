using UnityEngine;

[RequireComponent(typeof(BlockNS))]
public class GateSetup : MonoBehaviour, IBlockSetup
{
    public void SetupVariant(string _variant, MapContext _context)
    {
        int _gateIndex = int.Parse(_variant);
        var _gateRequirement = _context.miningConfig.gateRequirements[_gateIndex];
        //Debug.Log($"[SetupVariant] Gate {name} assigned gateIndex={_gateIndex}, requirement={_gateRequirement}");
        var gateBehav = GetComponent<GateBehaviour>();
        gateBehav.SetupBlock(_context,_gateRequirement, _gateIndex);
    }
}
