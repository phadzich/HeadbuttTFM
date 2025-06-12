using PrimeTween;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [Header("MAIN CAMERA")]
    public GameObject playerCam;
    public float playerCamHeight = 20f;
    public float moveDownDuration;

    public void MoveCamDown(int _count)
    {
        Tween.PositionY(playerCam.transform,
            startValue: playerCam.transform.position.y,
            endValue: (_count * -LevelManager.Instance.distanceBetweenSublevels) + playerCamHeight,
            duration: moveDownDuration,
            ease: Ease.InOutQuad);
    }
}
