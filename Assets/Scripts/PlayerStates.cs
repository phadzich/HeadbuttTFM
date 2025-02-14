using UnityEngine;

public class PlayerStates : MonoBehaviour
{


    public Material idleMaterial;
    public Material headbuttMaterial;
    public MeshRenderer bodyMeshRenderer;
    private void Awake()
    {

    }
    public void EnterIdleState()
    {
        bodyMeshRenderer.material = idleMaterial;
    }
    public void EnterHeadbuttState()
    {
        bodyMeshRenderer.material = headbuttMaterial;
    }

}
