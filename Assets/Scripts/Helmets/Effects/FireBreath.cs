using UnityEngine;

[System.Serializable]
public class FireBreath : HelmetEffect
{
    private readonly FireBreathEffectData data;

    public override bool hasSpecialAttack => true;
    public FireBreath(FireBreathEffectData _data)
    {
        data = _data;
    }

    public override void OnWear()
    {
        PlayerManager.Instance.playerHeadbutt.onHBPointsChanged += OnHBPointsChanged;
        UIManager.Instance.specialHeadbuttHUD.ShowIcon();
        OnHBPointsChanged(0, 0);
    }
    public override void OnUnwear()
    {
        PlayerManager.Instance.playerHeadbutt.onHBPointsChanged -= OnHBPointsChanged;
        UIManager.Instance.specialHeadbuttHUD.HideIcon();
    }

    private void OnHBPointsChanged(float _a, float _b)
    {
        bool hasMaxHBPoints = PlayerManager.Instance.playerHeadbutt.hasMaxHBPoints;
        UIManager.Instance.specialHeadbuttHUD.FadeIcon(hasMaxHBPoints ? 0f : 0.9f);
    }

    public override void OnSpecialAttack()
    {
        if (PlayerManager.Instance.playerHeadbutt.hasMaxHBPoints)
        {
            Debug.Log("FIREBREATH USED!");
            PlayerManager.Instance.playerHeadbutt.TryUseHBPoints(data.hbPointsUsed);
            PlayerManager.Instance.playerHeadbutt.HeadbuttUp();
            //LOGICA DEL COLLIDER!


            // Obtener la posicion del enano
            Transform dwarfTransform = PlayerManager.Instance.transform.GetChild(0); // Obtenemos el objeto del enano a partir de su game object padre
            Vector3 position = dwarfTransform.position; // Obtenemos su posicion para que sea el centro

            Collider[] hitColliders = Physics.OverlapBox(position, data.damageArea, Quaternion.identity, data.enemyLayer);

            foreach (var hit in hitColliders)
            {
                Enemy enemy = hit.GetComponent<Enemy>();
                if (enemy != null)
                {
                    Debug.Log(enemy);
                    enemy.OnHit(data.damage);
                }
            }
        }
        else
        {
            Debug.Log("HBs NOT AT 100%");
        }

    }
}