using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "GameData/Dialogue Sequence")]
public class DialogueSequence : ScriptableObject
{
    public List<DialogueLine> lines;

}
