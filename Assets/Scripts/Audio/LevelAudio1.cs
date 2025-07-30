using UnityEngine;

public class LevelAudio1 : MonoBehaviour
{
    void Awake()
    {
        SoundManager.PlaySound(SoundType.LEVELMUSIC, 1f);
        SoundManager.PlaySound(SoundType.LEVELAMBIENT, 1f);
    }
}