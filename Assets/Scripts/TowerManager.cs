using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class TowerManager : MonoBehaviour
{
    [Header("Arrow Tower")]
    [SerializeField]
    private GameObject arrowTower;
    [SerializeField]
    private float arrowTowerHeightOffset;
    public bool arrowTowerUnlocked;

    [Header("Flame Tower")]
    [SerializeField]
    private GameObject flameTower;
    [SerializeField]
    private float flameTowerHeightOffset;
    public bool flameTowerUnlocked;

    [Header("Wizard Tower")]
    [SerializeField]
    private GameObject wizardTower;
    [SerializeField]
    private float wizardTowerHeightOffset;
    public bool wizardTowerUnlocked;

    public string selectedTower;
    private OverlayController _overlayController;
    private MoneyManager _moneyManager;

    const string HIGHSCORE = "HIGHSCORE";

    private void Awake()
    {
        int currentHighscore = PlayerPrefs.GetInt(HIGHSCORE);
        if (currentHighscore >= 20)
        {
            flameTowerUnlocked = true;
            if (currentHighscore >= 60)
            {
                wizardTowerUnlocked = true;
            }
            else
            {
                wizardTowerUnlocked = false;
            }
        }
        else
        {
            flameTowerUnlocked = false;
            wizardTowerUnlocked = false;
        }
    }
    private void Start()
    {
        _overlayController = FindFirstObjectByType<OverlayController>();
        _moneyManager = FindFirstObjectByType<MoneyManager>();
    }

    public void PlaceTower(GameObject callObject)
    {
        // tower selector
        if (selectedTower == "") { return; }

        float offset;

        GameObject towerToPlace = arrowTower;
        if (selectedTower == "ARROW" && _moneyManager.BuyTower(_moneyManager.arrowTowerCost))
        { 
            towerToPlace = arrowTower;
            offset = arrowTowerHeightOffset;
        }
        else if (selectedTower == "FLAME" && _moneyManager.BuyTower(_moneyManager.flameTowerCost))
        {
            towerToPlace = flameTower;
            offset = flameTowerHeightOffset;
        }
        else if (selectedTower == "WIZARD" && _moneyManager.BuyTower(_moneyManager.wizardTowerCost))
        {
            towerToPlace = wizardTower;
            offset = wizardTowerHeightOffset;
        }
        else { return; }


        // tower placing
        Vector3 location = callObject.transform.position;
        Instantiate(towerToPlace, new Vector3(location.x, location.y + offset, location.z), towerToPlace.transform.rotation);
        _overlayController.DeselectTower();
        Destroy(callObject);
    }
}
