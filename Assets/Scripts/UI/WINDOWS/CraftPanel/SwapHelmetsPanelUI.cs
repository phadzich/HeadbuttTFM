using UnityEngine;

public class SwapHelmetsPanelUI : MonoBehaviour
{

    public GameObject helmetButtonPrefab;
    public GameObject helmetListContainer;

    private void OnEnable()
    {
        LoadMainPage();
    }

    private void LoadMainPage()
    {
        UpdateHelmetList();
    }

    public void UpdateHelmetList()
    {
        foreach (Transform child in helmetListContainer.transform)
        {
            Destroy(child.gameObject);
        }

        foreach (var _helmet in HelmetManager.Instance.helmetsEquipped)
        {
            Instantiate(helmetButtonPrefab, helmetListContainer.transform);
                //.GetComponent<HelmetSwapButton>().SetUp(_helmet);
        }
    }
}
