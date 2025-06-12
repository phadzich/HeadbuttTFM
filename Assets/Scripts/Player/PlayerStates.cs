using UnityEngine;

public class PlayerStates : MonoBehaviour
{

    public LevelManager level;
    public Material idleMaterial;
    public Material headbuttMaterial;
    public MeshRenderer bodyMeshRenderer;
    public PlayerMovement playerMovement;
 

    public void EnterIdleState()
    {
        bodyMeshRenderer.material = idleMaterial;
    }
    public void EnterHeadbuttState()
    {
        bodyMeshRenderer.material = headbuttMaterial;
    }

}
