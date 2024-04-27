using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using static UnityEditor.FilePathAttribute;

public class TowerManager : MonoBehaviour
{
    private string arrowTowerName = "ARROW";
    private string flameTowerName = "FLAME";
    private string wizardTowerName = "WIZARD";

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

    [Header("Combined Towers")]
    [SerializeField]
    GameObject arrowFlameTower;
    [SerializeField]
    private float arrowFlameTowerHeightOffset;
    [SerializeField]
    GameObject arrowWizardTower;
    [SerializeField]
    private float arrowWizardTowerHeightOffset;


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
        if (selectedTower == arrowTowerName && _moneyManager.BuyTower(_moneyManager.arrowTowerCost))
        { 
            towerToPlace = arrowTower;
            offset = arrowTowerHeightOffset;
        }
        else if (selectedTower == flameTowerName && _moneyManager.BuyTower(_moneyManager.flameTowerCost))
        {
            towerToPlace = flameTower;
            offset = flameTowerHeightOffset;
        }
        else if (selectedTower == wizardTowerName && _moneyManager.BuyTower(_moneyManager.wizardTowerCost))
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
    public void CombineTowers(GameObject callObject, string towerType)
    {
        // tower selector
        if (selectedTower == "" || selectedTower == towerType) { return; }

        // Flame + Arrow
        if (((selectedTower == arrowTowerName && towerType == flameTowerName) || (selectedTower == flameTowerName && towerType == arrowTowerName))
            && _moneyManager.BuyTower(selectedTower))
        {
            Vector3 location = callObject.transform.position;
            callObject.GetComponent<TowerCombinationController>().DestroyCurrentTower();

            Instantiate(arrowFlameTower, new Vector3(location.x, location.y + arrowFlameTowerHeightOffset, location.z), arrowFlameTower.transform.rotation);
            _overlayController.DeselectTower();
        } // Wizard + Arrow
        else if (((selectedTower == arrowTowerName && towerType == wizardTowerName) || (selectedTower == wizardTowerName && towerType == arrowTowerName))
            && _moneyManager.BuyTower(selectedTower))
        {
            Vector3 location = callObject.transform.position;
            callObject.GetComponent<TowerCombinationController>().DestroyCurrentTower();

            Instantiate(arrowWizardTower, new Vector3(location.x, location.y + arrowWizardTowerHeightOffset, location.z), arrowWizardTower.transform.rotation);
            _overlayController.DeselectTower();
        }
    }
}
