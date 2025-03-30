using Unity.Cinemachine;
using UnityEngine;
using static Unity.Collections.AllocatorManager;

public class BlockData : MonoBehaviour
{
    [SerializeField]
    public int jumpCount;

    public GameObject hitIndicatorPF;
    public ResourceData blockResource;
    public bool isDoor = false;
    public bool isMined = false;
    public GameObject blockGeo;
    public ParticleSystem minedParticles;
    CinemachineImpulseSource impulseSource;
    public Material groundMaterial;

    private void Start()
    {
        ShowHitIndicator(false);
        impulseSource = GetComponent<CinemachineImpulseSource>();
    }

    private void ScreenShake()
    {
        impulseSource.GenerateImpulse();
    }

    public void GetHit()
    {
        if (!isMined)
        {
            ShowHitIndicator(true);

            if (GameManager.instance.currentComboBlock != blockResource)
            {
                GameManager.instance.ClearAllHitBlocks();
                GameManager.instance.currentComboBlock = blockResource;
            }

            GameManager.instance.AddBlockToHitBlocks(this);


            if (GameManager.instance.currentComboCount == blockResource.hardness)
            {
                GameManager.instance.MineAllHitBlocks();
            }

        }
        else
        {
            GameManager.instance.IncreaseLevelJumpCount(1);
        }

    }

    public void GetMined()
    {
        if(isDoor)
        {
            this.gameObject.SetActive(false);
        }
        else
        {
            blockGeo.transform.localScale = new Vector3(blockGeo.transform.localScale.x, .2f, blockGeo.transform.localScale.z);
            blockGeo.transform.position = new Vector3(blockGeo.transform.position.x, blockGeo.transform.position.y -.5f, blockGeo.transform.position.z);
        }
        AddMinedResources();
        ScreenShake();
        minedParticles.Play();
        blockResource = null;
        isMined = true;
        ShowHitIndicator(false);
        blockGeo.GetComponent<MeshRenderer>().material = groundMaterial;
    }

    private void AddMinedResources()
    {
        ResourceManager.Instance.AddResource(blockResource, 1);
    }


    public void ShowHitIndicator(bool _visible)
    {
        hitIndicatorPF.SetActive(_visible);   
    }
}
