using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Unity.Android.Gradle;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Windows;

public class SublevelMapNewGenerator : MonoBehaviour
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

    Vector3 nextPosition;
    public Dictionary<Vector2Int, BlockNS> currentBlocks = new();

    private MapContext context;

    public void GenerateSublevel(Transform _parentTransform, Texture2D _inputMap, int _depth, MiningSublevelConfig _miningConfig, NPCSublevelConfig _npcConfig, Sublevel _sublevel)
    {
        context = new MapContext
        {
            depth = _depth,
            x = 0,
            y = 0,
            sublevel = _sublevel,
            miningConfig = _miningConfig,
            npcConfig = _npcConfig
        };

        LevelManager.Instance.currentContext = context;
        mapWidth = _inputMap.width;
        mapHeight = _inputMap.height;
        mapTexture = _inputMap;
        sublevelContainer = _parentTransform;
        InstanceAllBlocks(mapWidth, mapHeight);
        if (_miningConfig != null)
        {
            RestartSublevelStats();
        }


    }



    private void RestartSublevelStats()
    {
        
        ResetActiveObjectives(context.sublevel.activeObjectives);
        ResetActiveRequirements(context.sublevel.activeChestRequirements);
        ResetActiveRequirements(context.sublevel.activeGateRequirements);
    }

    private void ResetActiveRequirements(List<IRequirement> _list)
    {
        foreach (IRequirement _req in _list)
        {
            _req.current = 0;
        }
    }

    private void ResetActiveObjectives(List<ISublevelObjective> _list)
    {
        foreach (ISublevelObjective _obj in _list)
        {
            _obj.current = 0;
        }
    }

        void InstanceAllBlocks(int _width, int _height)
    {
        currentBlocks.Clear();
        int _spacing = 1;
        float offsetX = (_width - 1) * _spacing * 0.5f;
        float offsetZ = (_height - 1) * _spacing * 0.5f;
        for (int x = 0; x < mapWidth; x++)
        {
            context.x = x;
            for (int y = 0; y < mapHeight; y++)
            {
                context.y = y;
                nextPosition = new Vector3(x * _spacing - offsetX, sublevelContainer.transform.position.y, y * _spacing - offsetZ);
                var _newBlock = BlockFromPixel(x, y);
                Vector2Int pos = new(x, y);
                //Debug.Log(_newBlock);
                currentBlocks[pos] = _newBlock.GetComponent<BlockNS>();
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
        //Debug.Log(_pixelColor.a);

        if (_pixelColor.a == 0)
        {
            return null;
        }

        foreach (ColorToString _color in colorMappings.colors)
        {

            if (_color.color == _pixelColor)
            {
                return GetBlockFromString(_color);
            }
        }


        return null;
    }

    //LOS COLOR MAPPINGS DEBEN SER COLOR A ALGO COMO RES_01_ICE Y HACER UN PARSER QUE AGARRE PREFAB RES, APLIQUE RECURSO 01 Y APLIQUE ESTADO ICE
    //PRIMERO UNA FUNCION QUE SEPARE TYPE, DATA, VARIANT SEGUN LOS SUBGUIONES
    //LUEGO, DEPEDIENDO DEL TYPE TENER FUNCIONES INDEPENDIENTES PARA NEUTRAL, RES, DMG, DOOR, ETC
    //INSTANCIO, CONFIGURO Y EL GRID INSTANCER SOLO LO POSICIONA CORECTAMENTE, NO LO CONFIGURA.

 
    GameObject GetBlockFromString(ColorToString _blockString)
    {
        var _stringParts = _blockString.blockString.Split('_');
        string _blockName = _stringParts[0];
        string _blockVariant = _stringParts[1];

        GameObject _prefab = _blockString.prefab;

        GameObject _bloque = Instantiate(_prefab, nextPosition, Quaternion.identity, sublevelContainer);
        _bloque.GetComponent<BlockNS>().SetupBlock(_blockName, _blockVariant, context);

        return _bloque;
    }

}
