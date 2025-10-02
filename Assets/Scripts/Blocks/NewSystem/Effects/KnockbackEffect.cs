using System.Linq;
using UnityEngine;

[RequireComponent(typeof(BlockNS))]
public class KnockbackEffect : MonoBehaviour, IBlockEffect
{
    [Header("KNOCKBACK")]
    BlockNS[] directions = new BlockNS[4];
    public Vector3 newDirection;


    public void OnBounced(HelmetInstance _helmetInstance)
    {

        PushPlayerRandomly();
        PlayBounceSound();
    }

    public void OnHeadbutt(HelmetInstance _helmetInstance)
    {
        PushPlayerRandomly();
        PlayBounceSound();
    }

    private void PlayBounceSound()
    {
        SoundManager.PlaySound(SFXType.PUSH_DAMAGE, 0.7f);
    }

    public void PushPlayerRandomly()
    {

        directions[0] = PlayerManager.Instance.playerMovement.blockNSBelow.up;
        directions[1] = PlayerManager.Instance.playerMovement.blockNSBelow.down;
        directions[2] = PlayerManager.Instance.playerMovement.blockNSBelow.left;
        directions[3] = PlayerManager.Instance.playerMovement.blockNSBelow.right;
        directions = directions.OrderBy(d => Random.value).ToArray();

        foreach (BlockNS dir in directions)
        {
            if (dir.isWalkable)
            {

                Vector2 delta = dir.sublevelPosition - PlayerManager.Instance.playerMovement.blockNSBelow.sublevelPosition;
                Vector2 currentPos = PlayerManager.Instance.playerMovement.blockNSBelow.sublevelPosition;
                newDirection = GetCardinalDirection(delta);
                //Debug.Log(dir);
                break;
            }
        }

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
