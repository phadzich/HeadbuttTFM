using Unity.Cinemachine;
using UnityEngine;

public class NPCBlock : Block
{

    public GameObject hitIndicatorPF;
    public bool isDoor;
    public bool isMined;
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
            //doorMesh.SetActive(true);
            //this.GetComponent<BoxCollider>().enabled = false;
            //Activate();
            GetOpenedState();
        }
    }

    public override void Bounce()
    {
        //QUE PASA SI REBOTAMOS SOBRE UN PISO NPC

    }

    public override void Headbutt()
    {
        //QUE PASA SI HACEMOS HB SOBRE UN PISO NPC
        GameManager.Instance.ClearAllHitBlocks();
        Activate();
    }

    public override void Activate()
    {
        if (isDoor)
        {
            GetOpenedState();
            ScreenShake();
        }
    }

    private void MinedAnimation()
    {
        minedParticles.Play();
    }

    private void ScreenShake()
    {
        impulseSource.GenerateImpulse();
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
