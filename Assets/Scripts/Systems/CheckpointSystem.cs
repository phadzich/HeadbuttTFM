using Unity.VisualScripting;
using UnityEngine;

public class CheckpointSystem : MonoBehaviour
{
    public CheckpointSavedData checkpointSaveData;
    public NPCSublevelConfig lastNPCSublevel;


    public void EnterNPCSublevel(NPCSublevelConfig _NPCEntered, Sublevel _sublevelInfo)
    {
        //SI EL SUBLEVEL ES EL PRIMERO O ES UNO DIFERENTE
        if (lastNPCSublevel == null || lastNPCSublevel!= _NPCEntered)
        {
            lastNPCSublevel = _NPCEntered;
            EnterNewNPCLevel(_sublevelInfo);
        }
        }
    public void EnterNewNPCLevel(Sublevel _sublevelInfo)
    {
        checkpointSaveData.SaveNewData(_sublevelInfo);
    }

    public void RestoreToLastCheckpoint()
    {
        //PLAYER POSITION
        PlayerManager.Instance.playerMovement.ChangePositionTarget(checkpointSaveData.playerMovementPosition);
        //PLAYER POSITION
        PlayerManager.Instance.playerMovement.enanoParent.position = checkpointSaveData.playerBouncePosition;
        PlayerManager.Instance.playerCamera.MoveFogToDepth(checkpointSaveData.lastDepth);

        //RECURSOS
        ResourceManager.Instance.ownedResources = checkpointSaveData.currentResources;
        ResourceManager.Instance.onOwnedResourcesChanged();

        //REACTIVAMOS CHECKPOINT SUBLEVEL
        LevelManager.Instance.GenerateSublevel(LevelManager.Instance.sublevelsList[checkpointSaveData.lastDepth].config, checkpointSaveData.lastDepth);

        //ELIMINAMOS TODO LO DE ABAJO
        LevelManager.Instance.DestroySublevelsUntilCheckpoint(checkpointSaveData.lastDepth);

        //AL FINAL, HACEMOS QUE ENTRE AL SUBLEVEL D NUEVO
        LevelManager.Instance.currentLevelDepth = checkpointSaveData.lastDepth;

        LevelManager.Instance.EnterSublevel(LevelManager.Instance.sublevelsList[checkpointSaveData.lastDepth].config);
    }

    public void RestoreToHUB()
    {
        //RECURSOS
        ResourceManager.Instance.ownedResources = checkpointSaveData.currentResources;
        ResourceManager.Instance.onOwnedResourcesChanged();

        LevelManager.Instance.ChangeLevel(1);
    }
}
