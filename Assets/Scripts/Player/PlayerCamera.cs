using PrimeTween;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [Header("MAIN CAMERA")]
    public GameObject playerCam;
    public float playerCamHeight = 20f;
    public float moveDownDuration;
    public float moveUpDuration;

    public void MoveCamDown(int _count)
    {
        Tween.PositionY(playerCam.transform,
            startValue: playerCam.transform.position.y,
            endValue: (_count * -LevelManager.Instance.distanceBetweenSublevels) + playerCamHeight,
            duration: moveDownDuration,
            ease: Ease.InOutQuad);
    }

    public void MoveCamToDepth(int _depth)
    {
        Debug.Log(_depth);
        Tween.PositionY(playerCam.transform,
            startValue: playerCam.transform.position.y,
            endValue: (_depth*-LevelManager.Instance.distanceBetweenSublevels) + playerCamHeight,
            duration: moveUpDuration,
            ease: Ease.InOutQuad);
        Debug.Log((_depth * LevelManager.Instance.distanceBetweenSublevels) + playerCamHeight);
    }
}
