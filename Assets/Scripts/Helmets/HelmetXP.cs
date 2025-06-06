using System;
using UnityEngine;

public class HelmetXP
{
    // Level and sublevel info
    public int currentLevel = 1;
    public int currentSublevel = 1;
    public int currentXP = 0;
    public int baseRequiredXP;
    public float xpMultiplier;
    public HelmetInstance helmetInstanceRef;
    public int nextLevel => currentLevel + 1;

    // State
    private bool IsReadyToLevelUp => currentSublevel == 5; // Checa si tiene la suficiente experiencia para subir el nivel
    private bool IsAtMaxLevel => currentLevel == 3; // Checa si ya esta en el nivel mas alto
    public bool CanLevelUp => IsReadyToLevelUp && !IsAtMaxLevel; //Checa si el casco puede ser upgradeado tomando en cuenta lo anterior

    public Action<HelmetXP, HelmetInstance> XPChanged;
    public Action<int> SubleveledUp;
    public Action<int> LeveledUp;

    public HelmetXP(int baseXP, float ogXPMult,HelmetInstance _instanceRef)
    {
        baseRequiredXP = baseXP;
        xpMultiplier = ogXPMult;
        helmetInstanceRef = _instanceRef;
        XPChanged?.Invoke(this, helmetInstanceRef);
    }

    public void AddXP(int amount)
    {
        //Debug.Log("XP " + IsReadyToLevelUp + "Current sublevel " + currentSublevel);
        //Debug.Log("XP for next " + XPForNextSublevel());
        // Si ya puede subir el nivel del casco deja de de subir experiencia
        if (IsReadyToLevelUp) return;

        currentXP += amount;
        //Debug.Log("XP UPDATE:" + currentXP);
        while (currentXP >= XPForNextSublevel())
        {
            currentXP -= XPForNextSublevel();
            SublevelUp();
        }
        XPChanged?.Invoke(this,helmetInstanceRef);
    }

    public int XPForNextSublevel()
    {
        float levelFactor = Mathf.Pow(1.2f, currentLevel - 1);
        return Mathf.RoundToInt(baseRequiredXP * levelFactor * Mathf.Pow(xpMultiplier, currentSublevel - 1));
    }

    public void SublevelUp()
    {
        currentSublevel++;
        SubleveledUp?.Invoke(currentSublevel);
    }

    public void LevelUp()
    {
        currentLevel++;
        currentSublevel = 1; // reiniciamos los sublevels
        //Debug.Log("�Subiste a nivel " + currentLevel + "!");
        LeveledUp?.Invoke(currentLevel);
    }

}
