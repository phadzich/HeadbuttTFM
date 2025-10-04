using PrimeTween;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialoguePanelUI : MonoBehaviour
{
    public TextMeshProUGUI dialogueTXT;
    public TextMeshProUGUI dialogueButtonTXT;
    public Image playerImage;
    public Image npcImage;
    public GameObject dialogFade;

    public void Open()
    {
        AnimateNPC();
        AnimatePlayer();
        AnimateText();
        this.gameObject.SetActive(true);
        dialogFade.SetActive(true);
    }
    
    public void Close()
    {
        this.gameObject.SetActive(false);
        dialogFade.SetActive(false);
    }

    public void UpdateDialogContent(DialogueLine _line)
    {
        dialogueTXT.text = _line.text;
        npcImage.sprite = _line.npcImage;
        playerImage.sprite = _line.playerImage;
        dialogueButtonTXT.text = _line.buttonText;
        AnimateNPC();
        //AnimatePlayer();
        AnimateText();
    }

    private void AnimateNPC()
    {
        Tween.ScaleY(npcImage.transform, startValue: .6f, endValue: 1f, duration: .4f, ease:Ease.OutElastic);
    }
    private void AnimatePlayer()
    {
        Tween.ScaleY(playerImage.transform, startValue: .6f, endValue: 1f, duration: .3f, ease: Ease.OutBack);
    }
    private void AnimateText()
    {
        Tween.ScaleY(dialogueTXT.gameObject.transform, startValue: .3f, endValue: 1f, duration: .8f, ease: Ease.OutBack);
        Tween.ScaleX(dialogueTXT.gameObject.transform, startValue: .3f, endValue: 1f, duration: .6f, ease: Ease.OutBack);
    }
}
