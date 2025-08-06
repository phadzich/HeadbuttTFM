using UnityEngine;

public class ResourceDropFollow : MonoBehaviour
{

    public Transform target;
    public float MinModifier;
    public float MaxModifier;
    public Transform meshContainer;
    private float smoothTime = .2f;

    Vector3 _velocity = Vector3.zero;
    public bool _isFollowing = false;

    void Start()
    {
        target = PlayerManager.Instance.playerMovement.enanoParent;
    }

    public void ConfigDrop(GameObject _resMesh)
    {
        Instantiate(_resMesh, meshContainer);
    }

    public void StartFollowing()
        {
        smoothTime = Random.Range(.1f, .3f);
        _isFollowing = true;
        }

    void Update()
    {
        if (_isFollowing)
        {
            this.gameObject.transform.position = Vector3.SmoothDamp(transform.position, target.position, ref _velocity, smoothTime * Random.Range(MinModifier, MaxModifier));
        }
    }

}
