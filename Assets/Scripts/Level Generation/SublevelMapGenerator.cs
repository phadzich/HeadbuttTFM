using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Windows;

public class SublevelMapGenerator : MonoBehaviour
{
    public ColorToPrefab[] colorMappings;
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

    public GameObject resourceBlockPrefab;
    public GameObject wallBlockPrefab;
    public GameObject floorBlockPrefab;
    public GameObject doorBlockPrefab;

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

            }
        }
    }

         private GameObject BlockFromPixel(int _x, int _y)
        {
            Color _pixelColor = mapTexture.GetPixel(_x, _y);


            if (_pixelColor.a == 0)
            {
                return null;
            }

            foreach(ColorToPrefab _color in colorMappings)
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
                break;
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

        return _bloque;
    }
    GameObject ConfigLVLBlock(string _blockID)
    {
        GameObject _bloque = null;
        switch (_blockID)
        {
            case "WALL":
                _bloque =  Instantiate(wallBlockPrefab, nextPosition, Quaternion.identity, sublevelContainer);
                break;
            case "FLOOR":
                _bloque = Instantiate(floorBlockPrefab, nextPosition, Quaternion.identity, sublevelContainer);
                break;
            case "DOOR":
                _bloque = Instantiate(doorBlockPrefab, nextPosition, Quaternion.identity, sublevelContainer);
                DoorBlock _doorBlock = _bloque.GetComponent<DoorBlock>();
                _doorBlock.SetupBlock(currentDepth);
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
