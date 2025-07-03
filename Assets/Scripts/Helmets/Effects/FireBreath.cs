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
        UIManager.Instance.specialHeadbuttHUD.FadeIcon(hasMaxHBPoints ? 0f : 0.7f);
    }

    public override void OnSpecialAttack()
    {
        if (PlayerManager.Instance.playerHeadbutt.hasMaxHBPoints)
        {
            Debug.Log("FIREBREATH USED!");
            PlayerManager.Instance.playerHeadbutt.TryUseHBPoints(data.hbPointsUsed);
            PlayerManager.Instance.playerHeadbutt.HeadbuttUp();
            //LOGICA DEL COLLIDER!

            var boxSize = new Vector3(data.damageArea, data.damageArea, data.damageArea);
            Collider[] hitColliders = Physics.OverlapBox(PlayerManager.Instance.gameObject.transform.position, boxSize, Quaternion.identity, data.enemyLayer);

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
        else
        {
            Debug.Log("HBs NOT AT 100%");
        }

    }
}