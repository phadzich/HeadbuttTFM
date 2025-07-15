using PrimeTween;
using System.Collections.Generic;
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
    public bool isSelected;

    [Header("COMPATIBILIDAD")]
    public List<ResourceFamily> resourceFamilies;
    public int helmetPowerMultiplier;

    [Header("APARIENCIA")]
    public Transform blockMeshParent;
    public GameObject blockMesh;
    public GameObject resourceContainer;
    public ParticleSystem minedParticles;
    CinemachineImpulseSource impulseSource;
    public Material groundMaterial;

    [Header("PREFABS")]
    public GameObject hitIndicatorPF;

    public TextMeshProUGUI remianingBouncesText;
    public GameObject resourceDropPrefab;

    [Header("UI AND VFX")]
    public ResourceBlockUIAnims uiAnims;

    [Header("SFX")]
    public AudioClip hitSound;
    public AudioClip headbuttSound;
    public AudioClip minedSound;
    private AudioSource audioSource;

    private void Start()
    {
        impulseSource = GetComponent<CinemachineImpulseSource>();
        audioSource = GetComponent<AudioSource>();
    }

    public void SetupBlock(int _subId, int _xPos, int _yPos, ResourceData _resource)
    {
        //Debug.Log(_resource);
        sublevelId = _subId;
        sublevelPosition= new Vector2(_xPos, _yPos);
        resourceData = _resource;
        resourceFamilies = _resource.resourceFamilies;
        isWalkable= true;

        InstanceResourceBlockMesh();
        ToggleHitIndicator(false);
        minedParticles.GetComponent<ParticleSystemRenderer>().material = blockMesh.transform.GetChild(0).GetComponent<MeshRenderer>().material;
        uiAnims.resourceIcon.sprite = _resource.icon;
    }

    private void InstanceResourceBlockMesh()
    {
        blockMesh = Instantiate(resourceData.mesh, blockMeshParent);
        resourceContainer = blockMesh.transform.GetChild(1).gameObject;
    }

    private int HelmetPowerMultiplier(MiningPower helmetPower)
    {
        int multiplier = (int)helmetPower+1;
        return multiplier;
    }


    public override void OnBounced(HelmetInstance _helmetInstance)
    {
        if (!isMined) //SI NO HA SIDO MINADO AUN
        {
            helmetPowerMultiplier = HelmetPowerMultiplier(_helmetInstance.baseHelmet.miningPower);
            BouncedOnResource();
            MatchManager.Instance.TryToAddToChain();
        }
        else //YA HA SIDO MINADO, ACTUA COMO PISO
        {
            BouncedOnFloor();
        }
    }

    public override void OnHeadbutted(HelmetInstance _helmetInstance)
    {
        audioSource.PlayOneShot(headbuttSound);
        if (!isMined) //SI NO HA SIDO MINADO AUN
        {
            helmetPowerMultiplier = 3;
            BouncedOnResource();
            MatchManager.Instance.TryToAddToChain();
        }
        else //YA HA SIDO MINADO, ACTUA COMO PISO
        {
            BouncedOnFloor();
        }
    }

    private void BouncedOnResource()
    {
        //PROCEDEMOS CON LA SECUENCIA DE MATCH
        AnimateBounced();
        ToggleHitIndicator(true);

        //SI ES UN RESOURCE DIFERENTE AL ANTERIOR, RESETEAMOS EL COMBO
        MatchManager.Instance.ResourceBounced(this);
    }

    public void MoveResourceToken(float _height)
    {
        resourceContainer.transform.localPosition = new Vector3(resourceContainer.transform.localPosition.x, _height, resourceContainer.transform.localPosition.z);
    }
    private void BouncedOnFloor()
    {
        MatchManager.Instance.FloorBounced();
    }

    public override void Activate()
    {
        // Spawn the correct resource prefab (linked in ResourceData)
        if (resourceData != null && resourceData.resourceDropPrefab != null)
        {
            Instantiate(resourceData.resourceDropPrefab, transform.position, Quaternion.identity);
        }

        GetMinedState();
        ScreenShake();
        MinedAnimation();
        audioSource.PlayOneShot(minedSound);

        uiAnims.AnimateResourceRewards(helmetPowerMultiplier);
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
        ToggleHitIndicator(false);
        blockMesh.transform.GetChild(1).gameObject.SetActive(false);
        blockMesh.transform.GetChild(0).GetComponent<MeshRenderer>().material = groundMaterial;
        AnimateMined();
        //TRANSFORM, LUEGO DEBE SER ANIMADO
        blockMeshParent.localScale = new Vector3(blockMeshParent.localScale.x, .2f, blockMeshParent.localScale.z);
        blockMeshParent.position = new Vector3(blockMeshParent.position.x, blockMeshParent.position.y - .5f, blockMeshParent.position.z);
    }

    public void ToggleHitIndicator(bool _active)
    {
        hitIndicatorPF.SetActive(_active);
        if (_active)
        {
            MoveResourceToken(.4f);
        }
        else
        {
            MoveResourceToken(0);
        }
        
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
