using PrimeTween;
using System.Collections;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [Header("MAIN CAMERA")]
    public GameObject playerCam;
    public GameObject fogMesh;
    public float playerCamHeight;
    public float moveDownDuration;
    public float moveUpDuration;

    public void MoveFogDown(int _count)
    {
        Debug.Log("MOVINGFOGDOWN");
        Tween.PositionY(fogMesh.transform,
    startValue: fogMesh.transform.position.y,
    endValue: (_count * -LevelManager.Instance.distanceBetweenSublevels),
    duration: moveDownDuration,
    startDelay:1f,
    ease: Ease.InOutQuad);
    }

    public IEnumerator MoveFogNextFrame(int _depth)
    {
        yield return new WaitForSeconds(.1f);
        Debug.Log("MOVINGFOG");
        Tween.PositionY(fogMesh.transform,
  startValue: fogMesh.transform.position.y,
  endValue: (_depth * -LevelManager.Instance.distanceBetweenSublevels),
  duration: moveUpDuration,
  ease: Ease.InOutQuad);
    }

    public void MoveFogToDepth(int _depth)
    {
        Debug.Log("MOVINGFOGTODEPTH");
        Debug.Log(_depth * -LevelManager.Instance.distanceBetweenSublevels);
        StartCoroutine(MoveFogNextFrame(_depth));
        //Debug.Log((_depth * LevelManager.Instance.distanceBetweenSublevels) + playerCamHeight);
    }

    

}
