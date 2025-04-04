using Unity.Cinemachine;
using UnityEngine;

public class ResourceBlock : Block
{
    public ResourceData resourceData;

    public GameObject hitIndicatorPF;
    public bool isDoor;
    public bool isMined;
    public int bounceCount;
    public Transform blockMeshParent;
    public GameObject blockMesh;
    public ParticleSystem minedParticles;
    CinemachineImpulseSource impulseSource;
    public Material groundMaterial;

    private void Start()
    {

        impulseSource = GetComponent<CinemachineImpulseSource>();
    }

    public void SetupBlock(int _subId, int _xPos, int _yPos, ResourceData _resource)
    {
        sublevelId = _subId;
        sublevelPosition= new Vector2(_xPos, _yPos);
        resourceData = _resource;
        InstanceResourceBlockMesh();
        ShowHitIndicator(false);
    }

    private void InstanceResourceBlockMesh()
    {
        blockMesh = Instantiate(resourceData.mesh, blockMeshParent);
    }

    public override void Bounce()
    {
        if (!isMined) //ESTA VIRGEN
        {
            //LO MARCAMOS
            ShowHitIndicator(true);

            //SI ES UN RESOURCE DIFERENTE AL ANTERIOR, RESETEAMOS EL COMBO
            GameManager.Instance.CheckIfNewCombo(resourceData, this);

            //SI SE CUMPLE EL COMBO
            GameManager.Instance.CheckIfComboCompleted();
        }
        else //SI YA ESTABA MINADO
        {
            GameManager.Instance.IncreaseLevelJumpCount(1);
        }

    }

    public override void Headbutt()
    {
        Bounce();
    }

    public override void Activate()
    {
        AddMinedResources();
        if (isDoor)
        {
            GetOpenedState();
        }
        else
        {
            GetMinedState();
        }

        ScreenShake();
        MinedAnimation();

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
        resourceData = null;
        ShowHitIndicator(false);
        blockMesh.GetComponent<MeshRenderer>().material = groundMaterial;

        //TRANSFORM, LUEGO DEBE SER ANIMADO
        blockMeshParent.localScale = new Vector3(blockMeshParent.localScale.x, .2f, blockMeshParent.localScale.z);
        blockMeshParent.position = new Vector3(blockMeshParent.position.x, blockMeshParent.position.y - .5f, blockMeshParent.position.z);
    }
    private void GetOpenedState()
    {
        GetMinedState();
        this.gameObject.SetActive(false);
    }

    private void AddMinedResources()
    {
        ResourceManager.Instance.AddResource(resourceData, 1);
    }

    public void ShowHitIndicator(bool _visible)
    {
        hitIndicatorPF.SetActive(_visible);
    }

}
