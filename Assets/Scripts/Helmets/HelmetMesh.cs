using UnityEngine;

public class HelmetMesh : MonoBehaviour
{
    public Transform helmetAnchor; // Un GameObject vacío donde colocarás el casco
    private GameObject currentHelmetInstance;

    public void SetHelmetMesh(GameObject helmetPrefab)
    {
        if (currentHelmetInstance != null)
        {
            Destroy(currentHelmetInstance);
        }

        currentHelmetInstance = Instantiate(helmetPrefab, helmetAnchor);
    }
}
