using UnityEngine;

public class PlayerStates : MonoBehaviour
{

    public LevelManager level;
    public PlayerBounce bouncing;
    public Material idleMaterial;
    public Material headbuttMaterial;
    public MeshRenderer bodyMeshRenderer;
    public bool NPCLevel = false;
 
    private void Update()
    {
        NPCLevel = level.NPCLevel;
        if (NPCLevel)
        {
            bouncing.enabled = false;
        } else
        {
            bouncing.enabled = true;
        }
    }   
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
