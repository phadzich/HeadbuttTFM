using System.Linq;

public interface IElementReactive
{
    public bool IsAllowedForSource(InteractionSource source);

    void OnElementInteraction(ElementType sourceElement, ElementType targetElement);
}
