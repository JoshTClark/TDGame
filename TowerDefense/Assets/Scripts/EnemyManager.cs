using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{

    public float enemyInterval_s = 3;

    public GameObject testEnemyPrefab;

    public GameObject[] path;

    private float timer = 0;
    private float spacingTimer = 0;
    private float spacingTime = 0.2f;
    private float globalTime = 0;
    private int numToSpawn = 1;
    private bool spawnWave = false;

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
        globalTime += Time.deltaTime;
        spacingTimer += Time.deltaTime;

        if (timer >= enemyInterval_s && !spawnWave)
        {
            spawnWave = true;
            numToSpawn = (int)Random.Range(Mathf.Max(globalTime / 10, 1f), Mathf.Max(globalTime / 10 * 1.25f, 1f));
            spacingTime = Random.Range(0.2f, Mathf.Min(1f, 60 / globalTime));
        }

        if (spawnWave && spacingTimer >= spacingTime)
        {
            spacingTimer = 0;
            numToSpawn--;
            GameObject e = Instantiate(testEnemyPrefab);
            e.transform.position = path[0].transform.position;
            e.GetComponent<Enemy>().path = path;
            e.gameObject.GetComponent<Enemy>().maxHp *= Mathf.Max(globalTime / 120 + 1, 1f);
            e.gameObject.GetComponent<Enemy>().speed *= Mathf.Max(globalTime / 60, 1f);
            e.GetComponent<Enemy>().pathPosition = 0;

            enemyList.Add(e.GetComponent<Enemy>());

            if (numToSpawn <= 0)
            {
                spawnWave = false;
                timer = 0;
            }
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

    public Enemy GetFirst(Vector2 pos, float range)
    {
        Enemy closest = null;
        foreach (Enemy e in enemyList)
        {
            if (Vector2.Distance(e.transform.position, pos) <= range)
            {
                closest = e;
                return closest;
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