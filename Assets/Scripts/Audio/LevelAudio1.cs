using UnityEngine;

public class LevelAudio1 : MonoBehaviour
{
    void Awake()
    {
        SoundManager.PlaySound(SoundType.LEVELWATERMUSIC, 1f);
        SoundManager.PlaySound(SoundType.LEVELWATERAMBIENT, 1f);
    }
}