using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float hp = 2f;
    public float speed = 1.5f;

    void Update()
    {
        if(hp <= 0)
            Destroy(gameObject);

        this.transform.position = new Vector2(this.transform.position.x + (speed * Time.deltaTime), this.transform.position.y);
    }
}
