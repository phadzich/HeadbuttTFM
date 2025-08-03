using PrimeTween;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [Header("MAIN CAMERA")]
    public GameObject playerCam;
    public GameObject fogMesh;
    public float playerCamHeight;
    public float moveDownDuration;
    public float moveUpDuration;

    public void MoveCamDown(int _count)
    {

        Tween.PositionY(fogMesh.transform,
    startValue: fogMesh.transform.position.y,
    endValue: (_count * -LevelManager.Instance.distanceBetweenSublevels),
    duration: moveDownDuration,
    startDelay:1f,
    ease: Ease.InOutQuad);
    }

    public void MoveCamToDepth(int _depth)
    {

        Tween.PositionY(fogMesh.transform,
    startValue: fogMesh.transform.position.y,
    endValue: (_depth * -LevelManager.Instance.distanceBetweenSublevels),
    duration: moveUpDuration,
    ease: Ease.InOutQuad);
        //Debug.Log((_depth * LevelManager.Instance.distanceBetweenSublevels) + playerCamHeight);
    }
}
