using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Windows;

public class SublevelMapGenerator : MonoBehaviour
{
    public ColorPalette colorMappings;
    public Texture2D testMap;
    Transform sublevelContainer;
    [SerializeField]
    private Texture2D mapTexture;
    [SerializeField]
    int mapWidth;
    [SerializeField]
    int mapHeight;

    int currentX;
    int currentY;
    Vector3 nextPosition;
    int currentDepth;

    public Dictionary<Vector2Int, Block> currentBlocks = new();
    [Header("LEVEL")]
    public GameObject clearBlockPrefab;
    public GameObject floorBlockPrefab;
    public GameObject wallBlockPrefab;
    [Header("MINING")]
    public GameObject resourceBlockPrefab;
    public GameObject doorBlockPrefab;
    [Header("DAMAGE")]
    public GameObject slimeBlockPrefab;
    public GameObject lavaBlockPrefab;
    public GameObject ballBlockPrefab;
    [Header("NPC")]
    public GameObject npcDoorPrefab;
    public GameObject npcCraftPrefab;
    public GameObject npcUpgradePrefab;
    public GameObject npcElevatorPrefab;
    private void Start()
    {
        //GenerateSublevel(this.transform, testMap);
    }
    public void GenerateSublevel(Transform _parentTransform, Texture2D _inputMap,int _depth)
    {
        mapWidth = _inputMap.width;
        mapHeight = _inputMap.height;
        mapTexture = _inputMap;
        sublevelContainer = _parentTransform;
        currentDepth = _depth;
        InstanceAllBlocks(mapWidth, mapHeight);

    }

    void InstanceAllBlocks(int _width, int _height)
    {
        currentBlocks.Clear();
        int _spacing = 1;
        float offsetX = (_width - 1) * _spacing * 0.5f;
        float offsetZ = (_height - 1) * _spacing * 0.5f;
        for (int x = 0; x < mapWidth; x++)
        {
            currentX = x;
            for (int y = 0; y < mapHeight; y++)
            {
                currentY = y;
                nextPosition = new Vector3(x * _spacing - offsetX, sublevelContainer.transform.position.y, y * _spacing - offsetZ);
                var _newBlock = BlockFromPixel(x, y);
                Vector2Int pos = new(x, y);
                currentBlocks[pos] = _newBlock.GetComponent<Block>();
            }
        }
        AssignNeighbourBlocks();

    }

    private void AssignNeighbourBlocks()
    {
        //Debug.Log(currentBlocks.Count);
        foreach (var kvp in currentBlocks)
        {
            var pos = kvp.Key;
            var block = kvp.Value;

            currentBlocks.TryGetValue(pos + Vector2Int.up, out block.up);
            currentBlocks.TryGetValue(pos + Vector2Int.down, out block.down);
            currentBlocks.TryGetValue(pos + Vector2Int.left, out block.left);
            currentBlocks.TryGetValue(pos + Vector2Int.right, out block.right);
        }
    }

         private GameObject BlockFromPixel(int _x, int _y)
        {
            Color _pixelColor = mapTexture.GetPixel(_x, _y);


        if (_pixelColor.a == 0)
            {
                return null;
            }

            foreach(ColorToString _color in colorMappings.colors)
            {

            if (_color.color==_pixelColor)
                {
                
                return GetBlockFromString(_color.blockString);
            }
            }


        return null;
        }

    //LOS COLOR MAPPINGS DEBEN SER COLOR A ALGO COMO RES_01_ICE Y HACER UN PARSER QUE AGARRE PREFAB RES, APLIQUE RECURSO 01 Y APLIQUE ESTADO ICE
    //PRIMERO UNA FUNCION QUE SEPARE TYPE, DATA, VARIANT SEGUN LOS SUBGUIONES
    //LUEGO, DEPEDIENDO DEL TYPE TENER FUNCIONES INDEPENDIENTES PARA NEUTRAL, RES, DMG, DOOR, ETC
    //INSTANCIO, CONFIGURO Y EL GRID INSTANCER SOLO LO POSICIONA CORECTAMENTE, NO LO CONFIGURA.

    GameObject GetBlockFromString (string _blockString)
    {

        var _stringParts = _blockString.Split('_');
        string _blockType = _stringParts[0];
        string _blockID = _stringParts[1];
        string _blockVariant = _stringParts[2];

        switch (_blockType)
        {
            case "RES":
                return ConfigResourceBlock(int.Parse(_blockID));
            case "LVL":
                return ConfigLVLBlock(_blockID);
            case "DMG":
                return ConfigDMGBlock(_blockID);
            case "NPC":
                return ConfigNPCBlock(_blockID);
        }
        return null;
    }

