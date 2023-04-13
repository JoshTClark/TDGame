using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    public float range = 5f;
    public float damage = 1f;

    public float fireCooldown_s = 2.0f;
    private float activeFireCoolDown;

    void Start()
    {
        activeFireCoolDown = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        Aim();

        activeFireCoolDown += Time.deltaTime;
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
}