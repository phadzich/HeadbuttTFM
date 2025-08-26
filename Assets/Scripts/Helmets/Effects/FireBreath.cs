using UnityEngine;

[System.Serializable]
public class FireBreath : HelmetEffect
{
    private readonly FireBreathEffectData data;
    private Vector3 damageArea;

    public override bool hasSpecialAttack => true;
    public FireBreath(FireBreathEffectData _data)
    {
        data = _data;
        damageArea = _data.damageArea;
    }

    public override void OnWear()
    {
        PlayerManager.Instance.playerHeadbutt.onHBPointsChanged += OnHBPointsChanged;
        //UIManager.Instance.specialHeadbuttHUD.ShowIcon();
        OnHBPointsChanged(0, 0);
    }
    public override void OnUnwear()
    {
        PlayerManager.Instance.playerHeadbutt.onHBPointsChanged -= OnHBPointsChanged;
        //UIManager.Instance.specialHeadbuttHUD.HideIcon();
    }

    private void OnHBPointsChanged(float _a, float _b)
    {
        bool hasMaxHBPoints = PlayerManager.Instance.playerHeadbutt.hasMaxHBPoints;
        //UIManager.Instance.specialHeadbuttHUD.FadeIcon(hasMaxHBPoints ? 0f : 0.9f);
    }

    public override void OnUpgradeEffect(float stat)
    {
        var area = 0.5f + stat;

        damageArea = new Vector3(area, 1, area);
    }

    public override void OnHeadbutt()
    {
        if (PlayerManager.Instance.playerHeadbutt.TryUseHBPoints(data.hbPointsUsed))
        {
            Debug.Log("FIREBREATH USED!");
            HitEnemiesInArea();
        }
    }
    private void HitEnemiesInArea()
    {
        // Obtener la posicion del enano
        Transform dwarfTransform = PlayerManager.Instance.transform.GetChild(0); // Obtenemos el objeto del enano a partir de su game object padre
        Vector3 position = dwarfTransform.position; // Obtenemos su posicion para que sea el centro

        Collider[] hitColliders = Physics.OverlapBox(position, damageArea, Quaternion.identity, data.enemyLayer);

        InstantiateParticles(position);

        foreach (var hit in hitColliders)
        {
            Enemy enemy = hit.GetComponent<Enemy>();
            if (enemy != null)
            {
                Debug.Log(enemy);
                enemy.OnHit();
            }
        }
    }

    private void InstantiateParticles(Vector3 _position)
    {
        GameObject _particles = GameObject.Instantiate(data.effectParticles, _position, Quaternion.identity);
        _particles?.GetComponent<SpecialHeadbuttParticles>().Play();
    }
}