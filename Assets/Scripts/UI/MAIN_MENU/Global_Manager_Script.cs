using UnityEngine;

public class PersistentManagers : MonoBehaviour
{
    private static PersistentManagers instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
