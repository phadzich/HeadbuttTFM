using UnityEngine;
using static UnityEditor.UIElements.ToolbarMenu;
using static UnityEngine.InputSystem.Android.LowLevel.AndroidGameControllerState;

public class BlockNS : MonoBehaviour, IElemental
{
    public string blockName;

    public int sublevelId;
    public Vector2 sublevelPosition;
    public bool isWalkable = false;
    public MapContext mapContext;

    private IBlockEffect[] effects;
    [SerializeField] private IBlockBehaviour[] behaviours;

    // Vecinos
    public BlockNS up;
    public BlockNS down;
    public BlockNS left;
    public BlockNS right;

    [SerializeField] private ElementType elementType;

    public ElementType Element
    {
        get => elementType;
        set => elementType = value;
    }

    void Awake()
    {
        effects = GetComponents<IBlockEffect>();
        behaviours = GetComponents<IBlockBehaviour>();
    }

    private void OnEnable()
    {
        LevelManager.Instance.onSublevelEntered += StartBehaviours;
    }

    private void OnDisable()
    {
        LevelManager.Instance.onSublevelEntered -= StartBehaviours;
    }

    public void SetupBlock(string _name,string _variant, MapContext _context)
    {
        blockName = _name;

        mapContext = _context;
        sublevelId = _context.depth;
        sublevelPosition = new Vector2(_context.x, _context.y);

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

        foreach (var behaviour in behaviours)
            behaviour.OnBounced(_helmetInstance);

        HandleInteraction(_helmetInstance, InteractionSource.PlayerBounce);
    }

    private void HandleInteraction(HelmetInstance _helmetInstance, InteractionSource _source)
    {
        var handler = GetComponent<ElementInteractionComponent>();
        if (handler != null)
        {
            handler.HandleInteraction(_helmetInstance.Element, InteractionSource.PlayerAttack);
        }
    }

    public void OnHeadbutt(HelmetInstance _helmetInstance)
    {
        
        foreach (var effect in effects)
            effect.OnHeadbutt(_helmetInstance);

        foreach (var behaviour in behaviours)
        {
            
            behaviour.OnHeadbutt(_helmetInstance);
        }

        HandleInteraction(_helmetInstance, InteractionSource.PlayerHeadbutt);
    }

    public void StartBehaviours(Sublevel _sublevel)
    {
        if (_sublevel == mapContext.sublevel)
        {
            foreach (var behaviour in behaviours)
                        behaviour.StartBehaviour();
        }
        
    }

    public void StopBehaviours()
    {
        foreach (var behaviour in behaviours)
            behaviour.StopBehaviour();
    }


}
