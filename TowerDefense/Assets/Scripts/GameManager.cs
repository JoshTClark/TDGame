using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private InputAction click;

    private bool placingTower = false;
    private Tower selectedTower = null;

    void Start()
    {
        click.Enable();
        click.performed += (InputAction.CallbackContext context) =>
        {
            if (placingTower)
            {
                selectedTower.showRangeIndicator = false;
                selectedTower.isTowerActive = true;
                selectedTower = null;
                placingTower = false;
            }
            else
            {
            }
        };
    }

    public void BuyTower()
    {
        placingTower = true;
        Tower tower = GameObject.Instantiate<Tower>(Resources.Load<Tower>("Prefabs/Tower"));
        tower.isTowerActive = false;
        tower.showRangeIndicator = true;
        selectedTower = tower;
    }

    private void Update()
    {
        if (placingTower && selectedTower)
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            selectedTower.transform.position = mousePosition;
        }
    }
}
