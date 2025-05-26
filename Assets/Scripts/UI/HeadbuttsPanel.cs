using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeadbuttsPanel : MonoBehaviour
{
    public GameObject hbIndicatorPF;
    public Transform indicatorsContainer;
    public List<GameObject> allIndicators;
    public Color availableColor;
    public Color usedColor;

    public void UpdateUsedHeadbutts(HelmetInstance _helmetInstance)
    {
        //Debug.Log("UPDATING HEADBUTS IND" + _helmetInstance.baseHelmet.helmetName);
        ClearInstancedIndicators();
        GetComponent<Image>().color=_helmetInstance.baseHelmet.color;

        for (int i = 0; i < _helmetInstance.maxHeadbutts; i++)
        {
            var newIndicatorUI = Instantiate(hbIndicatorPF, indicatorsContainer);
            allIndicators.Add(newIndicatorUI);
            if (i < _helmetInstance.remainingHeadbutts)
            {
                allIndicators[i].GetComponent<Image>().color = availableColor;
            }
            else
            {
                allIndicators[i].GetComponent<Image>().color = usedColor;
            }
        }
    }

    public void ClearInstancedIndicators()
    {
        allIndicators.Clear();
        foreach (Transform _child in indicatorsContainer)
        {
            Destroy(_child.gameObject);
        }
    }
}
