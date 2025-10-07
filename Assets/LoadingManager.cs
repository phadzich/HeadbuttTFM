using UnityEngine;
using System.Collections;
using PrimeTween;

public class LoadingManager : MonoBehaviour
{
    [SerializeField] private GameObject loadingScreen;

    public void LoadLevelCoroutine(System.Action loadAction)
    {
        StartCoroutine(LoadAsync(loadAction));
    }

    private IEnumerator LoadAsync(System.Action loadAction)
    {
        StartCoroutine(FadeIn());

        // Espera un frame para que el UI se actualice visualmente
        yield return new WaitForSeconds(0.4f);

        // Ejecuta tu carga pesada (instanciación, destrucción, etc.)
        loadAction?.Invoke();

        // Espera un poco si quieres dar tiempo a assets o efectos
        yield return new WaitForSeconds(0.3f);

        FadeOut();
    }

    private IEnumerator FadeIn()
    {

        EnableScreen(true);

        // Forzar que Unity dibuje el canvas ahora, no en el siguiente frame
        Canvas.ForceUpdateCanvases();
        Tween.Alpha(loadingScreen.GetComponent<CanvasGroup>(), endValue: 1, duration: 0.3f, ease: Ease.OutSine);
        yield return null;
    }

    private void FadeOut()
    {
        Tween.Alpha(loadingScreen.GetComponent<CanvasGroup>(), startValue: 1, endValue: 0, duration: .5f, ease: Ease.InSine).OnComplete(() => EnableScreen(false));
    }

    private void EnableScreen(bool _value)
    {
        loadingScreen.SetActive(_value);

    }
}