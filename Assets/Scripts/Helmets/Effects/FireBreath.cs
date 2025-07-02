using UnityEngine;

[System.Serializable]
public class FireBreath : HelmetEffect
{
    private readonly FireBreathEffectData data;

    public FireBreath(FireBreathEffectData _data)
    {
        data = _data;
    }

    public override void OnSpecialAttack()
    {
        // Se desata la ola, se activa el colider
    }
}