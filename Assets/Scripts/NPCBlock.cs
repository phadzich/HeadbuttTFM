using Unity.Cinemachine;
using UnityEngine;

public class NPCBlock : Block
{

    public GameObject hitIndicatorPF;
    public bool isDoor;
    public bool isMined;
    public int bounceCount;
    public Transform blockMeshParent;
    public GameObject blockMesh;
    public GameObject doorMesh;
    public ParticleSystem minedParticles;
    CinemachineImpulseSource impulseSource;
    public Material groundMaterial;
    public GameObject doorTriggerPrefab;

    private void Start()
    {

        impulseSource = GetComponent<CinemachineImpulseSource>();
        doorTriggerPrefab.SetActive(false);
    }

    public void SetupBlock(int _subId, int _xPos, int _yPos)
    {
        sublevelId = _subId;
        sublevelPosition= new Vector2(_xPos, _yPos);
        if (isDoor)
        {
            doorMesh.SetActive(true);
        }
        //InstanceResourceBlockMesh();
    }

    /*
    private void InstanceResourceBlockMesh()
    {
        blockMesh = Instantiate(resourceData.mesh, blockMeshParent);
    }
    */

    public override void Bounce()
    {
        if (!isMined) //ESTA VIRGEN
        {
            //LO MARCAMOS
            //ShowHitIndicator(true);

            //SI ES UN RESOURCE DIFERENTE AL ANTERIOR, RESETEAMOS EL COMBO
            //GameManager.Instance.CheckIfNewCombo(resourceData, this);

            //SI SE CUMPLE EL COMBO
            //GameManager.Instance.CheckIfComboCompleted();
        }
        else //SI YA ESTABA MINADO
        {
            //GameManager.Instance.IncreaseLevelJumpCount(1);
        }

    }

    public override void Headbutt()
    {
        GameManager.Instance.ClearAllHitBlocks();
        Activate();
    }

    public override void Activate()
    {
        //AddMinedResources();
        if (isDoor)
        {
            GetOpenedState();
            ScreenShake();
        }
        else
        {
            //GetMinedState();
        }

        
        //MinedAnimation();

    }

    private void MinedAnimation()
    {
        minedParticles.Play();
    }

    private void ScreenShake()
    {
        impulseSource.GenerateImpulse();
    }

    private void GetMinedState()
    {
        isMined = true;
        blockMesh.GetComponent<MeshRenderer>().material = groundMaterial;

        //TRANSFORM, LUEGO DEBE SER ANIMADO
        blockMeshParent.localScale = new Vector3(blockMeshParent.localScale.x, .2f, blockMeshParent.localScale.z);
        blockMeshParent.position = new Vector3(blockMeshParent.position.x, blockMeshParent.position.y - .5f, blockMeshParent.position.z);
    }
    private void GetOpenedState()
    {
        //GetMinedState();
        blockMesh.SetActive(false);
        doorMesh.SetActive(false);
        this.GetComponent<BoxCollider>().enabled = false;
        this.GetComponent<NPCBlock>().enabled = false;
        if (isDoor)
        {
            doorTriggerPrefab.SetActive(true);
        }
    }


}
