using NUnit.Framework;
using PrimeTween;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.AI.Navigation;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;
using Random = UnityEngine.Random;
using System.Linq;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;
    public CheckpointSystem checkpointSystem;
    public List<LevelConfig> levelsList;
    public SublevelMapNewGenerator sublevelMapNewGenerator;

    public LoadingManager loadingManager;

    [Header("CURRENT LEVEL")]
    public MapContext currentContext;
    private List<ISublevelObjective> activeObjectives;

    public Level currentLevel;
    public Sublevel currentSublevel;
    public List<Sublevel> sublevelsList;
    [SerializeField]
    private int maxLevelDepth;
    [SerializeField]
    public int currentLevelDepth;
    public int sublevelWidth;
    public int sublevelHeight;
    public GameObject currentExitDoor;
    public GameObject currentDropBlock;
    public bool onMiningMusic = false;

    [Header("LEVEL CONTAINERS")]
    public Transform levelsContainer;
    public GameObject currentLoadedLevelContainer;


    [Header("GENERATOR CONFIG")]
    public float distanceBetweenSublevels;

    public bool NPCLevel = false;
    [SerializeField] private Material fogMaterial;
    [SerializeField] private Material wallMaterial;

    public Action<Sublevel> onSublevelEntered;

    public NavMeshSurface navMeshSurface; // Arrastra el GameObject con NavMeshSurface aqu�

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
        ResourceManager.Instance.InitOwnedResources();
        //CARGAMOS EL PRIMER NIVEL
        LoadLevel(levelsList[0]);

    }

    public void StartGame()
    {
        ChangeLevel(0);
    }

