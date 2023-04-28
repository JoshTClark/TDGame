using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Tower : MonoBehaviour
{
    public bool isTowerActive;
    public int level = 1;
    private int damageLevel = 1;
    private int pierceLevel = 1;
    private int cooldownLevel = 1;
    private int rangeLevel = 1;
    public float baseRange;
    public float baseDamage;
    public int basePierce;
    public float baseCooldown;

    private float activeFireCoolDown;
    public bool showRangeIndicator = false;
    public GameObject rangeIndicator;

    private float baseDamageCost = 5f;
    private float baseRangeCost = 5f;
    private float baseCooldownCost = 5f;
    private float basePierceCost = 10f;


    public float damage, range, cooldown;
    public int pierce;

    public GameObject lineRenderer;

    void Start()
    {
        damage = baseDamage;
        pierce = basePierce;
        cooldown = baseCooldown;
        range = baseRange;
        activeFireCoolDown = 0.0f;
        rangeIndicator.transform.localScale = new Vector3(range * 2, range * 2, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        activeFireCoolDown += Time.deltaTime;
        if (isTowerActive)
        {
            if (activeFireCoolDown >= cooldown)
            {
                Aim();
            }
            activeFireCoolDown += Time.deltaTime;
        }
        else
        {
        }

        if (showRangeIndicator)
        {
            rangeIndicator.SetActive(true);
        }
        else
        {
            rangeIndicator.SetActive(false);
        }
    }

    private void Aim()
    {
        Enemy target = EnemyManager.instance.GetFirst(this.gameObject.transform.position, range);
        if (target)
        {
            activeFireCoolDown = 0.0f;
            Vector3 rot = target.gameObject.transform.position - this.gameObject.transform.position;
            rot.Normalize();
            this.transform.right = rot;
            Shoot(target);
        }
    }

    private void Shoot(Enemy target)
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll(this.gameObject.transform.position, this.transform.right, 20f, LayerMask.GetMask("Enemy"));
        GameObject lineHit = Instantiate(lineRenderer);
        lineHit.transform.parent = gameObject.transform;
        LineRenderer renderer = lineHit.GetComponent<LineRenderer>();
        renderer.startWidth = 0.15f;
        renderer.endWidth = 0.15f;
        Vector3[] positions = new Vector3[2];
        positions[0] = this.gameObject.transform.position;
        if (hits.Length < pierce)
        {
            foreach (RaycastHit2D hit in hits)
            {
                hit.collider.gameObject.GetComponent<Enemy>().Damage(damage);
            }
            positions[1] = this.gameObject.transform.position + (this.transform.right * 20f);
        }
        else
        {
            for (int i = 0; i < pierce; i++)
            {
                hits[i].collider.gameObject.GetComponent<Enemy>().Damage(damage);
            }
            positions[1] = this.gameObject.transform.position + (this.transform.right * hits[pierce - 1].distance);
        }

        renderer.SetPositions(positions);
    }

    public bool IsTowerPlaceable()
    {
        Collider2D[] colliders = new Collider2D[5];
        Physics2D.OverlapCollider(this.gameObject.GetComponent<Collider2D>(), new ContactFilter2D(), colliders);

        foreach (Collider2D i in colliders)
        {
            if (i && i.gameObject && i.gameObject.GetComponent<Tower>())
            {
                return false;
            }
        }
        return true;
    }

    public float GetDamageCost()
    {
        return baseDamageCost * (1 + Mathf.Pow(damageLevel-1, 1.2f) + (level - 1) * 0.2f);
    }

    public float GetRangeCost()
    {
        return baseRangeCost * (1 + (rangeLevel - 1) * 0.5f + (level - 1) * 0.1f);
    }

    public float GetPierceCost()
    {
        return basePierceCost * (1 + (pierceLevel - 1) + (level - 1) * 0.5f);
    }

    public float GetCooldownCost()
    {
        return baseCooldownCost * (1 + (cooldownLevel - 1) * 0.5f + (level - 1) * 0.1f);
    }

    public void UpgradeDamage()
    {
        damageLevel++;
        damage *= 1.1f;
        level++;
    }

    public void UpgradePierce()
    {
        pierceLevel++;
        pierce++;
        level++;
    }

    public void UpgradeCooldown()
    {
        cooldownLevel++;
        cooldown /= 1.1f;
        level++;
    }

    public void UpgradeRange()
    {
        rangeLevel++;
        range += 0.35f;
        level++;
        rangeIndicator.transform.localScale = new Vector3(range * 2, range * 2, 1f);
    }
}