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

    public void UpdateHelmetInstanceInfo(HelmetInstance _instance)
    {
        foreach (HelmetIndicator _helmetInidcator in equippedIndicators)
        {
            if(_helmetInidcator.helmetInstance == _instance)
            {
                Debug.Log($"Udpdating Helmet Indicator{_instance}");
                _helmetInidcator.UpdateIndicator();
            }

        }
    }

    public void UpdateWearingHelmet(HelmetInstance _instance)
    {
        //Debug.Log(_instance.id);
        foreach (HelmetIndicator _helmetInidcator in equippedIndicators)
        {
            if(_helmetInidcator.helmetInstance!= _instance)
            {
                _helmetInidcator.Unwear();
            }
            else
            {
                _helmetInidcator.Wear();
            }

        }
    }


    public void ClearInstancedIndicators()
    {
        equippedIndicators.Clear();
        foreach (Transform _child in indicatorsContainer)
        {
            Destroy(_child.gameObject);
        }
    }

    public void CheckIfUpgradesAvailable()
    {
        foreach(HelmetIndicator _helmIndic in equippedIndicators)
        {
            if (CraftingManager.Instance.hasEnoughResourcesToUpgrade(_helmIndic.helmetInstance))
            {
                List<ResourceRequirement> _requirements = CraftingManager.Instance.GetHelmetBlueprint(_helmIndic.helmetInstance).requiredResources;
                _helmIndic.ShowUpgradeButton(true);
                if (_requirements.Count == 1)
                {
                    _helmIndic.UpdateReq01(_requirements[0]);
                    _helmIndic.UpdateReq02(null);
                }
                else if (_requirements.Count == 2)
                {
                    _helmIndic.UpdateReq01(_requirements[0]);
                    _helmIndic.UpdateReq02(_requirements[1]);
                }
            }
            else
            {
                _helmIndic.ShowUpgradeButton(false);
            }
        }
    }

}
