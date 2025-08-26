using UnityEngine;

[RequireComponent(typeof(ChestBehaviour))]
[RequireComponent(typeof(BlockNS))]
public class ChestSetup : MonoBehaviour, IBlockSetup
{
    public void SetupVariant(string _variant, MapContext _context)
    {
        int _chestIndex = int.Parse(_variant);
        var _chestRequirements = _context.miningConfig.chestsRequirements[_chestIndex];
        //Debug.Log($"[SetupVariant] Chest {name} assigned gateIndex={_chestIndex}, requirement={_chestRequirements}");
        var _chestBehav = GetComponent<ChestBehaviour>();
        _chestBehav.SetupBlock(_context,_chestRequirements, _chestIndex);
    }
}
