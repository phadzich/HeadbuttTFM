using UnityEngine;

public class TransformBehaviour : MonoBehaviour, IElementReactive
{
    public bool IsAllowedForSource(InteractionSource source)
    {
        return false;
    }

    public void OnElementInteraction(ElementType sourceElement, ElementType targetElement)
    {
        
    }
}
