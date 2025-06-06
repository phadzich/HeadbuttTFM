using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using PrimeTween;
using static UnityEngine.Rendering.DebugUI.Table;
using System.Drawing;
using UnityEngine.UIElements;
using static UnityEngine.EventSystems.EventTrigger;
using Unity.VisualScripting;
using System;
using Random = UnityEngine.Random;
public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;
    public List<LevelConfig> levelsList;
    public SublevelMapGenerator sublevelMapGenerator;

    [Header("CURRENT LEVEL")]
    public Level currentLevel;
    public Sublevel currentSublevel;
    public List<Sublevel> sublevelsList;
    [SerializeField]
    private int maxLevelDepth;
    [SerializeField]
    public int currentLevelDepth;
    public int sublevelWidth;
    public int sublevelHeight;

    [Header("GENERAL PREFABS")]
    public GameObject npcBlockPrefab;
    public GameObject resourceBlockPrefab;
    public GameObject doorBlockPrefab;
    public GameObject floorBlockPrefab;
    public GameObject doorTriggerPrefab;
    public GameObject sublevelWallPrefab;
    [Header("LEVEL CONTAINERS")]
    public Transform levelsContainer;
    public GameObject currentLoadedLevelContainer;


    [Header("GENERATOR CONFIG")]
    public float distanceBetweenSublevels;

    public bool NPCLevel = false;

    public Action onSublevelBlocksMined;
    public Action onSublevelEntered;
    public int currentSublevelBlocksMined;

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
        Debug.Log("LevelManager START");
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
        maxLevelDepth = _levelConfig.subLevels.Count - 1;
        currentLevelDepth = 0;

        //GENERAMOS EL PRIMER SUBNIVEL
        GenerateSublevel(_levelConfig.subLevels[currentLevelDepth], currentLevelDepth);

        //INDICAMOS QUE HEMOS ENTRADO EN EL
        EnterSublevel(_levelConfig.subLevels[currentLevelDepth]);

    }

    public void GenerateSublevel(SublevelConfig _sublevelConfig, int _depth)
    {

        GameObject _sublevelContainer = CreateEmptyGameobject(_sublevelConfig.name, currentLoadedLevelContainer.transform);
        _sublevelContainer.transform.localPosition = new Vector3(0, distanceBetweenSublevels * -_depth, 0);


        Sublevel _sublevel = _sublevelContainer.AddComponent<Sublevel>();
        sublevelsList.Add(_sublevel);
        _sublevel.SetupSublevel(_sublevelConfig.id, _depth, true, _sublevelConfig);

        if (_sublevelConfig is MiningSublevelConfig _miningSublevel)
        {
            _sublevel.SetMiningObjectives(_miningSublevel.blocksToComplete);

            sublevelMapGenerator.GenerateSublevel(_sublevelContainer.transform, _miningSublevel.sublevel2DMap, _depth);
        }
        else if (_sublevelConfig is NPCSublevelConfig _npcSublevel)
        {
            sublevelMapGenerator.GenerateSublevel(_sublevelContainer.transform, _npcSublevel.sublevel2DMap, _depth);
        }

    }

    void PrintStringDictionaryContents(Dictionary<ResourceData, int> _dictionary) 
    {
        foreach (KeyValuePair<ResourceData, int> _kvp in _dictionary)
        {
            Debug.Log($"{_kvp.Key} {_kvp.Value}");
        }
    }

    public void ExitSublevel()
    {
        if (sublevelsList[currentLevelDepth].isTotallyMined)
        {
            HelmetManager.Instance.ResetHelmetsStats();
            PlayerManager.Instance.MaxUpLives();
            UIManager.Instance.currentHelmetHUD.RestartEquippedCounters();
        }
        sublevelsList[currentLevelDepth].gameObject.SetActive(false);
        currentLevelDepth++;
        PlayerManager.Instance.playerCamera.MoveCamDown(currentLevelDepth);
        MatchManager.Instance.RestartMatches();
        EnterSublevel(currentLevel.config.subLevels[currentLevelDepth]);

    }
    public void EnterSublevel(SublevelConfig _sublevelConfig)
    {
        currentSublevel = sublevelsList[currentLevelDepth];
        if (_sublevelConfig is MiningSublevelConfig _miningSublevel)
        {
            PlayerManager.Instance.EnterMiningLevel();
            //Debug.Log($"Entering {currentSublevel.id}");
            onSublevelEntered?.Invoke();
        }
        else if (_sublevelConfig is NPCSublevelConfig _npcSublevel)
        {
            //ENTRAR A ESTADO CHECKPOINT
            PlayerManager.Instance.EnterNPCLevel();
            HelmetManager.Instance.ResetHelmetsStats();
            PlayerManager.Instance.MaxUpLives();
            UIManager.Instance.currentHelmetHUD.RestartEquippedCounters();

        }

        //CARGAMOS EL SIGUIENTE
        if (currentLevelDepth < maxLevelDepth)
        {
            GenerateSublevel(currentLevel.config.subLevels[currentLevelDepth + 1], currentLevelDepth + 1);
        }



        // RESETEAR O LO QUE SEA LOS HELMETS
        //HelmetManager.Instance.NewSublevel();
        //Debug.Log("Reseting Helmet Stats");
        GameManager.Instance.RestartSublevelStats();
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
                    //_npcBlock.isDoor = true;
                }
                _npcBlock.SetupBlock(0, x, z);
            }
        }
    }


    public void IncreaseMinedBlocks(int _newMinedBlocks)
    {
        currentSublevel.currentBlocksMined += _newMinedBlocks;
        onSublevelBlocksMined?.Invoke();
    }


    private GameObject CreateEmptyGameobject(string _name, Transform _parent)
    {
        GameObject newGameObject = new GameObject(_name);
        newGameObject.transform.SetParent(_parent);
        return newGameObject;
    }
}
