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

    public int requiredHits = 3;

    [Header("APARIENCIA")]
    public Transform blockMeshParent;
    public GameObject blockMesh;
    public GameObject resourceContainer;
    public ParticleSystem minedParticles;
    CinemachineImpulseSource impulseSource;
    public Material groundMaterial;
    public Material dmgMaterial;
    public Material bouncedMaterial;
    public Material selectedMaterial;

    [Header("PREFABS")]
    public GameObject hit1IndicatorPF;
    public GameObject hit2IndicatorPF;
    public GameObject hit3IndicatorPF;
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
        minedParticles.GetComponent<ParticleSystemRenderer>().material = blockMesh.transform.GetChild(0).GetComponent<MeshRenderer>().material;
        VisualHit0();
        requiredHits = 3;
        uiAnims.resourceIcon.sprite = _resource.icon;
    }

    private void InstanceResourceBlockMesh()
    {
        blockMesh = Instantiate(resourceData.mesh, blockMeshParent);
        resourceContainer = blockMesh.transform.GetChild(1).gameObject;
    }

    private int CalculateHelmetDamage(MiningPower helmetPower, MiningPower resourceRequiredPower)
    {
        int helmetLevel = (int)helmetPower;
        int resourceLevel = (int)resourceRequiredPower;

        if (helmetLevel >= resourceLevel)
        {
            return 3; // Perfecto match o superior
        }
        else if (helmetLevel == resourceLevel - 1)
        {
            return 2; // Está solo un nivel abajo
        }
        else
        {
            return 1; // Muy inferior
        }
    }


    public override void OnBounced(HelmetInstance _helmetInstance)
    {
        if (!isMined) //SI NO HA SIDO MINADO AUN
        {

            int _helmetDamage = CalculateHelmetDamage(_helmetInstance.baseHelmet.miningPower, resourceData.requiredMiningPower);
            requiredHits -= _helmetDamage;
            BouncedOnResource();
            if (requiredHits <= 0) //SOLO SI SE CUMPLEN LOS HITS, INTENTAMOS AGREGALO AL CHAIN
            {
                MatchManager.Instance.TryToAddToChain();
            }
            else //COMUNICAMOS EL PROGRESO INCOMPLETO
            {
                //Debug.Log($"Needs {requiredHits} hits");
            }
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
            requiredHits -= 3;
            BouncedOnResource();
            if (requiredHits <= 0) //SOLO SI SE CUMPLEN LOS HITS, INTENTAMOS AGREGALO AL CHAIN
            {
                MatchManager.Instance.TryToAddToChain();
            }
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
        
        if (requiredHits == 2)
        {
            audioSource.PlayOneShot(hitSound, 0.3f);
            VisualHit1();
        }
        else if (requiredHits == 1)
        {
            audioSource.PlayOneShot(hitSound, 0.5f);
            VisualHit2();
        }
        else if (requiredHits <= 0)
        {
            audioSource.PlayOneShot(hitSound, 0.7f);
            VisualHit3();

        }
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

        //XPManager.Instance.AddXP(resourceData.hardness);

        // Spawn the correct resource prefab (linked in ResourceData)
        if (resourceData != null && resourceData.resourceDropPrefab != null)
        {
            Instantiate(resourceData.resourceDropPrefab, transform.position, Quaternion.identity);
        }

        GetMinedState();
        ScreenShake();
        MinedAnimation();
        audioSource.PlayOneShot(minedSound);

        uiAnims.AnimateResourceRewards(1);
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
        VisualHit0();
        blockMesh.transform.GetChild(1).gameObject.SetActive(false);
        blockMesh.transform.GetChild(0).GetComponent<MeshRenderer>().material = groundMaterial;
        AnimateMined();
        //TRANSFORM, LUEGO DEBE SER ANIMADO
        blockMeshParent.localScale = new Vector3(blockMeshParent.localScale.x, .2f, blockMeshParent.localScale.z);
        blockMeshParent.position = new Vector3(blockMeshParent.position.x, blockMeshParent.position.y - .5f, blockMeshParent.position.z);
    }

    public void VisualHit0()
    {
        MoveResourceToken(0f);
        hit1IndicatorPF.SetActive(false);
        hit1IndicatorPF.GetComponent<MeshRenderer>().material = groundMaterial;
        hit2IndicatorPF.SetActive(false);
        hit2IndicatorPF.GetComponent<MeshRenderer>().material = groundMaterial;
        hit3IndicatorPF.SetActive(false);
        hit3IndicatorPF.GetComponent<MeshRenderer>().material = groundMaterial;
    }

    public void VisualHit1()
    {
        MoveResourceToken(.1f);
        hit1IndicatorPF.SetActive(true);
        hit1IndicatorPF.GetComponent<MeshRenderer>().material = groundMaterial;
        hit2IndicatorPF.SetActive(true);
        hit2IndicatorPF.GetComponent<MeshRenderer>().material = groundMaterial;
        hit3IndicatorPF.SetActive(true);
        hit3IndicatorPF.GetComponent<MeshRenderer>().material = dmgMaterial;
    }

    public void VisualHit2()
    {
        MoveResourceToken(.2f);
        hit1IndicatorPF.SetActive(true);
        hit1IndicatorPF.GetComponent<MeshRenderer>().material = groundMaterial;
        hit2IndicatorPF.SetActive(true);
        hit2IndicatorPF.GetComponent<MeshRenderer>().material = dmgMaterial;
        hit3IndicatorPF.SetActive(true);
        hit3IndicatorPF.GetComponent<MeshRenderer>().material = dmgMaterial;
    }

    public void VisualHit3()
    {
        MoveResourceToken(.4f);
        hit1IndicatorPF.SetActive(true);
        hit1IndicatorPF.GetComponent<MeshRenderer>().material = dmgMaterial;
        hit2IndicatorPF.SetActive(true);
        hit2IndicatorPF.GetComponent<MeshRenderer>().material = dmgMaterial;
        hit3IndicatorPF.SetActive(true);
        hit3IndicatorPF.GetComponent<MeshRenderer>().material = dmgMaterial;
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
