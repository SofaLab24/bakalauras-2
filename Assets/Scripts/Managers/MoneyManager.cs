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
    public bool BuyTower(string towerName)
    {
        if (towerName == "ARROW")
        {
            return BuyTower(arrowTowerCost);
        }
        else if (towerName == "FLAME")
        {
            return BuyTower(flameTowerCost);
        }
        else if (towerName == "WIZARD")
        {
            return BuyTower(wizardTowerCost);
        }
        else
        {
            return false;
        }

    }
}
