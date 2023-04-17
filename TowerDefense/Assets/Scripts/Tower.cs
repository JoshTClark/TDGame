using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    public bool isTowerActive;
    public float range = 5f;
    public float damage = 1f;

    public float fireCooldown_s = 2.0f;
    private float activeFireCoolDown;
    public bool showRangeIndicator = false;
    public GameObject rangeIndicator;

    void Start()
    {
        activeFireCoolDown = 0.0f;
        rangeIndicator.transform.localScale = new Vector3(range * 2, range * 2, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        activeFireCoolDown += Time.deltaTime;
        if (isTowerActive)
        {
            Aim();
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
        Enemy target = EnemyManager.instance.GetClosest(this.gameObject.transform.position, range);
        if (target)
        {
            Vector3 rot = target.gameObject.transform.position - this.gameObject.transform.position;
            rot.Normalize();
            this.transform.right = rot;

            if (activeFireCoolDown >= fireCooldown_s)
            {
                activeFireCoolDown = 0.0f;

                Shoot(target);
            }
        }
    }

    private void Shoot(Enemy target)
    {
        //temp: it is autohit right now
        Debug.Log("Shoot");
        target.GetComponent<Enemy>().Damage(damage);
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
}