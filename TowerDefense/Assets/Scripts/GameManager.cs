using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private InputAction click;

    private float timer = 0;

    void Start()
    {
        click.Enable();
        click.performed += PlaceTower;
    }

    private void PlaceTower(InputAction.CallbackContext obj)
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());

        Tower tower = GameObject.Instantiate<Tower>(Resources.Load<Tower>("Prefabs/Tower"));
        tower.transform.position = mousePosition;

    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= 3)
        {
            timer = 0;
            Enemy e = GameObject.Instantiate<Enemy>(Resources.Load<Enemy>("Prefabs/Enemy"));
            e.transform.position = new Vector2(-10, 0);
        }
    }
}
