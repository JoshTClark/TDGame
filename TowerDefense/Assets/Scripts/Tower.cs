using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    public float range = 5f;
    public float damage = 5f;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Aim();
    }

    private void Aim()
    {
        Enemy target = EnemyManager.instance.GetClosest(this.gameObject.transform.position, range);
        if (target)
        {
            Vector3 rot = target.gameObject.transform.position - this.gameObject.transform.position;
            rot.Normalize();
            this.transform.right = rot;
        }
    }
}
