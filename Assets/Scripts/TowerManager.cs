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

    private void Start()
    {
        _overlayController = FindFirstObjectByType<OverlayController>();
    }

    public void PlaceTower(GameObject callObject)
    {
        // tower selector
        if (selectedTower == "") { return; }

        GameObject towerToPlace = arrowTower;
        if (selectedTower == "ARROW")
        { towerToPlace = arrowTower; }
        else if (selectedTower == "FLAME")
        { towerToPlace = flameTower; }
        else if (selectedTower == "WIZARD")
        { towerToPlace = wizardTower; }

        // tower placing
        Vector3 location = callObject.transform.position;
        Instantiate(towerToPlace, new Vector3(location.x, location.y + arrowTowerHeightOffset, location.z), Quaternion.identity);
        _overlayController.DeselectTower();
        Destroy(callObject);
    }
}
