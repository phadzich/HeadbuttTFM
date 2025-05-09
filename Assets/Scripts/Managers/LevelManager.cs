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


    Dictionary<int, List<Block>> floorBlocksPorSubnivel = new Dictionary<int, List<Block>>();
    Dictionary<int, List<Block>> resourceBlocksPorSubnivel = new Dictionary<int, List<Block>>();


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
        //Debug.Log("Generating at depth: " + _depth.ToString());
        GameObject _sublevelContainer = CreateEmptyGameobject(_sublevelConfig.name, currentLoadedLevelContainer.transform);
        _sublevelContainer.transform.localPosition = new Vector3(0, distanceBetweenSublevels * -_depth, 0);
        //Debug.Log(_sublevelContainer.transform.localPosition);

        Sublevel _sublevel = _sublevelContainer.AddComponent<Sublevel>();
        sublevelsList.Add(_sublevel);
        _sublevel.SetupSublevel(_sublevelConfig.id, _depth, true, _sublevelConfig);

        if (_sublevelConfig is MiningSublevelConfig _miningSublevel)
        {
            _sublevel.SetMiningObjectives(_miningSublevel.blocksToComplete);
            GenerateMiningSublevel(_miningSublevel, _sublevelContainer, _depth);
            CheckIfExtraBlocksNeeded(_depth, _miningSublevel);

        }
        else if (_sublevelConfig is NPCSublevelConfig _npcSublevel)
        {
            GenerateNPCSublevel(_npcSublevel, _sublevelContainer);
        }


    }
    private void GenerateMiningSublevel(MiningSublevelConfig _miningSublevel, GameObject _sublevelContainer, int _depth)
    {
        Debug.Log($"**Generating MINING Sublevel {_miningSublevel.name}**");
        CreateNewBlocksListForSublevel(_depth);
        GenerateMiningSublevelBlocks(_miningSublevel, _sublevelContainer, _depth);
        InstanceSublevelWalls(_sublevelContainer.transform, _miningSublevel);
    }
    void CreateNewBlocksListForSublevel(int _depth)
    {
        if (!floorBlocksPorSubnivel.ContainsKey(_depth))
        {
            floorBlocksPorSubnivel[_depth] = new List<Block>();
        }
        if (!resourceBlocksPorSubnivel.ContainsKey(_depth))
        {
            resourceBlocksPorSubnivel[_depth] = new List<Block>();
        }
    }
    void CheckIfExtraBlocksNeeded(int _depth, MiningSublevelConfig _miningSublevel)
    {
        //POR CADA RECURSO FALTANTE, REEMPLAZAMOS FLOORS POR RESOURCEBLOCKS
        //ReplaceFloorsByMissingResources(floorBlocksPorSubnivel[_depth], MissingResourceBlocksInLevel(_depth, RequiredResourceBlocksInLevel(_miningSublevel.sublevelRequirements), ActualResourceBlocksInLevel(_depth)), _depth);
        ReplaceFloorsByDamageBlocks(floorBlocksPorSubnivel[_depth], _miningSublevel.dmgBlocksList, _miningSublevel.dmgBlocksQty,_depth);
    }
    /*
    void ReplaceFloorsByMissingResources(List<Block> floorBlocksDisponibles, Dictionary<ResourceData, int> recursosFaltantes, int depth)
    {
    foreach (var kvp in recursosFaltantes){
            ResourceData recurso = kvp.Key;
            int cantidad = kvp.Value;

            for (int i = 0; i < cantidad && floorBlocksDisponibles.Count > 0; i++)
            {
                //Debug.Log($"AGREGANDO {recurso.name}");
                // Elige un bloque vacío aleatorio
                int index = Random.Range(0, floorBlocksDisponibles.Count);
                GameObject bloqueOriginal = floorBlocksDisponibles[index].gameObject;
                floorBlocksDisponibles.RemoveAt(index);

                // Guarda la posición y rotación del bloque actual
                Vector3 posicion = bloqueOriginal.transform.position;
                Quaternion rotacion = bloqueOriginal.transform.rotation;
                Vector2 coordenadas = bloqueOriginal.GetComponent<FloorBlock>().sublevelPosition;
                Transform padre = bloqueOriginal.transform.parent;

                // Elimina el bloque vacío original
                Destroy(bloqueOriginal);

                // Instancia el nuevo bloque con recurso
                InstantiateResourceBlock(posicion, padre, recurso, (int)coordenadas.x, (int)coordenadas.y, depth);
            }
        }

    }
    */
    void ReplaceFloorsByDamageBlocks(List<Block> floorBlocksDisponibles, List<DamageBlock> _damageBlocks, int _damageBlocksQty, int depth)
    {

            for (int i = 0; i < _damageBlocksQty && floorBlocksDisponibles.Count > 0; i++)
            {


                // Elige un bloque vacío aleatorio
                int index = Random.Range(0, floorBlocksDisponibles.Count);
                GameObject bloqueOriginal = floorBlocksDisponibles[index].gameObject;
                floorBlocksDisponibles.RemoveAt(index);

                //Elige un DamageBlock aleatorio
                int dmgIndex = Random.Range(0, _damageBlocks.Count);
                DamageBlock dmgBlock = _damageBlocks[dmgIndex];
            //Debug.Log($"AGREGANDO {dmgBlock.name}");
            // Guarda la posición y rotación del bloque actual
            Vector3 posicion = bloqueOriginal.transform.position;
                Quaternion rotacion = bloqueOriginal.transform.rotation;
                Vector2 coordenadas = bloqueOriginal.GetComponent<FloorBlock>().sublevelPosition;
                Transform padre = bloqueOriginal.transform.parent;

                // Elimina el bloque vacío original
                Destroy(bloqueOriginal);

                // Instancia el nuevo bloque con recurso
                InstantiateDamageBlock(dmgBlock,posicion, padre,(int)coordenadas.x, (int)coordenadas.y, depth);
            }
        }

    Dictionary<ResourceData, int> ActualResourceBlocksInLevel(int _depth)
    {
        Dictionary<ResourceData, int> conteoActual = new Dictionary<ResourceData, int>();

        foreach (Block _resourceBlock in resourceBlocksPorSubnivel[_depth])
        {
            ResourceData tipo = _resourceBlock.GetComponent<ResourceBlock>().resourceData; // KEY UNICO

            if (tipo != null)
            {
                if (!conteoActual.ContainsKey(tipo))
                    conteoActual[tipo] = 0;

                conteoActual[tipo]++;
            }
        }
        return conteoActual;
    }

    Dictionary<ResourceData, int> RequiredResourceBlocksInLevel(Dictionary<ResourceData, int> _sublevelRequirements)
    {
        Dictionary<ResourceData, int> requiredPorTipo = new Dictionary<ResourceData, int>();
        foreach (KeyValuePair<ResourceData, int> _requirement in _sublevelRequirements)
        {
            requiredPorTipo.Add(_requirement.Key, _requirement.Value);
        }
        return requiredPorTipo;
    }

    Dictionary<ResourceData, int> MissingResourceBlocksInLevel(int _depth, Dictionary<ResourceData, int> minimosPorTipo, Dictionary<ResourceData, int> conteoActual)
    {
        Dictionary<ResourceData, int> faltantesPorTipo = new Dictionary<ResourceData, int>();
        foreach (KeyValuePair<ResourceData, int> _requirement in minimosPorTipo)
        {
            ResourceData tipo = _requirement.Key;
            int requerido = _requirement.Value;
            int yaHay = conteoActual.ContainsKey(tipo) ? conteoActual[tipo] : 0;
            int faltan = Mathf.Max(0, requerido - yaHay);
            faltantesPorTipo[tipo] = faltan;
        }
        return faltantesPorTipo;
    }

    void PrintStringDictionaryContents(Dictionary<ResourceData, int> _dictionary) 
    {
        foreach (KeyValuePair<ResourceData, int> _kvp in _dictionary)
        {
            Debug.Log($"{_kvp.Key} {_kvp.Value}");
        }
    }


    private void GenerateNPCSublevel(NPCSublevelConfig _npcSublevel, GameObject _sublevelContainer)
    {
        // GENERAR AQUI SUBLEVEL DE TIPO NPC
        int _cols = _npcSublevel.width;
        int _rows = _npcSublevel.height;
        Debug.Log($"**Generating NPC Sublevel {_npcSublevel.name}**");
        InstanceNPCBlocks(_cols, _rows, _sublevelContainer.transform);
        InstanceSublevelWalls(_sublevelContainer.transform, _npcSublevel);
    }

    public void ExitSublevel()
    {
        currentLevelDepth++;
        PlayerManager.Instance.playerCamera.MoveCamDown(currentLevelDepth);
        EnterSublevel(currentLevel.config.subLevels[currentLevelDepth]);

    }
    public void EnterSublevel(SublevelConfig _sublevelConfig)
    {
        currentSublevel = sublevelsList[currentLevelDepth];
        if (_sublevelConfig is MiningSublevelConfig _miningSublevel)
        {
            sublevelWidth = _miningSublevel.width;
            sublevelHeight = _miningSublevel.height;
            PlayerManager.Instance.EnterMiningLevel();
            Debug.Log($"Entering {currentSublevel.id}");
            onSublevelEntered?.Invoke();
        }
        else if (_sublevelConfig is NPCSublevelConfig _npcSublevel)
        {
            // CONFIGURAR AQUI SUBLEVEL DE TIPO NPC
            sublevelWidth = _npcSublevel.width;
            sublevelHeight = _npcSublevel.height;
            PlayerManager.Instance.EnterNPCLevel();
            //ENTRAR A ESTADO CHECKPOINT
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

    public void GenerateMiningSublevelBlocks(MiningSublevelConfig _sublevelConfig, GameObject _sublevelContainer, int _depth)
    {
        //DETERMINAMOS TODAS LAS VARIABLES
        int _width = _sublevelConfig.width;
        int _height = _sublevelConfig.height;
        int _borderDepth = _sublevelConfig.borderDepth;
        int _centerX = _width / 2;
        int _centerY = _height / 2;
        float _noiseScale = _sublevelConfig.noiseScale;
        float _noiseThreshold = _sublevelConfig.noiseThreshold;
        int _spacing = 1;
        float offsetX = (_width - 1) * _spacing * 0.5f;
        float offsetZ = (_height - 1) * _spacing * 0.5f;
        int sublevelSeed = GameManager.Instance.globalSeed + _depth;
        System.Random rng = new System.Random(sublevelSeed);
        float perlinOffset = (float)rng.NextDouble() * 10000f;
        //HACEMOS UNA PRIMERA PASADA SIN CONSIDERAR LOS MINIMOS DE CADA RECURSO NECESARIO
        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                //CALCULAMOS LA POSICION DEL NUEVL BLOQUE CONSIDERANDO QUE QUEDE CENTRADO EL SUBNIVEL
                Vector3 _posicion = new Vector3(x * _spacing - offsetX, _sublevelContainer.transform.position.y, y * _spacing - offsetZ);
                
                //COLOCAMOS LA PUERTA AL CENTRO
                if (x == _centerX && y == _centerY)
                {
                    //SI NO ES EL ULTIMO SUBNIVEL
                    if(currentLevelDepth + 1 < maxLevelDepth)
                    {
                        InstantiateDoorBlock(_posicion, _sublevelContainer.transform, _sublevelConfig, _depth);
                    }
                    else
                    {
                        InstantiateFloorBlock(_posicion, _sublevelContainer.transform, x, y,_depth);
                    }

                }
                //COLOCAMOS LOS RECURSOS EN LOS BORDES
                else if (IsOnBorder(x, y,_width,_height, _borderDepth))
                {
                    float noise = Mathf.PerlinNoise((x * _noiseScale)+perlinOffset, y * _noiseScale);
                    if (noise > _noiseThreshold)
                    {
                        ResourceData _randomResource = _sublevelConfig.resourcesList[Random.Range(0, _sublevelConfig.resourcesList.Count)];
                        InstantiateResourceBlock(_posicion, _sublevelContainer.transform, _randomResource, x,y, _depth);
                    }
                    else
                    {
                        InstantiateFloorBlock(_posicion, _sublevelContainer.transform, x, y, _depth);
                    }
                }
                //RELLENAMOS CON PISO NORMAL U OTRAS COSAS
                else
                {
                    InstantiateFloorBlock(_posicion, _sublevelContainer.transform, x, y, _depth);
                }
            }
        }
    }

    bool IsOnBorder(int x, int y, int width, int height, int borderThickness)
    {
        return (
            x < borderThickness || y < borderThickness || x >= width - borderThickness || y >= height - borderThickness
        );
    }

    void InstantiateDoorBlock(Vector3 _posicion, Transform _sublevelContainer, MiningSublevelConfig _sublevelConfig, int _depth)
    {
        Sublevel _parentSublevel = sublevelsList[_depth];
        GameObject _door = Instantiate(doorBlockPrefab, _posicion, Quaternion.identity, _sublevelContainer);
        DoorBlock _doorBlock = _door.GetComponent<DoorBlock>();
        _doorBlock.SetupBlock(_parentSublevel);
    }

    void InstantiateFloorBlock(Vector3 _posicion, Transform _sublevelContainer, int _x, int _y, int _depth)
    {
        GameObject _floor = Instantiate(floorBlockPrefab, _posicion, Quaternion.identity, _sublevelContainer);
        FloorBlock _floorBlock = _floor.GetComponent<FloorBlock>();
        _floorBlock.SetupBlock(0, _x, _y);
        floorBlocksPorSubnivel[_depth].Add(_floorBlock);
    }

    void InstantiateResourceBlock(Vector3 _posicion, Transform _sublevelContainer, ResourceData _resourceData, int _x, int _y, int _depth)
    {
        GameObject _bloque = Instantiate(resourceBlockPrefab, _posicion, Quaternion.identity, _sublevelContainer);
        ResourceBlock _resourceBlock = _bloque.GetComponent<ResourceBlock>();
        _resourceBlock.SetupBlock(0, _x, _y, _resourceData);
        _bloque.name = $"{_resourceBlock.resourceData.shortName}_c{_x}r_{_y}";
        resourceBlocksPorSubnivel[_depth].Add(_resourceBlock);
    }

void InstantiateDamageBlock(DamageBlock _dmgBlock,Vector3 _posicion, Transform _sublevelContainer, int _x, int _y, int _depth)
{
    GameObject _dmgBlockGO = Instantiate(_dmgBlock.gameObject, _posicion, Quaternion.identity, _sublevelContainer);
        _dmgBlock.SetupBlock(0, _x, _y);
        _dmgBlockGO.name = $"{_dmgBlock.name}_c{_x}r_{_y}";
    //resourceBlocksPorSubnivel[_depth].Add(_resourceBlock);
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
