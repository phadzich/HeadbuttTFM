using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RemainingBlocksIndicator : MonoBehaviour
{
    public GameObject resourceIcon;
    public Transform gridContainer;

    private void Start()
    {
        //ToggleIndicator(false);
    }
    public void UpdateIndicator(ResourceData _resource, int _current, int _max)
    {
        foreach(Transform _child in gridContainer)
        {
            Destroy(_child.gameObject);
        }

        for (int i = 0; i < _max; i++)
        {
            GameObject _newCounter = Instantiate(resourceIcon, gridContainer);
            _newCounter.GetComponent<Image>().sprite = _resource.icon;
            if (i < _current)
            {
                _newCounter.GetComponent<Image>().color = Color.white;
            }
            else
            {
                _newCounter.GetComponent<Image>().color = Color.black;
            }

        }

    }
    public void ToggleIndicator(bool _visible)
    {
        //Debug.Log(_visible);
        this.gameObject.SetActive(_visible);
    }
}
