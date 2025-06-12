using PrimeTween;
using TMPro;
using Unity.Cinemachine;
using UnityEngine;

public class ResourceBlock : Block
{
    [Header("DATA")]
    public ResourceData resourceData;

    [Header("STATS")]
    public bool isDoor;
    public bool isMined;
    public int bounceCount;

    [Header("APARIENCIA")]
    public Transform blockMeshParent;
    public GameObject blockMesh;
    public ParticleSystem minedParticles;
    CinemachineImpulseSource impulseSource;
    public Material groundMaterial;

    [Header("PREFABS")]
    public GameObject doorTriggerPrefab;
    public GameObject hitIndicatorPF;
    public TextMeshProUGUI remianingBouncesText;
    public GameObject resourceDropPrefab;

    [Header("UI AND VFX")]
    public ResourceBlockUIAnims uiAnims;

    [Header("SFX")]
    public AudioClip hitSound;
    public AudioClip headbuttSound;
    private AudioSource audioSource;

    private void Start()
    {
        impulseSource = GetComponent<CinemachineImpulseSource>();
        doorTriggerPrefab.SetActive(false);
        audioSource = GetComponent<AudioSource>();
    }

    public void SetupBlock(int _subId, int _xPos, int _yPos, ResourceData _resource)
    {
        //Debug.Log(_resource);
        sublevelId = _subId;
        sublevelPosition= new Vector2(_xPos, _yPos);
        resourceData = _resource;
        isWalkable= true;
        InstanceResourceBlockMesh();
        minedParticles.GetComponent<ParticleSystemRenderer>().material = blockMesh.transform.GetChild(0).GetComponent<MeshRenderer>().material;
        ShowHitIndicator(false);
        uiAnims.resourceIcon.sprite = _resource.icon;
    }

    private void InstanceResourceBlockMesh()
    {
        blockMesh = Instantiate(resourceData.mesh, blockMeshParent);
    }

    public override void Bounce()
    {

        if (!isMined) //ESTA VIRGEN
        {
            AnimateBounced();
            //LO MARCAMOS
            ShowHitIndicator(true);

            //SI ES UN RESOURCE DIFERENTE AL ANTERIOR, RESETEAMOS EL COMBO
            MatchManager.Instance.ResourceBounced(this);

            // Play hit sound - unmined
            if (hitSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(hitSound, 0.7f);
            }
        }
        else //SI YA ESTABA MINADO
        {
            MatchManager.Instance.FloorBounced();
        }


    }

    public override void Headbutt()
    {
        if (headbuttSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(headbuttSound, 0.7f);
        }

        Bounce();
    }

    public override void Activate()
    {

        //XPManager.Instance.AddXP(resourceData.hardness);

        // Spawn the correct resource prefab (linked in ResourceData)
        if (resourceData != null && resourceData.resourceDropPrefab != null)
        {
            Instantiate(resourceData.resourceDropPrefab, transform.position, Quaternion.identity);
        }

        GetMinedState();
        ScreenShake();
        MinedAnimation();

        uiAnims.AnimateResourceRewards(MatchManager.Instance.currentStreak-1);
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
        //Debug.Log("MINED");
        isMined = true;
        resourceData = null;
        ShowHitIndicator(false);
        blockMesh.transform.GetChild(0).GetComponent<MeshRenderer>().material = groundMaterial;
        AnimateMined();
        //TRANSFORM, LUEGO DEBE SER ANIMADO
        blockMeshParent.localScale = new Vector3(blockMeshParent.localScale.x, .2f, blockMeshParent.localScale.z);
        blockMeshParent.position = new Vector3(blockMeshParent.position.x, blockMeshParent.position.y - .5f, blockMeshParent.position.z);
    }

    public void ShowHitIndicator(bool _visible)
    {
        hitIndicatorPF.SetActive(_visible);
    }


    void AnimateBounced()
    {
        Tween.Scale(blockMesh.transform, startValue: new Vector3(1.2f, .8f, 1.2f), endValue: new Vector3(1, 1, 1), duration: .5f, ease: Ease.OutBack);
    }

    public void AnimateFailed()
    {
        StartCoroutine(uiAnims.FlashBlock(blockMesh.transform.GetChild(0).GetComponent<MeshRenderer>().material, Color.red)); // o Color.red
    }
    public void AnimateMined()
    {
        StartCoroutine(uiAnims.FlashBlock(blockMesh.transform.GetChild(0).GetComponent<MeshRenderer>().material, Color.white)); // o Color.red
    }

}
