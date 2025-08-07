using UnityEngine;

public class PotionCollectible : MonoBehaviour, ICollectibleEffect
{
    public GameObject potionMesh;
    private Sublevel parentSublevel;
    private int potionSize;
    float meshSize;

    [SerializeField] private PotionTypes type;

    public void SetupBlock(string _potionSize, MapContext _context)
    {
        parentSublevel = _context.sublevel;
        potionSize = int.Parse(_potionSize);
        meshSize = ((float)potionSize / 10f) + .2f;
        potionMesh.transform.localScale = new Vector3(meshSize, meshSize, meshSize);
    }

    public void Activate()
    {
        Debug.Log("POTION OBTAINED");

        switch (type)
        {
            case PotionTypes.Durability:
                HelmetManager.Instance.UseHelmetPotion(potionSize);
                break;
            case PotionTypes.HBPoints:
                PlayerManager.Instance.playerHeadbutt.UseHBPotion(potionSize);
                break;
        }

    }

}

