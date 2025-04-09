using UnityEngine;

public class Level : MonoBehaviour
{
    public string id;
    public LevelConfig config;

    public void SetupLevel(string _id, LevelConfig _config)
    {
        this.id = _id;
        this.config = _config;
    }
}
