using UnityEngine;
using PrimeTween;

public class SquashStretchIdle : MonoBehaviour
{
    [Header("Animación")]
    [SerializeField] float scaleAmount = 0.12f;   // cuánto se estira/aplasta
    [SerializeField] float duration = 0.8f;       // tiempo de ida (la vuelta la gestiona CycleMode.Yoyo)
    [SerializeField] bool includeChildren = false;
    [SerializeField] float maxRandomDelay = 0.35f; // fase aleatoria para desincronizar
    [SerializeField] Ease ease = Ease.InOutSine;

    void Start()
    {
        if (includeChildren)
        {
            foreach (var rend in GetComponentsInChildren<Renderer>())
            {
                Animate(rend.transform);
            }
        }
        else
        {
            Animate(transform);
        }
    }

    void Animate(Transform t)
    {
        Vector3 baseScale = t.localScale;
        Vector3 targetScale = new Vector3(
            baseScale.x + scaleAmount,
            baseScale.y - scaleAmount,
            baseScale.z + scaleAmount
        );

        // cycles: -1 -> infinito, cycleMode: Yoyo -> va y vuelve (ping-pong)
        Tween.Scale(
            t,
            endValue: targetScale,
            duration: duration,
            startDelay: Random.Range(0f, maxRandomDelay),
            ease: ease,
            cycles: -1,
            cycleMode: CycleMode.Yoyo
        );
    }
}
