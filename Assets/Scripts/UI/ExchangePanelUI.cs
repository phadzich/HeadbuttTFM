using UnityEngine;

public class ExchangePanelUI : MonoBehaviour
{
    public Transform buttonsContainer;
    public ExchangeButtonUI exchangeButtonPrefab;


    private void OnEnable()
    {
        PopulateButtons();
    }

    private void PopulateButtons()
    {
        ClearButtons();
        foreach (ResourceData _res in ResourceManager.Instance.allAvailableResources)
        {
            var _newButton = Instantiate(exchangeButtonPrefab, buttonsContainer);
            ExchangeButtonUI _exchangeButtonUI = _newButton.GetComponent<ExchangeButtonUI>();
            _exchangeButtonUI.SetupButton(_res);
        }
    }

    private void ClearButtons()
    {
        foreach(Transform _child in buttonsContainer)
        {
            Destroy(_child.gameObject);
        }
    }
}
