using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelConfig", menuName = "GameData/LevelConfig")]
public class LevelConfig : ScriptableObject
{
    public int id;

    public string levelName;
    public string levelDescription;
    public int maxDepth;

    public Sprite levelIcon;
    public Color levelColor;

    public AudioClip levelMusic;
    public AudioClip levelNpcMusic;
    public AudioClip levelAmbient;

    [SerializeField] public List<SublevelConfig> subLevels;
}
