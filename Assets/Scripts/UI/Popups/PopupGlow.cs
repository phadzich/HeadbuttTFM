using UnityEngine;

public class PopupGlow : MonoBehaviour
{
    private void Update()
    {
        transform.Rotate(0, 0, 100 * Time.deltaTime);
    }
}
