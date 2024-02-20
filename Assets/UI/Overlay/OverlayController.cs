using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class OverlayController : MonoBehaviour
{
    // TODO: Add tower scriptable objects and create the TowerController.cs from BasicTowerController.cs accordingly
    TowerManager _towerManager;
    [SerializeField]
    Texture2D arrowTowerIcon;
    [SerializeField]
    Texture2D flameTowerIcon;
    [SerializeField]
    Texture2D wizardTowerIcon;

    UIDocument mainOverlay;
    [SerializeField]
    VisualTreeAsset towerIconPrefabTree;
    

    private void Start()
    {
        _towerManager = FindFirstObjectByType<TowerManager>();
        mainOverlay = GetComponent<UIDocument>();

        VisualElement towerBar = mainOverlay.rootVisualElement.Q<VisualElement>("TowerBar");

        if (_towerManager.arrowTowerUnlocked)
        {
            VisualElement towerIconElement = towerIconPrefabTree.CloneTree().Q("TowerIcon");
            towerIconElement.style.backgroundImage = arrowTowerIcon;
            
            towerBar.Add(towerIconElement);
            Debug.Log("Add Arrow Tower");
        }
        if (_towerManager.flameTowerUnlocked)
        {
            VisualElement towerIconElement = towerIconPrefabTree.CloneTree().Q("TowerIcon");
            towerIconElement.style.backgroundImage = flameTowerIcon;
            towerBar.Add(towerIconElement);
            Debug.Log("Add Flame Tower");
        }
        if (_towerManager.wizardTowerUnlocked)
        {
            VisualElement towerIconElement = towerIconPrefabTree.CloneTree().Q("TowerIcon");
            towerIconElement.style.backgroundImage = wizardTowerIcon;
            towerBar.Add(towerIconElement);
            Debug.Log("Add Wizard Tower");
        }
    }
}
