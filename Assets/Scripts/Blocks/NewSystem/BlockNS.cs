using UnityEngine;
using static UnityEditor.UIElements.ToolbarMenu;
using static UnityEngine.InputSystem.Android.LowLevel.AndroidGameControllerState;

public class BlockNS : MonoBehaviour
{
    public int sublevelId;
    public Vector2 sublevelPosition;
    public bool isWalkable = false;

    private IBlockEffect[] effects;

    // Vecinos
    public BlockNS up;
    public BlockNS down;
    public BlockNS left;
    public BlockNS right;

    void Awake() => effects = GetComponents<IBlockEffect>();

    public void SetupBlock(string _variant, MapContext _context)
    {
        sublevelId = _context.depth;
        sublevelPosition = new Vector2(_context.x, _context.y);
        isWalkable = true;

        SetUpVariant(_variant, _context);

    }

    private void SetUpVariant(string _variant, MapContext _context)
    {
        var setups = GetComponents<IBlockSetup>();
        foreach (var s in setups)
            s.SetupVariant(_variant, _context);
    }

    public void OnBounced(HelmetInstance _helmetInstance)
    {
        foreach (var effect in effects)
            effect.OnBounced(_helmetInstance);
    }

    public void OnHeadbutt(HelmetInstance _helmetInstance)
    {
        foreach (var effect in effects)
            effect.OnHeadbutt(_helmetInstance);
    }

    public void Activate()
    {
        foreach (var effect in effects)
            effect.Activate();
    }
}
