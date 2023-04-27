using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastController : MonoBehaviour
{

    private float lifeTimer_s;

    // Start is called before the first frame update
    void Start()
    {
        lifeTimer_s = 0.1f;
    }

    // Update is called once per frame
    void Update()
    {
        lifeTimer_s -= Time.deltaTime;
        if (lifeTimer_s <= 0)
        {
            Destroy(this.gameObject);
        }
    }
}
