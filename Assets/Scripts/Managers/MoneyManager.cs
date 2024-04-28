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

    [Range(1f, 2f)]
    public float costInflation;

    static string ARROW_NAME = "ARROW";
    static string FLAME_NAME = "FLAME";
    static string WIZARD_NAME = "WIZARD";

    private void Start()
    {
        overlayController = FindFirstObjectByType<OverlayController>();
        overlayController.UpdateMoney(currentMoney);

        overlayController.UpdateTowerCost(ARROW_NAME, arrowTowerCost);
        overlayController.UpdateTowerCost(FLAME_NAME, flameTowerCost);
        overlayController.UpdateTowerCost(WIZARD_NAME, wizardTowerCost);
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
        if (towerName == ARROW_NAME)
        {
            if(BuyTower(arrowTowerCost))
            {
                Debug.Log("Price before inflation: " + arrowTowerCost);
                arrowTowerCost = Mathf.FloorToInt(arrowTowerCost * costInflation);
                Debug.Log("Price after inflation: " + arrowTowerCost);

                overlayController.UpdateTowerCost(ARROW_NAME, arrowTowerCost);

                return true;
            }
            else
            {
                return false;
            }
        }
        else if (towerName == FLAME_NAME)
        {
            if (BuyTower(flameTowerCost))
            {
                flameTowerCost = Mathf.FloorToInt(flameTowerCost * costInflation);
                overlayController.UpdateTowerCost(FLAME_NAME, flameTowerCost);

                return true;
            }
            else
            {
                return false;
            }
        }
        else if (towerName == WIZARD_NAME)
        {
            if (BuyTower(wizardTowerCost))
            {
                wizardTowerCost = Mathf.FloorToInt(wizardTowerCost * costInflation);
                overlayController.UpdateTowerCost(WIZARD_NAME, wizardTowerCost);

                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }

    }
}
