using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private InputAction click;

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
        
    }
}
