using System;
using System.Buffers.Text;
using UnityEngine;

public class XPManager : MonoBehaviour
{
    public static XPManager Instance;

    public int currentLevel = 1;
    public int currentXP = 0;
    public int baseRequiredXP = 20;
    public float xpMultiplier = 1.5f;

    public Action<int,int> XPChanged;
    public Action<int> LeveledUp;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            Debug.Log("XP Awake");
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddXP(int amount)
    {
        currentXP += amount;
        while (currentXP >= XPForNextLevel())
        {
            currentXP -= XPForNextLevel();
            LevelUp();
        }
        XPChanged?.Invoke(currentXP, XPForNextLevel());
    }
    int XPForNextLevel()
    {
        return Mathf.RoundToInt(baseRequiredXP * Mathf.Pow(xpMultiplier, currentLevel - 1));
    }

    void LevelUp()
    {
        currentLevel++;
        //Debug.Log("¡Subiste a nivel " + currentLevel + "!");
        LeveledUp?.Invoke(currentLevel);
    }

}

