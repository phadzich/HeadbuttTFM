using System.Collections.Generic;
using UnityEngine;

public class EquippedHelmetsPanel : MonoBehaviour
{
    public Transform indicatorsContainer;
    public GameObject indicatorPrefab;
    public List<HelmetIndicator> equippedIndicators;
    public void InstanceEquippedIndicators(List<HelmetInstance> _helmetList)
    {
        ClearInstancedIndicators();
        foreach (HelmetInstance _helmetInstance in _helmetList)
        {
            var newIndicatorUI = Instantiate(indicatorPrefab, indicatorsContainer);
            HelmetIndicator _indicator = newIndicatorUI.GetComponent<HelmetIndicator>();
            _indicator.SetupIndicator(_helmetInstance);
            equippedIndicators.Add(_indicator);
        }
    }

    public void UpdateHelmetInstanceInfo(string _instanceId)
    {
        foreach (HelmetIndicator _helmetInidcator in equippedIndicators)
        {
            if(_helmetInidcator.helmetInstance.id == _instanceId)
            {
                //Debug.Log($"Udpdating Helmet Indicator{_instanceId}");
                _helmetInidcator.UpdateIndicator();
            }

        }
    }

    public void UpdateWearingHelmet(int _index)
    {
        foreach (HelmetIndicator _helmetInidcator in equippedIndicators)
        {
            _helmetInidcator.Unwear();
        }
            equippedIndicators[_index].Wear();
    }


    public void ClearInstancedIndicators()
    {
        equippedIndicators.Clear();
        foreach (Transform _child in indicatorsContainer)
        {
            Destroy(_child.gameObject);
        }
    }

}
