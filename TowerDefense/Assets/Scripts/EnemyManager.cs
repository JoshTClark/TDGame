using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{

    public float enemyInterval_s = 3;

    public GameObject testEnemyPrefab;

    public GameObject[] path;

    private float timer = 0;

    // Start is called before the first frame update
    void Start()
    {
        
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
        }
    }
}