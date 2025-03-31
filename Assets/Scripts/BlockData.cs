using Unity.Cinemachine;
using UnityEngine;
using static Unity.Collections.AllocatorManager;

public class BlockData : MonoBehaviour
{
    /*
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

            if (GameManager.Instance.currentComboBlock != blockResource)
            {
                GameManager.Instance.ClearAllHitBlocks();
                GameManager.Instance.currentComboBlock = blockResource;
            }

            GameManager.Instance.AddBlockToHitBlocks(this);


            if (GameManager.Instance.currentComboCount == blockResource.hardness)
            {
                GameManager.Instance.MineAllHitBlocks();
            }

        }
        else
        {
            GameManager.Instance.IncreaseLevelJumpCount(1);
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
    */
}
