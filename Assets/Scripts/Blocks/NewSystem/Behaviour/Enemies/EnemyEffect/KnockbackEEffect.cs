using System.Linq;
using UnityEngine;

public class KnockbackEEffect : MonoBehaviour, IEnemyEffect
{
    [Header("KNOCKBACK")]
    BlockNS[] directions = new BlockNS[4];
    public Vector3 newDirection;
    public void OnHit()
    {
        //
    }

    public void OnTrigger()
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
                newDirection = new Vector3(Mathf.RoundToInt(delta.x), 0, Mathf.RoundToInt(delta.y));
                break;
            }
        }

        //Debug.Log(newDirection);
        PlayerManager.Instance.playerMovement.Knockback(newDirection);
    }

}
