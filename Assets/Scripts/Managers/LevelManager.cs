using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;
    public GameObject resourceBlockPrefab;
    public Transform levelsContainer;
    public GameObject currentLoadedLevelContainer;

    public List<ResourceBlock> resourceBlocks;

    public List<LevelConfig> levelsList;
    private int maxLevelDepth;
    private int currentLevelDepth;

    public int sublevelWidth;
    public int sublevelHeight;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            Debug.Log("LevelManager Awake");
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        //CARGAMOS EL PRIMER NIVEL
        LoadLevel(levelsList[0]);
    }

    private void LoadLevel(LevelConfig _levelConfig)
    {

        //CREAMOS UN GAME OBJECT PARA QUE CONTENGA TODOS LOS SUBNIVELES
        currentLoadedLevelContainer = CreateEmptyGameobject(_levelConfig.levelName, levelsContainer);
        Debug.Log($"* Loading Level {_levelConfig.name}*");
        Level _level = currentLoadedLevelContainer.AddComponent<Level>();
        _level.SetupLevel(_levelConfig.name, _levelConfig);

        //DETERMINAMOS DATA DEL DEPTH
        maxLevelDepth = _levelConfig.subLevels.Count;
        currentLevelDepth = 0;
        //GENERAMOS EL PRIMER SUBNIVEL
        GenerateSublevel(_levelConfig.subLevels[0]);


    }


    public void GenerateSublevel(SublevelConfig _sublevelConfig)
    {
        GameObject sublevelContainer = CreateEmptyGameobject(_sublevelConfig.name, currentLoadedLevelContainer.transform);
        Sublevel _sublevel = sublevelContainer.AddComponent<Sublevel>();
        _sublevel.SetupSublevel(_sublevelConfig.id, currentLevelDepth, true, _sublevelConfig);
        int _cols = _sublevelConfig.width;
        int _rows = _sublevelConfig.height; 
        Debug.Log($"**Generating Sublevel {_sublevelConfig.name}**");
        InstanceNewBlocks(_cols, _rows, _sublevelConfig.resourcesList, sublevelContainer.transform);
    }

    public void InstanceNewBlocks(int  _cols, int _rows, List<ResourceData> _resources, Transform _sublevelContainer)
    {
        int _spacing = 1;

        float offsetX = (_cols - 1) * _spacing * 0.5f;
        float offsetZ = (_cols - 1) * _spacing * 0.5f;

        for (int z = 0; z < _rows; z++)
        {
            for (int x = 0; x < _cols; x++)
            {
                Vector3 _posicion = new Vector3(x * _spacing - offsetX, 0, z * _spacing - offsetZ);
                GameObject _bloque = Instantiate(resourceBlockPrefab, _posicion, Quaternion.identity, _sublevelContainer);
                ResourceBlock _resourceBlock = _bloque.GetComponent<ResourceBlock>();
                _resourceBlock.SetupBlock(0, x, x, _resources[Random.Range(0, _resources.Count)]);
                _bloque.name = $"{_resourceBlock.resourceData.shortName}_c{x}r_{z}";
                resourceBlocks.Add(_resourceBlock);
                if (x == _cols/2 && z == _rows/2)
                {
                    _resourceBlock.isDoor = true;
                }
            }
        }
    }

    /*
    private void DestroyAllBlocks()
    {
        foreach (Transform child in sublevelContainer)
        {
            Destroy(child.gameObject);
        }
    }
    */
    private GameObject CreateEmptyGameobject(string _name, Transform _parent)
    {
        GameObject newGameObject = new GameObject(_name);
        newGameObject.transform.SetParent(_parent);
        return newGameObject;
    }
}
