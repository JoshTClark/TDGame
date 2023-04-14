using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{

    public float enemyInterval_s = 3;

    public GameObject testEnemyPrefab;

    public GameObject[] path;

    private float timer = 0;

    private List<Enemy> enemyList = new List<Enemy>();

    public static EnemyManager instance;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= enemyInterval_s)
        {
            timer = 0;
            GameObject e = Instantiate(testEnemyPrefab);
            e.transform.position = path[0].transform.position;
            e.GetComponent<Enemy>().path = path;
            e.GetComponent<Enemy>().pathPosition = 0;
            enemyList.Add(e.GetComponent<Enemy>());
        }

        for (int i = enemyList.Count - 1; i >= 0; i--)
        {
            Enemy e = enemyList[i];
            if (!e)
            {
                enemyList.Remove(e);
            }
        }
    }

    public Enemy GetClosest(Vector2 pos)
    {
        Enemy closest = null;
        foreach (Enemy e in enemyList)
        {
            if (!closest)
            {
                closest = e;
            }
            else
            {
                if (Vector2.Distance(pos, e.gameObject.transform.position) < Vector2.Distance(closest.gameObject.transform.position, pos))
                {
                    closest = e;
                }
            }
        }

        return closest;
    }

    public Enemy GetClosest(Vector2 pos, float range)
    {
        Enemy closest = GetClosest(pos);
        if (closest && Vector2.Distance(pos, closest.gameObject.transform.position) >= range)
        {
            closest = null;
        }
        return closest;
    }
}