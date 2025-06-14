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
        UpdatePlayerSpawnPosition();
        UpdateSavedSublevel();
        UpdateSavedResources();
        UpdateSavedHelmets();

        UpdateSaveTimestamp();
    }

    private void UpdatePlayerSpawnPosition()
    {
        playerMovementPosition = PlayerManager.Instance.playerMovement.positionTarget;
        playerBouncePosition = PlayerManager.Instance.playerMovement.enanoParent.position;
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

    private void UpdateSavedHelmets()
    {
        helmetInstances = new List<HelmetInstance>();

        foreach (var _helmInstance in HelmetManager.Instance.helmetsEquipped)
        {
            helmetInstances.Add(new HelmetInstance(_helmInstance.baseHelmet)
            {
                id = _helmInstance.id,
                currentInfo = _helmInstance.currentInfo,
                baseHelmet = _helmInstance.baseHelmet,
                currentDurability = _helmInstance.currentDurability,
                remainingHeadbutts = _helmInstance.remainingHeadbutts,
                maxHeadbutts = _helmInstance.maxHeadbutts,
                durability = _helmInstance.durability,
                bounceHeight = _helmInstance.bounceHeight,
                headBForce = _helmInstance.headBForce,
                headBCooldown = _helmInstance.headBCooldown,
                knockbackChance = _helmInstance.knockbackChance,
                helmetXP = _helmInstance.helmetXP,
                helmetEffect = _helmInstance.helmetEffect,
                helmetElement = _helmInstance.helmetElement
            });
        }
    }

    private void UpdateSavedResources()
    {
        currentResources = new Dictionary<ResourceData, int>(ResourceManager.Instance.ownedResources);
    }
}
