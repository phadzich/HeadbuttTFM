using System.Collections.Generic;
using UnityEngine;

public class ElementInteractionComponent : MonoBehaviour
{
    private List<IElementReactive> reactives;
    private IElemental elementalOwner;

    private void Awake()
    {
        // Buscar todos los behaviours/effects en este objeto que puedan reaccionar
        reactives = new List<IElementReactive>(GetComponents<IElementReactive>());

        // Obtener el elemento del dueño (si lo tiene)
        elementalOwner = GetComponent<IElemental>();
    }

    /// Llamar cuando algo con elemento interactúa con este objeto.
    public void HandleInteraction(ElementType sourceElement, InteractionSource source)
    {
        foreach (var reaction in reactives)
        {
            if (reaction.IsAllowedForSource(source))
            {
                reaction.OnElementInteraction(sourceElement, elementalOwner.Element);
            }
        }
    }
}
