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
    }

    public override void Bounce()
    {
        MatchManager.Instance.FloorBounced();
    }

    public override void Headbutt()
    {
        MatchManager.Instance.FloorBounced();
        audioSource.PlayOneShot(headbuttSound, 0.7f);
    }

    public override void Activate()
    {

    }

}
