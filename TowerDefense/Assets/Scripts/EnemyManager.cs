using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{

    public float enemyInterval_s = 10;

    public GameObject testEnemyPrefab;

    public GameObject[] path;

    private float timer = 0;
    private float biggestTimer = 0;
    public float globalTime = 0;

    private List<Enemy> enemyList = new List<Enemy>();

    public static EnemyManager instance;

    private List<Wave> waves = new List<Wave>();

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        biggestTimer += Time.deltaTime;
        globalTime += Time.deltaTime;

        if (timer >= enemyInterval_s)
        {
            CreateWave();
            timer = 0f;
        }

        for (int i = waves.Count - 1; i >= 0; i--)
        {
            waves[i].Update();
            if (!waves[i].isActive)
            {
                waves.RemoveAt(i);
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
        Enemy first = null;
        foreach (Enemy e in enemyList)
        {
            if (Vector2.Distance(e.transform.position, pos) <= range && e.transform.position.x > -10)
            {
                if (!first)
                {
                    first = e;
                }
                else if (e.distanceTraveled > first.distanceTraveled)
                {
                    first = e;
                }
            }
        }

        return first;
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

    private void CreateWave()
    {
        float spacingTime = 0f;
        float spawnValue = (int)Random.Range(Mathf.Max(1f, globalTime * 0.75f * 0.15f), Mathf.Max(1f, globalTime * 1.25f * 0.15f));

        while (spawnValue >= 1)
        {
            float innerValue = spawnValue;

            float density = Random.value;
            if (density < 0.1)
            {
                // Incredibly dense round
                innerValue = Mathf.Min(1f, spawnValue * 0.75f);
                spacingTime = 0.25f;
            }
            else if (density < 0.25)
            {
                // Somewhat dense round
                spacingTime = 0.6f;
            }
            else if (density < 0.75)
            {
                spacingTime = 1f;
            }
            else
            {
                spacingTime = 1.5f;
                innerValue *= 1.25f;
            }

            spacingTime = Mathf.Clamp(spacingTime * (1 - ((int)(globalTime / 30)) * 0.1f), 0.01f, 1.5f);

            Wave.EnemyType type = Wave.EnemyType.Normal;
            float spawnCost = 1f;

            if (innerValue >= 25 && Random.value >= 0.65f & biggestTimer >= 35f)
            {
                biggestTimer = 0f;
                type = Wave.EnemyType.Biggest;
                spawnCost = 25;
            }
            else if (innerValue >= 15 && Random.value >= 0.65f)
            {
                type = Wave.EnemyType.Big;
                spawnCost = 15f;
            }
            else if (innerValue >= 3 && Random.value >= 0.25f)
            {
                spawnCost = 3f;
                type = Wave.EnemyType.Fast;
            }

            int maxSpawn = (int)(innerValue / spawnCost);
            int minSpawn = Mathf.Clamp(maxSpawn / 4, 1, maxSpawn);
            int amountToSpawn = Random.Range(minSpawn, maxSpawn);

            Wave wave = new Wave(spacingTime, amountToSpawn, type);
            waves.Add(wave);
            spawnValue -= amountToSpawn * spawnCost;
        }
    }

    public class Wave
    {
        public enum EnemyType
        {
            Normal,
            Big,
            Fast,
            Biggest
        }

        private float spacingTimer = 0f;
        private float spacingTime = 0f;
        private int toSpawn = 0;
        private EnemyType spawnType;

        public bool isActive = true;

        public Wave(float time, int amount, EnemyType type)
        {
            spacingTime = time;
            toSpawn = amount;
            spawnType = type;

            Debug.Log("Creating wave of " + amount + " with spacing: " + spacingTime);
        }

        public void Update()
        {
            spacingTimer += Time.deltaTime;
            if (spacingTimer >= spacingTime && toSpawn > 0)
            {
                spacingTimer = 0;
                toSpawn--;
                switch (spawnType)
                {
                    case EnemyType.Normal:
                        GameObject e = Instantiate(EnemyManager.instance.testEnemyPrefab);
                        e.gameObject.transform.position = EnemyManager.instance.path[0].transform.position;
                        e.GetComponent<Enemy>().path = EnemyManager.instance.path;
                        e.gameObject.GetComponent<Enemy>().maxHp *= Mathf.Max(EnemyManager.instance.globalTime / 240, 1f);
                        e.gameObject.GetComponent<Enemy>().speed *= Mathf.Max(EnemyManager.instance.globalTime / 240, 1f);
                        e.GetComponent<Enemy>().pathPosition = 0;
                        e.gameObject.GetComponent<Enemy>().worth = 2f;
                        e.gameObject.GetComponent<Enemy>().SetColorAndSize(spawnType);
                        EnemyManager.instance.enemyList.Add(e.GetComponent<Enemy>());
                        break;
                    case EnemyType.Fast:
                        GameObject e2 = Instantiate(EnemyManager.instance.testEnemyPrefab);
                        e2.gameObject.transform.position = EnemyManager.instance.path[0].transform.position;
                        e2.GetComponent<Enemy>().path = EnemyManager.instance.path;
                        e2.gameObject.GetComponent<Enemy>().maxHp *= Mathf.Max(EnemyManager.instance.globalTime / 240, 1f) * 0.75f;
                        e2.gameObject.GetComponent<Enemy>().speed *= Mathf.Max(EnemyManager.instance.globalTime / 240, 1f) * 2f;
                        e2.GetComponent<Enemy>().pathPosition = 0;
                        e2.gameObject.GetComponent<Enemy>().worth = 5f;
                        e2.gameObject.GetComponent<Enemy>().SetColorAndSize(spawnType);
                        EnemyManager.instance.enemyList.Add(e2.GetComponent<Enemy>());
                        break;
                    case EnemyType.Big:
                        GameObject e3 = Instantiate(EnemyManager.instance.testEnemyPrefab);
                        e3.gameObject.transform.position = EnemyManager.instance.path[0].transform.position;
                        e3.GetComponent<Enemy>().path = EnemyManager.instance.path;
                        e3.gameObject.GetComponent<Enemy>().maxHp *= Mathf.Max(EnemyManager.instance.globalTime / 240, 1f) * 20;
                        e3.gameObject.GetComponent<Enemy>().speed *= Mathf.Max(EnemyManager.instance.globalTime / 240, 1f) * 0.45f;
                        e3.GetComponent<Enemy>().pathPosition = 0;
                        e3.gameObject.GetComponent<Enemy>().worth = 20f;
                        e3.gameObject.GetComponent<Enemy>().SetColorAndSize(spawnType);
                        EnemyManager.instance.enemyList.Add(e3.GetComponent<Enemy>());
                        break;
                    case EnemyType.Biggest:
                        GameObject e4 = Instantiate(EnemyManager.instance.testEnemyPrefab);
                        e4.gameObject.transform.position = EnemyManager.instance.path[0].transform.position;
                        e4.GetComponent<Enemy>().path = EnemyManager.instance.path;
                        e4.gameObject.GetComponent<Enemy>().maxHp *= Mathf.Max(EnemyManager.instance.globalTime / 240, 1f) * 300f;
                        e4.gameObject.GetComponent<Enemy>().speed *= Mathf.Max(EnemyManager.instance.globalTime / 240, 1f) * 0.15f;
                        e4.GetComponent<Enemy>().pathPosition = 0;
                        e4.gameObject.GetComponent<Enemy>().worth = 250f;
                        e4.gameObject.GetComponent<Enemy>().SetColorAndSize(spawnType);
                        EnemyManager.instance.enemyList.Add(e4.GetComponent<Enemy>());
                        break;
                    default:
                        GameObject e5 = Instantiate(EnemyManager.instance.testEnemyPrefab);
                        e5.gameObject.transform.position = EnemyManager.instance.path[0].transform.position;
                        e5.GetComponent<Enemy>().path = EnemyManager.instance.path;
                        e5.gameObject.GetComponent<Enemy>().maxHp *= Mathf.Max(EnemyManager.instance.globalTime / 240, 1f);
                        e5.gameObject.GetComponent<Enemy>().speed *= Mathf.Max(EnemyManager.instance.globalTime / 240, 1f);
                        e5.GetComponent<Enemy>().pathPosition = 0;
                        e5.gameObject.GetComponent<Enemy>().worth = 1f * Mathf.Max(EnemyManager.instance.globalTime / 180, 1f);
                        e5.gameObject.GetComponent<Enemy>().SetColorAndSize(spawnType);
                        EnemyManager.instance.enemyList.Add(e5.GetComponent<Enemy>());
                        break;
                }
            }

            if (toSpawn <= 0)
            {
                isActive = false;
            }
        }
    }
}