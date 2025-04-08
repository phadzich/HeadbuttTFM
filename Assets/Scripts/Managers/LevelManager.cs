using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using PrimeTween;
using static UnityEngine.Rendering.DebugUI.Table;
using System.Drawing;
public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;
    public List<LevelConfig> levelsList;

    [Header("CURRENT LEVEL")]
    public Level currentLevel;
    public List<ResourceBlock> resourceBlocks;
    [SerializeField]
    private int maxLevelDepth;
    [SerializeField]
    private int currentLevelDepth;
    public int sublevelWidth;
    public int sublevelHeight;

    [Header("MAIN CAMERA")]
    public GameObject playerCam;
    public float playerCamHeight = 10f;
    [Header("GENERAL PREFABS")]
    public GameObject npcBlockPrefab;
    public GameObject resourceBlockPrefab;
    public GameObject doorTriggerPrefab;
    public GameObject sublevelWallPrefab;
    [Header("LEVEL CONTAINERS")]
    public Transform levelsContainer;
    public GameObject currentLoadedLevelContainer;


    [Header("GENERATOR CONFIG")]
    public float distanceBetweenSublevels;


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
        currentLevel = currentLoadedLevelContainer.AddComponent<Level>();
        currentLevel.SetupLevel(_levelConfig.name, _levelConfig);

        //DETERMINAMOS DATA DEL DEPTH
        maxLevelDepth = _levelConfig.subLevels.Count-1;
        currentLevelDepth = 0;

        //GENERAMOS EL PRIMER SUBNIVEL
        GenerateSublevel(_levelConfig.subLevels[currentLevelDepth], currentLevelDepth);
        //INDICAMOS QUE HEMOS ENTRADO EN EL
        EnterSublevel(_levelConfig.subLevels[currentLevelDepth]);

    }

    public void GenerateSublevel(SublevelConfig _sublevelConfig, int _depth)
    {
        //Debug.Log("Generating at depth: " + _depth.ToString());
        GameObject _sublevelContainer = CreateEmptyGameobject(_sublevelConfig.name, currentLoadedLevelContainer.transform);
        _sublevelContainer.transform.localPosition = new Vector3(0, distanceBetweenSublevels * -_depth, 0);
        //Debug.Log(_sublevelContainer.transform.localPosition);

        Sublevel _sublevel = _sublevelContainer.AddComponent<Sublevel>();
        _sublevel.SetupSublevel(_sublevelConfig.id, _depth, true, _sublevelConfig);

        if (_sublevelConfig is MiningSublevelConfig _miningSublevel)
        {

            int _cols = _miningSublevel.width;
            int _rows = _miningSublevel.height;
            Debug.Log($"**Generating MINING Sublevel {_miningSublevel.name}**");
            InstanceMiningBlocks(_cols, _rows, _miningSublevel.resourcesList, _sublevelContainer.transform);
            InstanceSublevelWalls(_sublevelContainer.transform, _miningSublevel);

        }
        else if (_sublevelConfig is NPCSublevelConfig _npcSublevel)
        {
            // GENERAR AQUI SUBLEVEL DE TIPO NPC
            int _cols = _npcSublevel.width;
            int _rows = _npcSublevel.height;
            Debug.Log($"**Generating NPC Sublevel {_npcSublevel.name}**");
            InstanceNPCBlocks(_cols, _rows, _sublevelContainer.transform);
            InstanceSublevelWalls(_sublevelContainer.transform, _npcSublevel);
        }


    }

    public void ExitSublevel()
    {

        currentLevelDepth++;
        MoveCamDown(currentLevelDepth);
        EnterSublevel(currentLevel.config.subLevels[currentLevelDepth]);
    }
    public void EnterSublevel(SublevelConfig _sublevelConfig)
    {

        if (_sublevelConfig is MiningSublevelConfig _miningSublevel)
        {

            sublevelWidth = _miningSublevel.width;
            sublevelHeight = _miningSublevel.height;

        }
        else if (_sublevelConfig is NPCSublevelConfig _npcSublevel)
        {
            // CONFIGURAR AQUI SUBLEVEL DE TIPO NPC
            sublevelWidth = _npcSublevel.width;
            sublevelHeight = _npcSublevel.height;
        }

        //CARGAMOS EL SIGUIENTE
        if (currentLevelDepth < maxLevelDepth)
        {
            GenerateSublevel(currentLevel.config.subLevels[currentLevelDepth + 1], currentLevelDepth + 1);
        }



        GameManager.Instance.RestartSublevelStats();
    }

    public void InstanceMiningBlocks(int  _cols, int _rows, List<ResourceData> _resources, Transform _sublevelContainer)
    {
        int _spacing = 1;

        float offsetX = (_cols - 1) * _spacing * 0.5f;
        float offsetZ = (_cols - 1) * _spacing * 0.5f;

        for (int z = 0; z < _rows; z++)
        {
            for (int x = 0; x < _cols; x++)
            {
                Vector3 _posicion = new Vector3(x * _spacing - offsetX, _sublevelContainer.transform.position.y, z * _spacing - offsetZ);
                GameObject _bloque = Instantiate(resourceBlockPrefab, _posicion, Quaternion.identity, _sublevelContainer);
                ResourceBlock _resourceBlock = _bloque.GetComponent<ResourceBlock>();
                _resourceBlock.SetupBlock(0, x, z, _resources[Random.Range(0, _resources.Count)]);
                _bloque.name = $"{_resourceBlock.resourceData.shortName}_c{x}r_{z}";
                resourceBlocks.Add(_resourceBlock);
                if (x == _cols/2 && z == _rows/2 && currentLevelDepth+1 < maxLevelDepth)
                {
                    Debug.Log("Door trigger at" + currentLevelDepth);
                    _resourceBlock.isDoor = true;
                }
            }
        }
    }

    public void InstanceNPCBlocks(int _cols, int _rows, Transform _sublevelContainer)
    {
        int _spacing = 1;

        float offsetX = (_cols - 1) * _spacing * 0.5f;
        float offsetZ = (_cols - 1) * _spacing * 0.5f;

        for (int z = 0; z < _rows; z++)
        {
            for (int x = 0; x < _cols; x++)
            {
                Vector3 _posicion = new Vector3(x * _spacing - offsetX, _sublevelContainer.transform.position.y, z * _spacing - offsetZ);
                GameObject _bloque = Instantiate(npcBlockPrefab, _posicion, Quaternion.identity, _sublevelContainer);
                NPCBlock _npcBlock = _bloque.GetComponent<NPCBlock>();

                _bloque.name = $"{_npcBlock.name}_c{x}r_{z}";
                if (x == _cols / 2 && z == _rows / 2 && currentLevelDepth + 1 < maxLevelDepth)
                {
                    //Debug.Log("NPC Door trigger at" + currentLevelDepth);
                    _npcBlock.isDoor = true;
                }
                _npcBlock.SetupBlock(0, x, z);
            }
        }
    }

    private void InstanceSublevelWalls(Transform _sublevelContainer, SublevelConfig _config)
    {
        Vector3 leftPos = _sublevelContainer.transform.position + new Vector3(-_config.width / 2f, 0, 0);
        Vector3 rightPos = _sublevelContainer.transform.position + new Vector3(_config.width / 2f, 0, 0);
        Vector3 topPos = _sublevelContainer.transform.position + new Vector3(0,0, -_config.height / 2f);
        Vector3 bottomPos = _sublevelContainer.transform.position + new Vector3(0,0, _config.height / 2f);

        var leftWall = Instantiate(sublevelWallPrefab, leftPos, Quaternion.Euler(0, 90, 0), _sublevelContainer);
        var rightWall = Instantiate(sublevelWallPrefab, rightPos, Quaternion.Euler(0, -90, 0), _sublevelContainer);
        var topWall = Instantiate(sublevelWallPrefab, topPos, Quaternion.Euler(0, 0, 0), _sublevelContainer);
        var botWall = Instantiate(sublevelWallPrefab, bottomPos, Quaternion.Euler(0, 180, 0), _sublevelContainer);

        leftWall.transform.localScale = new Vector3(_config.width, distanceBetweenSublevels, 1);
        rightWall.transform.localScale = new Vector3(_config.width, distanceBetweenSublevels, 1);
        topWall.transform.localScale = new Vector3(_config.height, distanceBetweenSublevels, 1);
        botWall.transform.localScale = new Vector3(_config.height, distanceBetweenSublevels, 1);
    }

    private void MoveCamDown(int _count)
    {
        Tween.PositionY(playerCam.transform,
            startValue: playerCam.transform.position.y, 
            endValue: (_count * -distanceBetweenSublevels) + playerCamHeight,
            duration:1f,
            ease:Ease.InOutQuad);
}


    private GameObject CreateEmptyGameobject(string _name, Transform _parent)
    {
        GameObject newGameObject = new GameObject(_name);
        newGameObject.transform.SetParent(_parent);
        return newGameObject;
    }
}
