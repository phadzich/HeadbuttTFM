using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Pseudo;

[System.Serializable]
public class CrossAttackEffect : HelmetEffect
{
    private readonly CrossAttackEffectData data;
    private int crossRange;

    public override bool hasSpecialAttack => true;
    public CrossAttackEffect(CrossAttackEffectData _data)
    {
        data = _data;
        crossRange = _data.crossRange;
    }

    public override void OnWear()
    {
        PlayerManager.Instance.playerHeadbutt.onHBPointsChanged += OnHBPointsChanged;
        OnHBPointsChanged(0, 0);
    }
    public override void OnUnwear()
    {
        PlayerManager.Instance.playerHeadbutt.onHBPointsChanged -= OnHBPointsChanged;
    }

    private void OnHBPointsChanged(float _a, float _b)
    {
        bool hasMaxHBPoints = PlayerManager.Instance.playerHeadbutt.hasMaxHBPoints;
    }

    public override void OnUpgradeEffect(float _stat)
    {
        crossRange = (int)_stat;
    }

    public override void OnHeadbutt()
    {
        if (PlayerManager.Instance.playerHeadbutt.TryUseHBPoints(data.hbPointsUsed))
        {
            HitEnemiesInCross();
        }
    }
    private void HitEnemiesInCross()
    {
        Transform dwarfTransform = PlayerManager.Instance.transform.GetChild(0);
        Vector3 center = dwarfTransform.position;

        HashSet<Collider> hitColliders = new HashSet<Collider>();

        // --- Centro ---
        Vector3 centerExtents = new Vector3(0.5f, 1f, 0.5f);
        AddHitsFromBox(center, centerExtents, hitColliders);

        // --- Direcciones ---
        float step = 1.0f; // tamaño de 1 celda
        float width = 0.5f;
        float height = 1f;

        Vector3[] directions = new Vector3[]
        {
        Vector3.forward,
        Vector3.back,
        Vector3.left,
        Vector3.right
        };

        foreach (var dir in directions)
        {
            float length = crossRange * step;
            float halfLength = length * 0.5f;

            // halfExtents orientados según la dirección
            Vector3 halfExtents;
            Quaternion rot;

            if (dir == Vector3.forward || dir == Vector3.back)
            {
                halfExtents = new Vector3(width, height, halfLength);
                rot = Quaternion.identity; // Z ya apunta hacia adelante
            }
            else // left / right
            {
                halfExtents = new Vector3(halfLength, height, width);
                rot = Quaternion.Euler(0, 90, 0); // rotar para alinear en X
            }

            // Centro del box
            Vector3 boxCenter = center + dir * (halfLength+0.5f);

            AddHitsFromBox(boxCenter, halfExtents, hitColliders);

            Vector3 size;

            if (dir == Vector3.forward || dir == Vector3.back)
            {
                // brazo vertical (Z)
                size = new Vector3(1f, crossRange, 1f);
            }
            else
            {
                // brazo horizontal (X)
                size = new Vector3(crossRange, 1f, 1f);
            }

            // centro del brazo a mitad de largo más offset
            Vector3 pos = center + dir * (crossRange * 0.5f + 0.5f);

            InstantiateParticles(pos, size);

        }



        // --- Aplicar daño ---
        foreach (var hit in hitColliders)
        {
            Enemy enemy = hit.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.OnHit();
            }
        }
    }

    private void AddHitsFromBox(Vector3 center, Vector3 halfExtents, HashSet<Collider> hitColliders)
    {
        Collider[] cols = Physics.OverlapBox(center, halfExtents, Quaternion.identity, LayerMask.GetMask("Enemy"));
        foreach (var c in cols)
        {
            hitColliders.Add(c);
        }

        // Debug
        Debug.DrawLine(center, center + Vector3.up * 2f, Color.red, 1f);
        DrawBox(center, halfExtents, Quaternion.identity, Color.yellow, 1f);
    }

    private void InstantiateParticles(Vector3 pos, Vector3 size)
    {
        GameObject fx = GameObject.Instantiate(data.effectParticles, pos, Quaternion.identity);
        var sp = fx.GetComponent<SpecialHeadbuttParticles>();
        sp.SetSize(size);
        sp.Play();
    }



    private void DrawBox(Vector3 center, Vector3 halfExtents, Quaternion orientation, Color color, float duration = 0f)
    {
        Vector3 right = orientation * Vector3.right;
        Vector3 up = orientation * Vector3.up;
        Vector3 forward = orientation * Vector3.forward;

        Vector3 r = right * halfExtents.x;
        Vector3 u = up * halfExtents.y;
        Vector3 f = forward * halfExtents.z;

        Vector3[] v = new Vector3[8];
        v[0] = center - r - u - f;
        v[1] = center + r - u - f;
        v[2] = center + r - u + f;
        v[3] = center - r - u + f;
        v[4] = center - r + u - f;
        v[5] = center + r + u - f;
        v[6] = center + r + u + f;
        v[7] = center - r + u + f;

        // base
        Debug.DrawLine(v[0], v[1], color, duration);
        Debug.DrawLine(v[1], v[2], color, duration);
        Debug.DrawLine(v[2], v[3], color, duration);
        Debug.DrawLine(v[3], v[0], color, duration);

        // top
        Debug.DrawLine(v[4], v[5], color, duration);
        Debug.DrawLine(v[5], v[6], color, duration);
        Debug.DrawLine(v[6], v[7], color, duration);
        Debug.DrawLine(v[7], v[4], color, duration);

        // verticals
        Debug.DrawLine(v[0], v[4], color, duration);
        Debug.DrawLine(v[1], v[5], color, duration);
        Debug.DrawLine(v[2], v[6], color, duration);
        Debug.DrawLine(v[3], v[7], color, duration);
    }
}
