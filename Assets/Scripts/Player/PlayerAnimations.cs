using PrimeTween;
using Unity.Cinemachine;
using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    [SerializeField]

    public GameObject bodyMesh;
    public float headbuttSquashRatio = .4f;
    public float headbuttSquashDuration = 1f;
    public float bounceSquashRatio = .3f;
    public float bounceSquashDuration = .3f;

    public void RotateBody(float _degrees)
    {
        var finalRotation = new Vector3(_degrees,0,0);
        Tween.LocalRotation(bodyMesh.transform,endValue: finalRotation, duration:.5f,ease:Ease.InOutExpo);
    }

    public void HeadbuttSS()
    {
        var startScale = new Vector3(1+headbuttSquashRatio, headbuttSquashRatio, 1+headbuttSquashRatio);
        var endScale = new Vector3(1, 1, 1);
        var startRotation = new Vector3(-180,0,0);
        var endRotation = new Vector3(0, 0, 0);
        Tween.Scale(bodyMesh.transform, startValue: startScale, endValue: endScale, duration: headbuttSquashDuration, ease: Ease.OutBack);
        //Tween.Rotation(bodyMesh.transform, startValue: startRotation, endValue: endRotation, duration: 2f, ease: Ease.InOutExpo).OnComplete(() => RotateBody(180));
        Tween.EulerAngles(bodyMesh.transform, startValue: startRotation, endValue: new Vector3(+180, 0,0), duration: 2);
    }

    public void BounceSS()
    {
        var startScale = new Vector3(1 + bounceSquashRatio, bounceSquashRatio, 1 + bounceSquashRatio);
        var endScale = new Vector3(1,1,1);
        Tween.Scale(bodyMesh.transform, startValue: startScale, endValue: endScale, duration: bounceSquashDuration, ease: Ease.OutSine);
    }
}
