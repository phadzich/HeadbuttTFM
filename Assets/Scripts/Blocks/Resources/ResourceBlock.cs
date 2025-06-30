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
    public ParticleSystem minedParticles;
    CinemachineImpulseSource impulseSource;
    public Material groundMaterial;

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
        ShowHit1Indicator(false);
        ShowHit2Indicator(false);
        ShowHit3Indicator(false);
        requiredHits = 3;
        uiAnims.resourceIcon.sprite = _resource.icon;
    }

    private void InstanceResourceBlockMesh()
    {
        blockMesh = Instantiate(resourceData.mesh, blockMeshParent);
    }

    private int CalculateHelmetDamage(List<ResourceFamily> _helmetFamilies)
    {
        foreach (var _helmetFamily in _helmetFamilies)
        {
            foreach (var _resFamily in resourceFamilies)
            {
                if (_helmetFamily == _resFamily) //ES DEL MISMO RESOURCE FAMILY
                {
                    return 3;
                }
                else if (IsFamilyNeighbour(_helmetFamily, _resFamily)) //DIFERENTE FAMILIA, PERO NO LEJANA
                {
                    return 2;
                }
            }
        }
        return 1;
    }

    bool IsFamilyNeighbour(ResourceFamily _famA, ResourceFamily _famB)
    {
        return(_famA == ResourceFamily.EARTH && _famB==ResourceFamily.METAL)|| //earth + metal = cercanos
            (_famA == ResourceFamily.METAL && (_famB == ResourceFamily.EARTH || _famB == ResourceFamily.GEM)) || // metal + cualquiera = cercanos
           (_famA == ResourceFamily.GEM && _famB == ResourceFamily.METAL); // gem + metal = cercanos
    }

    public override void OnBounced(HelmetInstance _helmetInstance)
    {
        if (!isMined) //SI NO HA SIDO MINADO AUN
        {

            int _helmetDamage = CalculateHelmetDamage(_helmetInstance.baseHelmet.resourceFamilies);
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

        /*
        if (!isSelected)
        {
            if (!isMined) //ESTA VIRGEN
            {
                Debug.Log($"BOUNCED OVER {resourceData.compatibleWithElement} with {HelmetManager.Instance.currentHelmet.baseHelmet.element.family}");
                //SI ES DIRECTAMENTE COMPATIBLE CON EL HELMET
                if (isCompatibleWithElement())
                {
                    SendBlockToMatchManager();
                    // Play hit sound - unmined
                    if (hitSound != null && audioSource != null)
                    {
                        audioSource.PlayOneShot(hitSound, 0.7f);
                    }
                }
                //NO ES DIRECTAMENTE COMPATIBLE, SE COMPORTA COMO PISO
                else
                {
                    DontSendBlockToMatchManager();
                }
            }
            else //SI YA ESTABA MINADO
            {
                DontSendBlockToMatchManager();
            }
        }
        */

    }

    public override void OnHeadbutted(HelmetInstance _helmetInstance)
    {
        /*
        if (!isSelected)
        {
            if (!isMined) //ESTA VIRGEN
            {
                Debug.Log($"HB OVER {resourceData.compatibleWithElement} with {HelmetManager.Instance.currentHelmet.baseHelmet.element.family}");
                //SI NO ES IMMUNE
                if (!isImmuneToElement())
                {
                    Debug.Log("HB sent");
                    BouncedOnResource();
                    // Play hit sound - unmined
                    if (headbuttSound != null && audioSource != null)
                    {
                        audioSource.PlayOneShot(headbuttSound, 0.7f);
                    }
                }
                //ES IMMUNE
                else
                {
                    Debug.Log("HB IMMUNE");
                    BouncedOnFloor();
                }
            }
            else //SI YA ESTABA MINADO
            {
                Debug.Log("HB MINADO");
                BouncedOnFloor();
            }
        }
        */
            
    }

    private void BouncedOnResource()
    {
        //PROCEDEMOS CON LA SECUENCIA DE MATCH
        AnimateBounced();
        
        if (requiredHits == 2)
        {
            ShowHit1Indicator(true);
            ShowHit2Indicator(false);
            ShowHit3Indicator(false);
        }
        else if (requiredHits == 1)
        {
            ShowHit2Indicator(true);
            ShowHit1Indicator(true);
            ShowHit3Indicator(false);
        }
        else if (requiredHits <= 0)
        {
            ShowHit2Indicator(true);
            ShowHit1Indicator(true);
            ShowHit3Indicator(true);
        }


        //SI ES UN RESOURCE DIFERENTE AL ANTERIOR, RESETEAMOS EL COMBO
        MatchManager.Instance.ResourceBounced(this);
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
        ShowHit1Indicator(false);
        ShowHit2Indicator(false);
        ShowHit3Indicator(false);
        blockMesh.transform.GetChild(0).GetComponent<MeshRenderer>().material = groundMaterial;
        AnimateMined();
        //TRANSFORM, LUEGO DEBE SER ANIMADO
        blockMeshParent.localScale = new Vector3(blockMeshParent.localScale.x, .2f, blockMeshParent.localScale.z);
        blockMeshParent.position = new Vector3(blockMeshParent.position.x, blockMeshParent.position.y - .5f, blockMeshParent.position.z);
    }

    public void ShowHit1Indicator(bool _visible)
    {
        hit1IndicatorPF.SetActive(_visible);
    }

    public void ShowHit2Indicator(bool _visible)
    {
        hit2IndicatorPF.SetActive(_visible);
    }

    public void ShowHit3Indicator(bool _visible)
    {
        hit3IndicatorPF.SetActive(_visible);
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
