using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public class OverlayController : MonoBehaviour
{
    // TODO: Add tower scriptable objects and create the TowerController.cs from BasicTowerController.cs accordingly
    TowerManager _towerManager;
    [Header("Towers")]
    [SerializeField]
    Texture2D arrowTowerIcon;
    [SerializeField]
    Texture2D flameTowerIcon;
    [SerializeField]
    Texture2D wizardTowerIcon;

    UIDocument mainOverlay;
    [SerializeField]
    VisualTreeAsset towerIconPrefabTree;

    [Header("Hearts")]
    BaseController baseController;
    [SerializeField]
    VisualTreeAsset heartIconPrefabTree;
    [SerializeField]
    Texture2D fullHeartIcon;
    [SerializeField]
    Texture2D emptyHeartIcon;
    int currentHealth;
    List<VisualElement> heartIcons;

    bool arrowSelected;
    bool flameSelected;
    bool wizardSelected;

    VisualElement towerBar;
    VisualElement heartBar;
    Label moneyCount;

    public bool isCurrentlyInWave;

    PathGenerator pathGenerator;
    Button nextWaveButton;
    EnemySpawner enemySpawner;

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

        // Hearts overlay
        baseController = FindFirstObjectByType<BaseController>();
        heartBar = mainOverlay.rootVisualElement.Q<VisualElement>("HeartsContainer");
        heartBar.pickingMode = PickingMode.Ignore;

        currentHealth = baseController.maxHealth;
        heartIcons = new List<VisualElement>();
        for (int i = 0; i < currentHealth; i++)
        {
            VisualElement heartIconElement = heartIconPrefabTree.CloneTree().Q("HeartIcon");
            heartIconElement.style.backgroundImage = fullHeartIcon;
            heartIcons.Add(heartIconElement.Q<VisualElement>());
            heartBar.Add(heartIcons[heartIcons.Count - 1]);
        }

        // Money generation
        moneyCount = mainOverlay.rootVisualElement.Q<Label>("MoneyCount");

        arrowSelected = false;
        flameSelected = false;
        wizardSelected = false;

        // Wave generation
        pathGenerator = FindFirstObjectByType<PathGenerator>();
        enemySpawner = FindFirstObjectByType<EnemySpawner>();
        nextWaveButton = mainOverlay.rootVisualElement.Q<Button>("NextWaveButton");
        nextWaveButton.RegisterCallback<ClickEvent>(NewWave);
        isCurrentlyInWave = false;

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
        UpdateMoney(100);
    }
    public void TakeDamage(int damage)
    {
        Debug.Log(damage);
        currentHealth -= damage;
        for (int i = currentHealth; i < heartIcons.Count; i++)
        {
            heartIcons[i].style.backgroundImage = emptyHeartIcon;
        }
    }
    private void NewWave(ClickEvent evt)
    {
        isCurrentlyInWave = true;
        pathGenerator.GeneratePaths();
        enemySpawner.GenerateNextWave();
        nextWaveButton.style.visibility = Visibility.Hidden;
    }
    public void ShowNextWaveButton()
    {
        nextWaveButton.style.visibility = Visibility.Visible;
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
