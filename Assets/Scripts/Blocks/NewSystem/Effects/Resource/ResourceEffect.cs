using PrimeTween;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.Cinemachine;
using UnityEditor.Localization.Plugins.XLIFF.V12;
using UnityEngine;

[RequireComponent(typeof(ResourceSetup))]
public class ResourceEffect : MonoBehaviour, IBlockEffect
{

    [Header("DATA")]
    public ResourceData resourceData;

    [Header("STATS")]
    public bool isDoor;
    public bool isMined;
    public bool isSelected;

    [Header("COMPATIBILIDAD")]
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


    [Header("UI AND VFX")]
    public ResourceDropFollow resourceDropPrefab;
    public HeadbuttDropFollow hbDropPrefab;
    public ResourceBlockUIAnims uiAnims;

    private void Start()
    {
        impulseSource = GetComponent<CinemachineImpulseSource>();
    }

    public void SetupBlock(MapContext _context, ResourceData _resource)
    {
        resourceData = _resource;

        InstanceResourceBlockMesh();
        InstanceResourceDropMesh();
        ToggleHitIndicator(false);
        minedParticles.GetComponent<ParticleSystemRenderer>().material = blockMesh.transform.GetChild(0).GetComponent<MeshRenderer>().material;
        uiAnims.resourceIcon.sprite = _resource.icon;
        SetRandomRotation();
        gameObject.name = $"{_resource.shortName}_c{_context.x}r_{_context.y}";

        //Debug.Log(_context.sublevel);
    }

    private void InstanceResourceBlockMesh()
    {
        blockMesh = Instantiate(resourceData.blockMesh, blockMeshParent);
        resourceContainer = blockMesh.transform.GetChild(1).gameObject;
    }

    private int HelmetPowerMultiplier(HelmetPower helmetPower)
    {
        int multiplier = (int)helmetPower + 1;
        return multiplier;
    }

    private void SetRandomRotation()
    {
        int _randomRotation = Random.Range(0, 4);
        transform.Rotate(0, (float)_randomRotation * 90, 0);
    }

    public void OnBounced(HelmetInstance _helmetInstance)
    {
        if (!isMined) //SI NO HA SIDO MINADO AUN
        {
            helmetPowerMultiplier = HelmetPowerMultiplier(_helmetInstance.baseHelmet.miningPower);
            BouncedOnResource();
            SoundManager.PlaySound(SFXType.RESOURCEBOUNCE, 0.7f);
            MatchManager.Instance.TryToAddToChain();
        }
        else //YA HA SIDO MINADO, ACTUA COMO PISO
        {
            BouncedOnFloor();
        }
    }

    public void OnHeadbutt(HelmetInstance _helmetInstance)
    {
        SoundManager.PlaySound(SFXType.HEADBUTT, 0.7f);
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

    public void Activate()
    {
        GetMinedState();
        ScreenShake();
        MinedAnimation();
        ReleaseResourceDrop();
        ReleaseHBDrop();
        //SoundManager.PlaySound(SoundType.MINEDCOMPLETE, 0.7f);
        SoundManager.PlaySound(SFXType.MINEDCOMPLETE, 0.7f);

        uiAnims.AnimateResourceRewards(helmetPowerMultiplier);
    }

    private void MinedAnimation()
    {
        minedParticles.Play();
    }

    private void InstanceResourceDropMesh()
    {
        resourceDropPrefab.ConfigDrop(resourceData.resMesh);
    }
    private void ReleaseResourceDrop()
    {
        resourceDropPrefab.gameObject.SetActive(true);
    }

    private void ReleaseHBDrop()
    {
        hbDropPrefab.StartFollow();
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
        blockMeshParent.localScale = new Vector3(blockMeshParent.localScale.x, .9f, blockMeshParent.localScale.z);
        blockMeshParent.position = new Vector3(blockMeshParent.position.x, blockMeshParent.position.y - .1f, blockMeshParent.position.z);
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
