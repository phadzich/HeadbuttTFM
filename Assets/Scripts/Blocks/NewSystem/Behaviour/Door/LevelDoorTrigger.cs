using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelDoorTrigger : MonoBehaviour
{
    public LevelDoorBehaviour doorbehaviour;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log($"LOADING LEVEL {doorbehaviour.targetLevelIndex}");
            LevelManager.Instance.ChangeLevel(doorbehaviour.targetLevelIndex);
            DisableTrigger();
            Invoke("EnableTrigger", 4f);
        }
    }

    private void DisableTrigger()
    {
        this.GetComponent<Collider>().enabled = false;
    }

    private void EnableTrigger()
    {
        this.GetComponent<Collider>().enabled = true;
    }
}