private void ChangeFogColor()
    {
        Debug.Log("FOGDHADE");
        UnityEngine.Color _high = currentLevel.config.fogHigh;
        UnityEngine.Color _low = currentLevel.config.fogLow;
        fogMaterial.SetColor("_Color_High", _high);
        fogMaterial.SetColor("_Color_Low", _low);
        ChangeWallColor();
    }

    private void ChangeWallColor()
    {
        UnityEngine.Color _base = currentLevel.config.fogHigh;
        UnityEngine.Color _emision = currentLevel.config.fogLow;
        wallMaterial.SetColor("_BaseColor", currentLevel.config.wallBase);
        wallMaterial.SetColor("_EmissionColor", currentLevel.config.wallEmission);
    }

    private void UpdateFogSpeed()
    {
        float t = (float)currentLevelDepth / Mathf.Max(1, (float)LevelTotalDepth(currentLevel.config)); // Normaliza entre 0 y 1
        float fogSpeed = Mathf.Lerp(0.005f, 0.08f, t); // 🔹 Va de 0.01 → 0.1

        fogMaterial.SetFloat("_Speed", fogSpeed);
    }
    private void LoadLevel(LevelConfig _levelConfig)
    {

        //CREAMOS UN GAME OBJECT PARA QUE CONTENGA TODOS LOS SUBNIVELES
        currentLoadedLevelContainer = CreateEmptyGameobject(_levelConfig.levelName, levelsContainer);
        Debug.Log($"* Loading Level {_levelConfig.name}*");
        currentLevel = currentLoadedLevelContainer.AddComponent<Level>();
        currentLevel.SetupLevel(_levelConfig.name, _levelConfig);
        SoundManager.PlaySound(AmbientType.LEVEL_AMBIENT, currentLevel.config.levelAmbient);
        ChangeFogColor();
        //DETERMINAMOS DATA DEL DEPTH
        maxLevelDepth = _levelConfig.subLevels.Count - 1;
        currentLevelDepth = 0;
        PreloadSublevelsList();
        //GENERAMOS EL PRIMER SUBNIVEL
        GenerateSublevel(_levelConfig.subLevels[currentLevelDepth], currentLevelDepth);
        PlayerManager.Instance.EnterNewLevel();
        //INDICAMOS QUE HEMOS ENTRADO EN EL
        EnterSublevel(_levelConfig.subLevels[currentLevelDepth]);

    }

    private void LoadLevelAndCheckpoint(LevelConfig _levelConfig,int _checkpointDepth)
    {

        //CREAMOS UN GAME OBJECT PARA QUE CONTENGA TODOS LOS SUBNIVELES
        currentLoadedLevelContainer = CreateEmptyGameobject(_levelConfig.levelName, levelsContainer);
        Debug.Log($"* Loading Level {_levelConfig.name}*");
        currentLevel = currentLoadedLevelContainer.AddComponent<Level>();
        currentLevel.SetupLevel(_levelConfig.name, _levelConfig);
        SoundManager.PlaySound(AmbientType.LEVEL_AMBIENT, currentLevel.config.levelAmbient);
        ChangeFogColor();
        //DETERMINAMOS DATA DEL DEPTH
        maxLevelDepth = _levelConfig.subLevels.Count - 1;
        currentLevelDepth = _checkpointDepth;
        PreloadSublevelsList();
        //GENERAMOS EL PRIMER SUBNIVEL
        GenerateSublevel(_levelConfig.subLevels[currentLevelDepth], currentLevelDepth);
        PlayerManager.Instance.EnterNewLevel();
        //INDICAMOS QUE HEMOS ENTRADO EN EL
        EnterSublevel(_levelConfig.subLevels[currentLevelDepth]);

    }

    public void PreloadSublevelsList()
    {
        foreach(SublevelConfig _sublevelConfig in currentLevel.config.subLevels)
        {
            sublevelsList.Add(null);
        }
    }

    public void GenerateSublevel(SublevelConfig _sublevelConfig, int _depth)
    {
        GameObject _sublevelContainer = CreateEmptyGameobject(_sublevelConfig.name, currentLoadedLevelContainer.transform);
        _sublevelContainer.transform.localPosition = new Vector3(0, distanceBetweenSublevels * -_depth, 0);


        Sublevel _sublevel = _sublevelContainer.AddComponent<Sublevel>();
        sublevelsList[_depth]=_sublevel;
        _sublevel.SetupSublevel(_sublevelConfig.id, _depth, true, _sublevelConfig);

        if (_sublevelConfig is MiningSublevelConfig _miningSublevel)
        {
            sublevelMapNewGenerator.GenerateSublevel(_sublevelContainer.transform, _miningSublevel.sublevel2DMap, _depth, _miningSublevel, null, _sublevel);
        }
        else if (_sublevelConfig is NPCSublevelConfig _npcSublevel)
        {
            sublevelMapNewGenerator.GenerateSublevel(_sublevelContainer.transform, _npcSublevel.sublevel2DMap, _depth, null, _npcSublevel, _sublevel);
        }

    }

    public void ExitSublevel()
    {

        StartCoroutine(DestroySublevelContentDelayed(currentSublevel, 3f));

        currentLevelDepth++;
        //PlayerManager.Instance.playerCamera.MoveFogDown(currentLevelDepth);
        MatchManager.Instance.RestartMatches();
        EnterSublevel(currentLevel.config.subLevels[currentLevelDepth]);

    }
    public void ExitLevel()
    {
        Debug.Log($"Exiting {currentLevel}");
        StartCoroutine(DestroySublevelContentDelayed(currentSublevel, 3f));
        MatchManager.Instance.RestartMatches();

        currentDropBlock = null;
    }

    public void ChangeLevel(int _levelIndex)
    {
        loadingManager.LoadLevelCoroutine(() =>
        {
            PlayerManager.Instance.ShowPlayerMesh(true);
            UIManager.Instance.ShowNPCKey(false);
            ExitLevel();
            UnloadLevel();
            LoadLevel(levelsList[_levelIndex]);
        });
    }

    public void ChangeLevelAndCheckpoint(int _levelIndex, int _checkIndex)
    {
        loadingManager.LoadLevelCoroutine(() =>
        {
            PlayerManager.Instance.ShowPlayerMesh(true);
        UIManager.Instance.ShowNPCKey(false);
        ExitLevel();
        UnloadLevel();
        LoadLevelAndCheckpoint(levelsList[_levelIndex], _checkIndex);
        });
    }



    private void UnloadLevel()
    {
        Destroy(currentLoadedLevelContainer.gameObject);
    }

    public void DestroySublevelContent(Sublevel _sublevel)
    {
        if (_sublevel != null)
        {
            Destroy(_sublevel.gameObject);
        }
    }

    public IEnumerator DestroySublevelContentDelayed(Sublevel _sublevel, float _waitTime)
    {
        yield return new WaitForSeconds(_waitTime);
        DestroySublevelContent(_sublevel);
    }

    public void DestroySublevelsUntilCheckpoint(int _targetDepth)
    {
        for (int i = currentLevelDepth+1;i> _targetDepth; i--)
        {
            DestroySublevelContent(sublevelsList[i]);
        }
    }

    public void EnterSublevel(SublevelConfig _sublevelConfig)
    {
        currentSublevel = sublevelsList[currentLevelDepth];
        UpdateFogSpeed();
        Debug.Log($"Entering {currentSublevel}");
        onSublevelEntered?.Invoke(currentSublevel);
        int _realWorldDepth = currentLevel.config.subLevels.IndexOf(currentSublevel.config);

        PlayerManager.Instance.playerCamera.MoveFogToDepth(_realWorldDepth);
        if (_sublevelConfig is MiningSublevelConfig _miningSublevel)
        {
            if (!onMiningMusic)
            {
                SoundManager.PlaySound(MusicType.LEVEL_MUSIC, currentLevel.config.levelMusic);
                onMiningMusic = true;
            }

            PlayerManager.Instance.EnterMiningLevel();
        }

        else if (_sublevelConfig is NPCSublevelConfig _npcSublevel)
        {
            SoundManager.PlaySound(MusicType.LEVEL_MUSIC, currentLevel.config.levelNpcMusic);
            onMiningMusic = false;

            //ENTRAR A ESTADO CHECKPOINT
            PlayerManager.Instance.EnterNPCLevel();
            HelmetManager.Instance.ResetHelmetsStats();
            checkpointSystem.EnterNPCSublevel(_npcSublevel, sublevelsList[currentLevelDepth]);
        }

        TryGenerateNextSublevel();
        ActivateDialogIfAvailable(_sublevelConfig);
        MovePlayerToDropBlock();
        CheckMaxDepth();
    }
    
    private void CheckMaxDepth()
    {
        int _levelMax = currentLevel.config.maxDepth;
        if(currentLevelDepth > _levelMax)
        {
            currentLevel.config.maxDepth = currentLevelDepth;
        }
    }

    public float LevelProgress(LevelConfig _level)
    {
        int _total = LevelTotalDepth(_level);
        int _actual = LevelMaxDepth(_level);

        float _progress = (float)_actual / (float)_total;


        return _progress;
    }

    public int LevelTotalDepth(LevelConfig _level)
    {
        return levelsList[levelsList.IndexOf(_level)].subLevels.Count-1;

    }

    public int LevelMaxDepth(LevelConfig _level)
    {
        return levelsList[levelsList.IndexOf(_level)].maxDepth;

    }

    private void MovePlayerToDropBlock()
    {
        if (currentDropBlock != null)
        {
            PlayerManager.Instance.playerMovement.MoveToDrop(currentDropBlock.transform.position);
        }
        else
        {
            //Debug.Log("CHECK");
        }
    }

    private void ActivateDialogIfAvailable(SublevelConfig _sublevelConfig)
    {
        if (_sublevelConfig.dialogueSequence != null)
        {
            UIManager.Instance.dialogueSystem.StartDialogue(_sublevelConfig.dialogueSequence);
        }
    }


    private void TryGenerateNextSublevel()
    {
        if (currentLevelDepth < maxLevelDepth)
        {
            GenerateSublevel(currentLevel.config.subLevels[currentLevelDepth + 1], currentLevelDepth + 1);

            GenerateNavMesh();
        }
    }

    private GameObject CreateEmptyGameobject(string _name, Transform _parent)
    {
        GameObject newGameObject = new GameObject(_name);
        newGameObject.transform.SetParent(_parent);
        return newGameObject;
    }

    private void GenerateNavMesh()
    {
        if (navMeshSurface != null)
        {
            navMeshSurface.RemoveData();
            navMeshSurface.BuildNavMesh();
        }
        else
        {
            Debug.LogError("NavMeshSurface not assigned! Cannot bake NavMesh.");
        }
    }

    public void ReturnToCheckpoint()
    {
        PlayerManager.Instance.playerStates.CleanDeathCoroutine();
        LevelManager.Instance.checkpointSystem.RestoreToLastCheckpoint();
        UIManager.Instance.HideGameOver();
    }

    public void ReturnToHUB()
    {
        PlayerManager.Instance.playerStates.CleanDeathCoroutine();
        LevelManager.Instance.checkpointSystem.RestoreToHUB();
        UIManager.Instance.HideGameOver();
    }
}
