using System.Linq;
using UnityEngine;
using UnityEngine.Audio;

public class SlimeDmgBlock : DamageBlock
{
    Block[] directions = new Block[4];
    [Header("Slime")]
    public Vector3 currentPos;
    public Vector3 newDirection;
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    public override void Bounce()
    {

        if (HelmetManager.Instance.currentHelmet.helmetEffect != helmetCounter)
        {
            PushPlayerRandomly();
        }

        PushPlayerRandomly();
        audioSource.PlayOneShot(damageSound, 0.7f);
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
