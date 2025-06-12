using UnityEngine;

public class ResourceDropFollow : MonoBehaviour
{

    public Transform Target;
    public float MinModifier;
    public float MaxModifier;

    Vector3 _velocity = Vector3.zero;
    bool _isFollowing = false;

    void Start()
    {
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            Target = player.transform;
        }
        else
        {
            Debug.LogWarning("Player not found! Make sure your player GameObject has the 'Player' tag.");
        }
    }

    public void StartFollowing()
        { 
            _isFollowing = true;
        }

    void Update()
    {
        //Starts running right from the beginning...
        if (_isFollowing)
        { 
        transform.position = Vector3.SmoothDamp(transform.position, Target.position, ref _velocity, Time.deltaTime * Random.Range(MinModifier, MaxModifier));
        }
    }
}
