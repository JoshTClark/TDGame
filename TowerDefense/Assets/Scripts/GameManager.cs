using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class GameManager : MonoBehaviour
{
    private enum ActionState
    {
        None,
        PlacingTower,
        TowerSelected
    }

    [SerializeField]
    private InputActionReference select, deselect;

    [SerializeField]
    private TMP_Text moneyLabel;
    [SerializeField]
    private TMP_Text healthLabel;

    private ActionState state = ActionState.None;
    private Tower selectedTower = null;
    private bool canPlaceTower = false;

    private int health = 10;

    [HideInInspector]
    public int money;
    private int towerCost = 10;

    void Start()
    {
        select.action.Enable();
        select.action.performed += SelectAction;

        deselect.action.Enable();
        deselect.action.performed += DeselctAction;

        money = 20;
    }

    public void DealPlayerDamage(int p_Damage) {
        health -= p_Damage;
    }

    public void GiveMoney(int p_Money) {
        money += p_Money;
    }

    public void BuyTower()
    {
        if (money < towerCost) {
            return;
        }
        state = ActionState.PlacingTower;
        Tower tower = GameObject.Instantiate<Tower>(Resources.Load<Tower>("Prefabs/Tower"));
        tower.isTowerActive = false;
        tower.showRangeIndicator = true;
        selectedTower = tower;
        money -= towerCost;
    }

    private void Update()
    {
        switch (state)
        {
            case ActionState.None:
                // Do nothing
                break;
            case ActionState.PlacingTower:
                if (selectedTower)
                {
                    Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
                    selectedTower.transform.position = mousePosition;
                    if (!selectedTower.IsTowerPlaceable())
                    {
                        selectedTower.rangeIndicator.GetComponent<SpriteRenderer>().color = new Color(1f, 0f, 0f, 0.3f);
                        canPlaceTower = false;
                    }
                    else 
                    {
                        selectedTower.rangeIndicator.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0.3f);
                        canPlaceTower = true;
                    }
                }
                break;
            case ActionState.TowerSelected:
                if (selectedTower)
                {
                    selectedTower.showRangeIndicator = true;
                }
                break;
        }

        moneyLabel.text = "Money: " + money;
        healthLabel.text = "Health: " + health;
    }

    private void SelectAction(InputAction.CallbackContext context)
    {
        switch (state)
        {
            case ActionState.None:
                SelectTower();
                break;
            case ActionState.TowerSelected:
                SelectTower();
                break;
            case ActionState.PlacingTower:
                if (canPlaceTower)
                {
                    selectedTower.showRangeIndicator = false;
                    selectedTower.isTowerActive = true;
                    selectedTower = null;
                    state = ActionState.None;
                }
                break;
        }
    }

    private void DeselctAction(InputAction.CallbackContext context)
    {
        switch (state)
        {
            case ActionState.PlacingTower:
                Destroy(selectedTower.gameObject);
                state = ActionState.None;
                break;
            case ActionState.TowerSelected:
                selectedTower.showRangeIndicator = false;
                selectedTower = null;
                state = ActionState.None;
                break;
        }
    }

    private void SelectTower()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

        RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero, 10f, LayerMask.GetMask("Tower"));
        if (hit.collider != null)
        {
            selectedTower = hit.collider.gameObject.GetComponent<Tower>();
            state = ActionState.TowerSelected;
            selectedTower.showRangeIndicator = true;
        }
    }
}
