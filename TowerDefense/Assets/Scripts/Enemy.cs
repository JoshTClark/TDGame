using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private float hp = 2f;
    public float maxHp = 2f;
    public float speed = 1.5f;
    public float nodeRange = 0.1f;

    [HideInInspector]
    public GameObject[] path;
    [HideInInspector]
    public int pathPosition;

    [SerializeField]
    private GameObject healthBar;

    private void Start()
    {
        hp = maxHp;
    }

    void Update()
    {
        if(hp <= 0)
        {
            Destroy(gameObject);
        }

        this.transform.position += GetDirectionVectorNormalized(path[pathPosition + 1]) * speed * Time.deltaTime;
        if (GetDistance(path[pathPosition + 1]) <= nodeRange)
        {
            pathPosition++;
            if (pathPosition == path.Length)
            {
                pathPosition--;
            }
        }

        healthBar.transform.localScale = new Vector3(hp / maxHp, healthBar.transform.localScale.y, healthBar.transform.localScale.z);
    }

    Vector3 GetDirectionVectorNormalized(GameObject target)
    {
        Vector3 direction = target.transform.position - gameObject.transform.position;
        Vector3 directionNormalized = direction.normalized;
        return directionNormalized;
    }

    float GetDistance(GameObject target)
    {
        Vector3 distanceVector = target.transform.position - gameObject.transform.position;
        float distance = distanceVector.magnitude;
        return distance;
    }

    public void Damage(float amountOfDamage)
    {
        hp -= amountOfDamage;
    }
}