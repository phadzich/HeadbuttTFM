using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Audio;

public class FloorBlock : Block
{


    public Transform blockMeshParent;
    public GameObject blockMesh;

    [Header("SFX")]
    public AudioClip headbuttSound;
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        
    }

    public void SetupBlock(int _subId, int _xPos, int _yPos)
    {
        sublevelPosition = new Vector2(_xPos, _yPos);
        sublevelId = _subId;
        isWalkable = true;
        int _randomRotation = Random.Range(0, 4);
        transform.Rotate(0, (float)_randomRotation * 90, 0);
    }

    public override void OnBounced(HelmetInstance _helmetInstance)
    {
        MatchManager.Instance.FloorBounced();
    }

    public override void OnHeadbutted(HelmetInstance _helmetInstance)
    {
        MatchManager.Instance.FloorBounced();
        audioSource.PlayOneShot(headbuttSound, 0.7f);
    }

    public override void Activate()
    {

    }

}
