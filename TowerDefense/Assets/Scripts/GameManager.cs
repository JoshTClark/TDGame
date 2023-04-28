using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using System;

public class GameManager : MonoBehaviour
{
    private enum ActionState
    {
        None,
        PlacingTower,
        TowerSelected
    }

    [SerializeField]
    private InputActionReference select, deselect, increaseTime, increaseMoney;

    [SerializeField]
    private CanvasRenderer bottomBar, upgradeBar, normalBar;

    [SerializeField]
    private TMP_Text moneyLabel, healthLabel, timeLabel, damageLabel, pierceLabel, rangeLabel, cooldownLabel, levelLabel, damageLabelButton, pierceLabelButton, rangeLabelButton, cooldownLabelButton;

    private ActionState state = ActionState.None;
    private Tower selectedTower = null;
    private bool canPlaceTower = false;

    private int health = 10;

    [HideInInspector]
    public float money;
    private int towerCost = 10;

    void Start()
    {
        select.action.Enable();
        select.action.performed += SelectAction;

        deselect.action.Enable();
        deselect.action.performed += DeselctAction;

        increaseTime.action.Enable();
        increaseTime.action.performed += (InputAction.CallbackContext context) =>
        {
            EnemyManager.instance.globalTime += 30f;
        };

        increaseMoney.action.Enable();
        increaseMoney.action.performed += (InputAction.CallbackContext context) =>
        {
            money += 50;
        };

        money = 20;
    }

    public void DealPlayerDamage(int p_Damage)
    {
        health -= p_Damage;
    }

    public void GiveMoney(float p_Money)
    {
        money += p_Money;
    }

    public void BuyTower()
    {
        if (money < towerCost)
        {
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
        timeLabel.text = TimeSpan.FromSeconds(EnemyManager.instance.globalTime).ToString("mm\\:ss");
        switch (state)
        {
            case ActionState.None:
                // Do nothing
                bottomBar.gameObject.SetActive(true);
                upgradeBar.gameObject.SetActive(false);
                normalBar.gameObject.SetActive(true);
                break;
            case ActionState.PlacingTower:
                bottomBar.gameObject.SetActive(false);
                upgradeBar.gameObject.SetActive(false);
                normalBar.gameObject.SetActive(false);
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
                bottomBar.gameObject.SetActive(true);
                upgradeBar.gameObject.SetActive(true);
                normalBar.gameObject.SetActive(false);
                if (selectedTower)
                {
                    selectedTower.showRangeIndicator = true;
                }
                break;
        }

        moneyLabel.text = string.Format("{0:C1}", money);
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

        if (selectedTower)
        {
            selectedTower.showRangeIndicator = false;
        }

        RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero, 10f, LayerMask.GetMask("Tower"));
        if (hit.collider != null)
        {
            selectedTower = hit.collider.gameObject.GetComponent<Tower>();
            state = ActionState.TowerSelected;
            selectedTower.showRangeIndicator = true;
            UpdateLabels();
        }
    }

    private void UpdateLabels()
    {
        damageLabel.text = string.Format("Damage: {0:0.0}", selectedTower.damage);
        pierceLabel.text = string.Format("Pierce: {0:0}", selectedTower.pierce);
        cooldownLabel.text = string.Format("Cooldown: {0:0.0}", selectedTower.cooldown);
        rangeLabel.text = string.Format("Range: {0:0.0}", selectedTower.range);
        levelLabel.text = "Level\n" + selectedTower.level;
        damageLabelButton.text = string.Format("{0:C1}", selectedTower.GetDamageCost());
        pierceLabelButton.text = string.Format("{0:C1}", selectedTower.GetPierceCost());
        cooldownLabelButton.text = string.Format("{0:C1}", selectedTower.GetCooldownCost());
        rangeLabelButton.text = string.Format("{0:C1}", selectedTower.GetRangeCost());
    }

    public void BuyDamage()
    {
        if (money >= selectedTower.GetDamageCost())
        {
            money -= selectedTower.GetDamageCost();
            selectedTower.UpgradeDamage();
            UpdateLabels();
        }
    }

    public void BuyPierce()
    {
        if (money >= selectedTower.GetPierceCost())
        {
            money -= selectedTower.GetPierceCost();
            selectedTower.UpgradePierce();
            UpdateLabels();
        }
    }

    public void BuyRange()
    {
        if (money >= selectedTower.GetRangeCost())
        {
            money -= selectedTower.GetRangeCost();
            selectedTower.UpgradeRange();
            UpdateLabels();
        }
    }

    public void BuyCooldown()
    {
        if (money >= selectedTower.GetCooldownCost())
        {
            money -= selectedTower.GetCooldownCost();
            selectedTower.UpgradeCooldown();
            UpdateLabels();
        }
    }
}
