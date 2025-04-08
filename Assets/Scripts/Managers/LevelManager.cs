using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using PrimeTween;
public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;
    public GameObject playerCam;
    public float playerCamHeight = 10f;
    public GameObject resourceBlockPrefab;
    public Transform levelsContainer;
    public GameObject currentLoadedLevelContainer;
    public Level currentLevel;
    public GameObject doorTriggerPrefab;


    public List<ResourceBlock> resourceBlocks;

    public List<LevelConfig> levelsList;
    [SerializeField]
    private int maxLevelDepth;
    [SerializeField]
    private int currentLevelDepth;
    public float distanceBetweenSublevels;

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
        _sublevelContainer.transform.localPosition = new Vector3(0, distanceBetweenSublevels* -_depth, 0);
        //Debug.Log(_sublevelContainer.transform.localPosition);
        
        Sublevel _sublevel = _sublevelContainer.AddComponent<Sublevel>();
        _sublevel.SetupSublevel(_sublevelConfig.id, _depth, true, _sublevelConfig);
        int _cols = _sublevelConfig.width;
        int _rows = _sublevelConfig.height; 
        Debug.Log($"**Generating Sublevel {_sublevelConfig.name}**");
        InstanceNewBlocks(_cols, _rows, _sublevelConfig.resourcesList, _sublevelContainer.transform);
        Instantiate(doorTriggerPrefab, _sublevelContainer.transform);
    }

    public void ExitSublevel()
    {

        currentLevelDepth++;
        MoveCamDown(currentLevelDepth);
        EnterSublevel(currentLevel.config.subLevels[currentLevelDepth]);
    }
    public void EnterSublevel(SublevelConfig _sublevelConfig)
    {
           sublevelWidth = _sublevelConfig.width;
            sublevelHeight = _sublevelConfig.height;
        //CARGAMOS EL SIGUIENTE
        if (currentLevelDepth < maxLevelDepth)
        {
            GenerateSublevel(currentLevel.config.subLevels[currentLevelDepth + 1], currentLevelDepth + 1);
        }



        GameManager.Instance.RestartSublevelStats();
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
                Vector3 _posicion = new Vector3(x * _spacing - offsetX, _sublevelContainer.transform.position.y, z * _spacing - offsetZ);
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

    private void MoveCamDown(int _count)
    {
        Tween.PositionY(playerCam.transform,
            startValue: playerCam.transform.position.y, 
            endValue: (_count * -distanceBetweenSublevels) + playerCamHeight,
            duration:1f,
            ease:Ease.InOutQuad);
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
