using JetBrains.Annotations;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.Image;

[CreateAssetMenu(fileName = "CheckpointSaveData", menuName = "GameData/CheckpointData")]
public class CheckpointSavedData : ScriptableObject
{
    [SerializeField] public DateTime lastSaved;

    [SerializeField] public Sublevel sublevelInstance;
    [SerializeField] public int lastDepth;
    [SerializeField] public Vector3 playerMovementPosition;
    [SerializeField] public Vector3 playerBouncePosition;
    [SerializeField] public List<HelmetInstance> helmetInstances;
    [SerializeField] public Dictionary<ResourceData, int> currentResources;


    public void SaveNewData(Sublevel _sublevelInfo)
    {
        CombatLogHUD.Instance.AddLog(UIManager.Instance.iconsLibrary.savedGame, "GAME SAVED!");
        UpdatePlayerSpawnPosition();
        UpdateSavedSublevel();
        UpdateSavedResources();
        UpdateSaveTimestamp();
    }

    private void UpdatePlayerSpawnPosition()
    {
        Vector3 dropBlockPos = LevelManager.Instance.currentDropBlock.gameObject.transform.position;

        playerMovementPosition = dropBlockPos;
        playerBouncePosition = new Vector3(dropBlockPos.x, dropBlockPos.y + 20, dropBlockPos.z);
    }
    private void UpdateSavedSublevel()
    {
        sublevelInstance = LevelManager.Instance.currentSublevel;
        lastDepth = sublevelInstance.depth;
    }
    private void UpdateSaveTimestamp()
    {
        lastSaved = System.DateTime.Now;
        Debug.Log("SavedCheckpint at " + lastSaved);
    }

    private void UpdateSavedResources()
    {
        currentResources = new Dictionary<ResourceData, int>(ResourceManager.Instance.ownedResources);
    }
}
