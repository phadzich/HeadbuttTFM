using System.Drawing;
using UnityEngine;

public class CombatLogHUD : MonoBehaviour
{
    public LogDialog logPrefab;
    public static CombatLogHUD Instance;
    public GameObject logHUD;

    private void Awake()
    {
        Instance = this;
    }
    public void AddLog(Sprite _icon, string _text)
    {
        LogDialog _logObj = Instantiate(logPrefab, this.transform);
        _logObj.ShowDialog(_icon, _text);
    }


}
