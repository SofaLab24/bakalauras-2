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

    bool arrowSelected;
    bool flameSelected;
    bool wizardSelected;

    VisualElement towerBar;
    Label moneyCount;

    //TESTING
    PathGenerator pathGenerator;
    Button pathButton;

    private void Start()
    {
        _towerManager = FindFirstObjectByType<TowerManager>();
        mainOverlay = GetComponent<UIDocument>();
        mainOverlay.rootVisualElement.Q<VisualElement>("RootElement").pickingMode = PickingMode.Ignore;
        mainOverlay.rootVisualElement.Q<VisualElement>("Top").pickingMode = PickingMode.Ignore;
        mainOverlay.rootVisualElement.Q<VisualElement>("Mid").pickingMode = PickingMode.Ignore;
        mainOverlay.rootVisualElement.Q<VisualElement>("Bot").pickingMode = PickingMode.Ignore;

        mainOverlay.rootVisualElement.Q<VisualElement>("TopLeft").pickingMode = PickingMode.Ignore;
        mainOverlay.rootVisualElement.Q<VisualElement>("TopMid").pickingMode = PickingMode.Ignore;
        mainOverlay.rootVisualElement.Q<VisualElement>("TopRight").pickingMode = PickingMode.Ignore;
        mainOverlay.rootVisualElement.Q<VisualElement>("MoneyTab").pickingMode = PickingMode.Ignore;
        mainOverlay.rootVisualElement.Q<VisualElement>("CoinIcon").pickingMode = PickingMode.Ignore;
        mainOverlay.rootVisualElement.Q<VisualElement>("MoneyCount").pickingMode = PickingMode.Ignore;


        towerBar = mainOverlay.rootVisualElement.Q<VisualElement>("TowerBar");
        towerBar.pickingMode = PickingMode.Ignore;
        moneyCount = mainOverlay.rootVisualElement.Q<Label>("MoneyCount");

        arrowSelected = false;
        flameSelected = false;
        wizardSelected = false;

        // ONLY FOR TESTING
        pathGenerator = FindFirstObjectByType<PathGenerator>();
        pathButton = mainOverlay.rootVisualElement.Q<Button>("TestButton");
        pathButton.RegisterCallback<ClickEvent>(NewPath);

        // Load tower icons
        if (_towerManager.arrowTowerUnlocked)
        {
            VisualElement towerIconElement = towerIconPrefabTree.CloneTree().Q("TowerIcon");
            towerIconElement.name = "ARROW";
            towerIconElement.style.backgroundImage = arrowTowerIcon;
            towerIconElement.RegisterCallback<ClickEvent, string>(SelectTower, "ARROW");

            towerBar.Add(towerIconElement);
            Debug.Log("Add Arrow Tower");
        }
        if (_towerManager.flameTowerUnlocked)
        {
            VisualElement towerIconElement = towerIconPrefabTree.CloneTree().Q("TowerIcon");
            towerIconElement.name = "FLAME";
            towerIconElement.style.backgroundImage = flameTowerIcon;
            towerIconElement.RegisterCallback<ClickEvent, string>(SelectTower, "FLAME");

            towerBar.Add(towerIconElement);
            Debug.Log("Add Flame Tower");
        }
        if (_towerManager.wizardTowerUnlocked)
        {
            VisualElement towerIconElement = towerIconPrefabTree.CloneTree().Q("TowerIcon");
            towerIconElement.name = "WIZARD";
            towerIconElement.style.backgroundImage = wizardTowerIcon;
            towerIconElement.RegisterCallback<ClickEvent, string>(SelectTower, "WIZARD");
            towerBar.Add(towerIconElement);
            Debug.Log("Add Wizard Tower");
        }

        // THIS NEEDS TO BE REWORKED SOMEHOW
        UpdateMoney(50);
    }
    private void NewPath(ClickEvent evt)
    {
        pathGenerator.GeneratePaths();
    }
    private void SelectTower(ClickEvent evt, string tower)
    {
        if (tower == "ARROW")
        {
            arrowSelected = true;
            flameSelected = false;
            wizardSelected = false;
            RemoveBorder("FLAME");
            RemoveBorder("WIZARD");
            AddBorder(tower);
            _towerManager.selectedTower = tower;
        }
        else if (tower == "FLAME")
        {
            arrowSelected = false;
            flameSelected = true;
            wizardSelected = false;
            RemoveBorder("ARROW");
            RemoveBorder("WIZARD");
            AddBorder(tower);
            _towerManager.selectedTower = tower;
        }
        else if (tower == "WIZARD")
        {
            arrowSelected = false;
            flameSelected = false;
            wizardSelected = true;
            RemoveBorder("ARROW");
            RemoveBorder("FLAME");
            AddBorder(tower);
            _towerManager.selectedTower = tower;
        }
    }
    private void AddBorder(string tower)
    {
        VisualElement cur = towerBar.Q<VisualElement>(tower);
        cur.style.borderBottomColor = cur.style.borderLeftColor = cur.style.borderRightColor = cur.style.borderTopColor = new StyleColor(Color.black);
        cur.style.borderBottomWidth = cur.style.borderLeftWidth = cur.style.borderRightWidth = cur.style.borderTopWidth = 3;
    }
    private void RemoveBorder(string tower)
    {
        VisualElement cur = towerBar.Q<VisualElement>(tower);
        if (cur == null) return; 
        cur.style.borderBottomColor = cur.style.borderLeftColor = cur.style.borderRightColor = cur.style.borderTopColor = new StyleColor(Color.clear);
        cur.style.borderBottomWidth = cur.style.borderLeftWidth = cur.style.borderRightWidth = cur.style.borderTopWidth = 0;
    }
    public void DeselectTower()
    {
        arrowSelected = false;
        flameSelected = false;
        wizardSelected = false;
        RemoveBorder("ARROW");
        RemoveBorder("FLAME");
        RemoveBorder("WIZARD");
        _towerManager.selectedTower = "";
    }
    public void UpdateMoney(int money)
    {
        moneyCount.text = "" + money;
    }
}
