using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("DOOR TRIGGER");
            //SceneManager.LoadScene("SampleScene");
            LevelManager.Instance.ExitSublevel();
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
