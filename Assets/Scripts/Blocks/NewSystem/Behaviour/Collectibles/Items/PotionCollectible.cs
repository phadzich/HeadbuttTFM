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
                PlayHPPotionSound();
                HelmetManager.Instance.UseHelmetPotion(potionSize);
                break;
            case PotionTypes.HBPoints:
                PlayHBPotionSound();
                PlayerManager.Instance.playerHeadbutt.UseHBPotion(potionSize);
                break;
        }

    }

    public void PlayHBPotionSound()
    {
        switch (potionSize)
        {
            case 0:
                SoundManager.PlaySound(SFXType.HB_SMALL_POTION);
                break;
            case 1:
                SoundManager.PlaySound(SFXType.HB_MEDIUM_POTION);
                break;
            case 2:
                SoundManager.PlaySound(SFXType.HB_LARGE_POTION);
                break;
            case 3:
                SoundManager.PlaySound(SFXType.HB_EXTLARGE_POTION);
                break;
        }
             
    }

    public void PlayHPPotionSound()
    {
        switch (potionSize)
        {
            case 0:
                SoundManager.PlaySound(SFXType.HP_SMALL_POTION);
                break;
            case 1:
                SoundManager.PlaySound(SFXType.HP_MEDIUM_POTION);
                break;
            case 2:
                SoundManager.PlaySound(SFXType.HP_LARGE_POTION);
                break;
            case 3:
                SoundManager.PlaySound(SFXType.HP_EXTLARGE_POTION);
                break;
        }

    }

}

