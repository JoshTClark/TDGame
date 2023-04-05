using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float hp = 2f;
    public float speed = 1.5f;
    private float speedReductionMagicNumber = 1000;
    public float nodeRange = 0.1f;

    [HideInInspector]
    public GameObject[] path;
    [HideInInspector]
    public int pathPosition;

    void Update()
    {
        if(hp <= 0)
        {
            Destroy(gameObject);
        }

        this.transform.position += GetDirectionVectorNormalized(path[pathPosition + 1]) * speed / speedReductionMagicNumber;
        if (GetDistance(path[pathPosition + 1]) <= nodeRange)
        {
            pathPosition++;
            if (pathPosition == path.Length)
            {
                pathPosition--;
            }
        }
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
}