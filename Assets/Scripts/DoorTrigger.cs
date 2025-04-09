using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        //CUANDO EL PLAYER TOCA EL TRIGGER
        if (other.CompareTag("Player"))
        {

            //AVISAMOS QUE SALIMOS DEL LEVEL ACTUAL
            LevelManager.Instance.ExitSublevel();

            //DESACTIVAMOS EL TRIGGER PARA QUE NO SE EJECUTE MAS DE UNA VEZ
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
