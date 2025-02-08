using UnityEngine;

public class BlockData : MonoBehaviour
{
    [SerializeField]
    public int jumpCount;

    public GameObject hitIndicatorPF;
    public string blockResource;
    public int comboToBreak;

    private void Start()
    {
        ShowHitIndicator(false);
    }

    public void GetHit()
    {
        ShowHitIndicator(true);

        if (GameManager.instance.currentComboBlock != blockResource)
            {
                GameManager.instance.ClearAllHitBlocks();
                GameManager.instance.currentComboBlock = blockResource;
        }

        GameManager.instance.AddBlockToHitBlocks(this);


        if (GameManager.instance.currentComboCount == comboToBreak)
        {
            GameManager.instance.BreakAllHitBlocks();
        }
    }

    public void ShowHitIndicator(bool _visible)
    {
        hitIndicatorPF.SetActive(_visible);   
    }
}
