using System.Runtime.CompilerServices;
using UnityEngine;

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

    private void Start()
    {
        GenerateSublevel(this.transform, testMap);
    }
    public void GenerateSublevel(Transform _parentTransform, Texture2D _inputMap)
    {
        mapWidth = _inputMap.width;
        mapHeight = _inputMap.height;
        mapTexture = _inputMap;
        sublevelContainer = _parentTransform;
        InstanceAllBlocks(mapWidth, mapHeight);
    }

    void InstanceAllBlocks(int _width, int _height)
    {
        int _spacing = 1;
        float offsetX = (_width - 1) * _spacing * 0.5f;
        float offsetZ = (_height - 1) * _spacing * 0.5f;
        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapHeight; y++)
            {
                Vector3 _posicion = new Vector3(x * _spacing - offsetX, sublevelContainer.transform.position.y, y * _spacing - offsetZ);
                Instantiate(BlockFromPixel(x, y), _posicion, Quaternion.identity, sublevelContainer);
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
                return _color.prefab;
            }
            else { Debug.Log("noMatch"); }
            }


        return null;
        }

    //LOS COLOR MAPPINGS DEBEN SER COLOR A ALGO COMO RES_01_ICE Y HACER UN PARSER QUE AGARRE PREFAB RES, APLIQUE RECURSO 01 Y APLIQUE ESTADO ICE
    //PRIMERO UNA FUNCION QUE SEPARE TYPE, DATA, VARIANT SEGUN LOS SUBGUIONES
    //LUEGO, DEPEDIENDO DEL TYPE TENER FUNCIONES INDEPENDIENTES PARA NEUTRAL, RES, DMG, DOOR, ETC
    //INSTANCIO, CONFIGURO Y EL GRID INSTANCER SOLO LO POSICIONA CORECTAMENTE, NO LO CONFIGURA.



}
