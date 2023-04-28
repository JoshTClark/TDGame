using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private float hp = 2f;
    public float maxHp = 2f;
    public float speed = 1.5f;
    public float nodeRange = 0.1f;
    public float worth = 1f;
    public float distanceTraveled = 0f;

    [SerializeField]
    private SpriteRenderer spriteRenderer;

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
        if (hp <= 0)
        {
            Destroy(gameObject);
            GameObject.Find("GameManager").GetComponent<GameManager>().GiveMoney(worth);
        }

        if (pathPosition + 1 < path.Length)
        {
            this.transform.position += GetDirectionVectorNormalized(path[pathPosition + 1]) * speed * Time.deltaTime;
            distanceTraveled += (GetDirectionVectorNormalized(path[pathPosition + 1]) * speed * Time.deltaTime).magnitude;

            spriteRenderer.gameObject.transform.right = GetDirectionVectorNormalized(path[pathPosition + 1]);
            if (GetDistance(path[pathPosition + 1]) <= nodeRange)
            {
                pathPosition++;
                if (pathPosition >= path.Length - 1)
                {
                    GameObject.Find("GameManager").GetComponent<GameManager>().DealPlayerDamage(1);
                    this.hp = 0;
                }
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

    public void SetColorAndSize(EnemyManager.Wave.EnemyType type)
    {
        switch (type) {
            case EnemyManager.Wave.EnemyType.Fast:
                spriteRenderer.color = new Color(0f, 1f, 1f, 1f);
                break;
            case EnemyManager.Wave.EnemyType.Big:
                spriteRenderer.color = new Color(0f, 1f, 0.5f, 1f);
                this.gameObject.transform.localScale = new Vector3(0.75f, 0.75f, 1f);
                break;
            case EnemyManager.Wave.EnemyType.Biggest:
                spriteRenderer.color = new Color(1f, 0f, 1f, 1f);
                this.gameObject.transform.localScale = new Vector3(2f, 2f, 1f);
                break;
        }
    }
}