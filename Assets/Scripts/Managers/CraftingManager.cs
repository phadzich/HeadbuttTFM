using System.Collections.Generic;
using UnityEngine;

public class CraftingManager : MonoBehaviour
{
    public static CraftingManager Instance;

    public List<HelmetBlueprint> blueprints;
    public ResourceRequirement bouncePrice;
    public ResourceRequirement headbuttPrice;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            Debug.Log("HelmetManager Awake");
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void LevelUpJumpHelmet(HelmetInstance helmet, int quantity) {
        helmet.upgradeJump(quantity);
    }

    public void LevelUpHeadbuttHelmet(HelmetInstance helmet, int quantity)
    {
        helmet.upgradeHeadbutt(quantity);
    }

    public void MergeHelmets()
    {
    }
}
