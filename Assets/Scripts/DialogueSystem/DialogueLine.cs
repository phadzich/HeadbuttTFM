using UnityEngine;

[System.Serializable]
public class DialogueLine
{
    [TextArea]
    public string text;
    public Sprite npcImage;
    public string buttonText;
    public int highlightID;
    public AudioClip npcSFX;
    public AudioClip jomaSFX;
}
