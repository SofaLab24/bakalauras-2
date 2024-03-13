using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyManager : MonoBehaviour
{
    OverlayController overlayController;
    public int currentMoney = 50;

    public int arrowTowerCost;
    public int flameTowerCost;
    public int wizardTowerCost;

    private void Start()
    {
        overlayController = FindFirstObjectByType<OverlayController>();
    }
    public void AddMoney(int money)
    {
        currentMoney += money;
        overlayController.UpdateMoney(currentMoney);
    }
    public bool BuyTower(int towerCost)
    {
        if (towerCost > currentMoney) return false;
        else
        {
            AddMoney(-towerCost);
            return true;
        }
    }
}
