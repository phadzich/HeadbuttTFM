using UnityEngine;

[System.Serializable]
public class DialogueLine
{
    [TextArea]
    public string text;
    public Sprite npcImage;
    public Sprite playerImage;
    public string buttonText;
    public int highlightID;
}
