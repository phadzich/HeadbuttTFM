using UnityEngine;

public class ResourceDropFollow : MonoBehaviour
{

    public Transform target;
    public float MinModifier;
    public float MaxModifier;
    public Transform meshContainer;

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
        Debug.Log("STARTFOLLOWING");
        _isFollowing = true;
        }

    void Update()
    {
        if (_isFollowing)
        {
            this.gameObject.transform.position = Vector3.SmoothDamp(transform.position, target.position, ref _velocity, Time.deltaTime * Random.Range(MinModifier, MaxModifier));
        }
    }

}