    GameObject ConfigResourceBlock(int _resID)
    {
        ResourceData _resourceData = GetResourceFromID(_resID);
        GameObject _bloque = Instantiate(resourceBlockPrefab,nextPosition,Quaternion.identity,sublevelContainer);
        ResourceBlock _resourceBlock = _bloque.GetComponent<ResourceBlock>();
        _resourceBlock.SetupBlock(0, currentX, currentY, _resourceData);
        _bloque.name = $"{_resourceBlock.resourceData.shortName}_c{currentX}r_{currentY}";
        LevelManager.Instance.sublevelsList[currentDepth].maxResourceBlocks++;
        return _bloque;
    }
    GameObject ConfigLVLBlock(string _blockID)
    {
        GameObject _bloque = null;
        switch (_blockID)
        {
            case "CLEAR":
                _bloque = Instantiate(clearBlockPrefab, nextPosition, Quaternion.identity, sublevelContainer);
                break;
            case "WALL":
                _bloque =  Instantiate(wallBlockPrefab, nextPosition, Quaternion.identity, sublevelContainer);
                WallBlock _wallBlock = _bloque.GetComponent<WallBlock>();
                _wallBlock.SetupBlock(currentDepth, currentX, currentY);
                break;
            case "FLOOR":
                
                _bloque = Instantiate(floorBlockPrefab, nextPosition, Quaternion.identity, sublevelContainer);
                FloorBlock _floorBlock = _bloque.GetComponent<FloorBlock>();
                _floorBlock.SetupBlock(currentDepth, currentX, currentY);
                break;
            case "DOOR":
                _bloque = Instantiate(doorBlockPrefab, nextPosition, Quaternion.identity, sublevelContainer);
                DoorBlock _doorBlock = _bloque.GetComponent<DoorBlock>();
                _doorBlock.SetupBlock(currentDepth,currentX,currentY);
                break;
        }
        return _bloque;
    }
    GameObject ConfigNPCBlock(string _blockID)
    {
        GameObject _bloque = null;
        switch (_blockID)
        {
            case "CRAFT":
                _bloque = Instantiate(npcCraftPrefab, nextPosition, Quaternion.identity, sublevelContainer);
                NPCBlock _npcCraftBlock = _bloque.GetComponent<NPCBlock>();
                _npcCraftBlock.SetupBlock(currentDepth, currentX, currentY);
                break;
            case "UPGRADE":
                
                _bloque = Instantiate(npcUpgradePrefab, nextPosition, Quaternion.identity, sublevelContainer);
                NPCBlock _npcUpgradeBlock = _bloque.GetComponent<NPCBlock>();
                _npcUpgradeBlock.SetupBlock(currentDepth, currentX, currentY);
                break;
            case "ELEVATOR":
                _bloque = Instantiate(npcElevatorPrefab, nextPosition, Quaternion.identity, sublevelContainer);
                NPCBlock _npcElevatorBlock = _bloque.GetComponent<NPCBlock>();
                _npcElevatorBlock.SetupBlock(currentDepth, currentX, currentY);
                break;
            case "DOOR":
                _bloque = Instantiate(npcDoorPrefab, nextPosition, Quaternion.identity, sublevelContainer);
                NPCBlock _npcDoorBlock = _bloque.GetComponent<NPCBlock>();
                _npcDoorBlock.SetupBlock(currentDepth, currentX, currentY);
                _npcDoorBlock.isWalkable= true;
                break;
        }
        return _bloque;
    }
    GameObject ConfigDMGBlock(string _blockID)
    {
        GameObject _bloque = null;
        switch (_blockID)
        {
            case "SLIME":
                _bloque = Instantiate(slimeBlockPrefab, nextPosition, Quaternion.identity, sublevelContainer);
                DamageBlock _slimeBlock = _bloque.GetComponent<DamageBlock>();
                _slimeBlock.SetupBlock(currentDepth, currentX, currentY);
                break;
            case "LAVA":
                _bloque = Instantiate(lavaBlockPrefab, nextPosition, Quaternion.identity, sublevelContainer);
                DamageBlock _lavaBlock = _bloque.GetComponent<DamageBlock>();
                _lavaBlock.SetupBlock(currentDepth, currentX, currentY);
                break;
            case "BALL":
                _bloque = Instantiate(ballBlockPrefab, nextPosition, Quaternion.identity, sublevelContainer);
                DamageBlock _ballBlock = _bloque.GetComponent<BallDmgBlock>();
                _ballBlock.SetupBlock(currentDepth, currentX, currentY);
                break;
        }
        return _bloque;
    }

    ResourceData GetResourceFromID(int _resID)
    {
        foreach(ResourceData _resData in ResourceManager.Instance.allAvailableResources)
        {
            if(_resData.id== _resID)
            {
                return _resData;
            }
        }

        return null;
    }

}
