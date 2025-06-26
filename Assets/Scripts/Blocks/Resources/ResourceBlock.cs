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
    public bool isSelected;
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
    public AudioClip minedSound;
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
        
    }

    public override void Headbutt()
    {
        if (!isSelected)
        {
            if (!isMined) //ESTA VIRGEN
            {
                Debug.Log($"HB OVER {resourceData.compatibleWithElement} with {HelmetManager.Instance.currentHelmet.baseHelmet.element.family}");
                //SI NO ES IMMUNE
                if (!isImmuneToElement())
                {
                    Debug.Log("HB sent");
                    SendBlockToMatchManager();
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
                    DontSendBlockToMatchManager();
                }
            }
            else //SI YA ESTABA MINADO
            {
                Debug.Log("HB MINADO");
                DontSendBlockToMatchManager();
            }
        }
            
    }

    private void SendBlockToMatchManager()
    {
        //PROCEDEMOS CON LA SECUENCIA DE MATCH
        AnimateBounced();
        ShowHitIndicator(true);

        //SI ES UN RESOURCE DIFERENTE AL ANTERIOR, RESETEAMOS EL COMBO
        MatchManager.Instance.ResourceBounced(this);


    }

    private void DontSendBlockToMatchManager()
    {
        MatchManager.Instance.FloorBounced();
    }

    private bool isCompatibleWithElement()
    {

        if (HelmetManager.Instance.currentHelmet.baseHelmet.element.family == resourceData.compatibleWithElement
            || resourceData.compatibleWithElement == ElementFamily.NONE)
        {
            Debug.Log("COMPATIBLE");
            return true;

        }
        else
        {
            return false;
        }
    }

    private bool isImmuneToElement()
    {
        Debug.Log(resourceData.immuneToElement);
        Debug.Log(HelmetManager.Instance.currentHelmet.baseHelmet.element.family);
        if (HelmetManager.Instance.currentHelmet.baseHelmet.element.family == resourceData.immuneToElement)
        {
            Debug.Log("IMMUNE");

            return true;

        }
        else
        {
            Debug.Log("NOT IMMUNE");
            return false;

        }
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
