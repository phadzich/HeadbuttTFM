using PrimeTween;
using System.Linq;
using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;

public class BallDmgBlock : DamageBlock
{
    public float height;
    public float speed;
    public GameObject ball;
    private AudioSource audioSource;

    Block[] directions = new Block[4];
    public Vector3 currentPos;
    public Vector3 newDirection;
    private void Start()
    {
        impulseSource = GetComponent<CinemachineImpulseSource>();
        Tween.LocalPositionY(ball.transform, endValue: height, duration: speed, ease: Ease.OutExpo, startDelay: Random.Range(0, .5f)).OnComplete(AnimateDown);
        audioSource = GetComponent<AudioSource>();
    }

    public override void Bounce()
    {
        audioSource.PlayOneShot(damageSound, 0.7f);
    }

    private void OnDisable()
    {
        Tween.StopAll(ball.transform);
    }

    void AnimateUp()
    {
        Tween.LocalPositionY(ball.transform, endValue: height, duration: speed, ease: Ease.OutExpo).OnComplete(AnimateDown);
    }

    void AnimateDown()
    {
        Tween.LocalPositionY(ball.transform, endValue: -1, duration: speed, ease: Ease.InExpo).OnComplete(AnimateUp);
    }


    public void PushPlayerRandomly()
    {

        directions[0] = PlayerManager.Instance.playerMovement.blockBelow.up;
        directions[1] = PlayerManager.Instance.playerMovement.blockBelow.down;
        directions[2] = PlayerManager.Instance.playerMovement.blockBelow.left;
        directions[3] = PlayerManager.Instance.playerMovement.blockBelow.right;
        directions = directions.OrderBy(d => Random.value).ToArray();

        foreach (Block dir in directions)
        {
            if (dir.isWalkable)
            {

                Vector2 delta = dir.sublevelPosition - PlayerManager.Instance.playerMovement.blockBelow.sublevelPosition;
                Vector2 currentPos = PlayerManager.Instance.playerMovement.blockBelow.sublevelPosition;
                newDirection = GetCardinalDirection(delta);
                //Debug.Log(dir);
                break;
            }
        }
        audioSource.PlayOneShot(damageSound, 0.7f);
        //Debug.Log(newDirection);
        PlayerManager.Instance.playerMovement.Knockback(newDirection);
    }
    Vector3 GetCardinalDirection(Vector2 delta)
    {
        // Elige el eje con mayor valor absoluto
        if (Mathf.Abs(delta.x) > Mathf.Abs(delta.y))
        {
            return new Vector3(Mathf.Sign(delta.x), 0, 0);
        }
        else if (Mathf.Abs(delta.y) > 0)
        {
            return new Vector3(0, 0, Mathf.Sign(delta.y));
        }
        else
        {
            // Si delta es cero en ambos ejes (caso raro), no moverse
            return Vector3.zero;
        }
    }

}
