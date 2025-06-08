using System;
using UnityEngine;

public class HelmetXP
{
    // Level and sublevel info
    public int currentEvolution = 1;
    public int currentLevel = 1;
    public int currentXP = 0;
    public int baseRequiredXP;
    public float xpMultiplier;
    public HelmetInstance helmetInstanceRef;
    public int nextEvolution => currentEvolution + 1;

    // State
    private bool IsReadyToEvolve => currentLevel == 5; // Checa si tiene la suficiente experiencia para subir el nivel
    private bool IsEvolveToMax => currentEvolution == 3; // Checa si ya esta en el nivel mas alto
    public bool CanEvolve => IsReadyToEvolve && !IsEvolveToMax; //Checa si el casco puede ser upgradeado tomando en cuenta lo anterior

    public Action<HelmetXP, HelmetInstance> XPChanged;
    public Action<int> SubleveledUp;
    public Action<int> LeveledUp;

    public HelmetXP(int _baseXP, float _ogXPMult,HelmetInstance _instanceRef)
    {
        baseRequiredXP = _baseXP;
        xpMultiplier = _ogXPMult;
        helmetInstanceRef = _instanceRef;
        XPChanged?.Invoke(this, helmetInstanceRef);
    }

    public void AddXP(int amount)
    {
        // Si ya puede subir el nivel del casco deja de de subir experiencia
        if (IsReadyToEvolve) return;

        currentXP += amount;
        //Debug.Log("XP UPDATE:" + currentXP);
        while (currentXP >= XPForNextLevel())
        {
            currentXP -= XPForNextLevel();
            SublevelUp();
        }
        XPChanged?.Invoke(this,helmetInstanceRef);
    }

    public int XPForNextLevel()
    {
        float levelFactor = Mathf.Pow(1.2f, currentEvolution - 1);
        return Mathf.RoundToInt(baseRequiredXP * levelFactor * Mathf.Pow(xpMultiplier, currentLevel - 1));
    }

    public void SublevelUp()
    {
        currentLevel++;
        SubleveledUp?.Invoke(currentLevel);
    }

    public void Evolve()
    {
        currentEvolution++;
        currentLevel = 1; // reiniciamos los sublevels
        //Debug.Log("�Subiste a nivel " + currentLevel + "!");
        LeveledUp?.Invoke(currentEvolution);
    }

}
