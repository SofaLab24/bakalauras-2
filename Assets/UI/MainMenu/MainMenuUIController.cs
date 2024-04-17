using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MainMenuUIController : MonoBehaviour
{
    UIDocument mainOverlay;

    Label startGame;
    Label unlockTowers;
    Label exitGame;

    VisualElement buttonsWrapper;
    [SerializeField]
    VisualTreeAsset unlockTowerScreenTemplate;
    VisualElement unlockTowerScreen;
    Label unlockTowerScreenBackButton;

    Label flameLock;
    Label wizardLock;

    Label highScore;
    const string HIGHSCORE = "HIGHSCORE";

    MainMenuManager menuManager;

    Button testButton;
    private void Start()
    {
        menuManager = FindFirstObjectByType<MainMenuManager>();
        mainOverlay = GetComponent<UIDocument>();
        unlockTowerScreen = unlockTowerScreenTemplate.CloneTree();
        unlockTowerScreen.style.width = Length.Percent(100);
        unlockTowerScreen.style.height = Length.Percent(100);

        startGame = mainOverlay.rootVisualElement.Q<Label>("Start");
        startGame.RegisterCallback<ClickEvent>(OnStartGame);
        unlockTowers = mainOverlay.rootVisualElement.Q<Label>("UnlockTowers");
        unlockTowers.RegisterCallback<ClickEvent>(OnUnlockTowers);
        exitGame = mainOverlay.rootVisualElement.Q<Label>("Exit");
        exitGame.RegisterCallback<ClickEvent>(OnExitGame);
        buttonsWrapper = mainOverlay.rootVisualElement.Q<VisualElement>("Buttons");
        

        unlockTowerScreenBackButton = unlockTowerScreen.Q<Label>("Back");
        unlockTowerScreenBackButton.RegisterCallback<ClickEvent>(OnUnlockTowersBack);

        highScore = mainOverlay.rootVisualElement.Q<Label>("Highscore");
        int currentHighScore = PlayerPrefs.GetInt(HIGHSCORE);
        highScore.text = "Highscore: " + currentHighScore;

        testButton = mainOverlay.rootVisualElement.Q<Button>("TestButton");
        testButton.RegisterCallback<ClickEvent>(TestMethod);

        flameLock = unlockTowerScreen.Q<Label>("FlameLock");
        wizardLock = unlockTowerScreen.Q<Label>("WizardLock");
    }
    void OnStartGame(ClickEvent evt)
    {
        SceneManager.LoadScene(1);
    }
    void OnUnlockTowers(ClickEvent evt)
    {
        buttonsWrapper.Clear();
        buttonsWrapper.Add(unlockTowerScreen);
        if (PlayerPrefs.GetInt(HIGHSCORE) >= 20)
        {
            flameLock.style.visibility = Visibility.Hidden;
            if (PlayerPrefs.GetInt(HIGHSCORE) >= 60)
            {
                wizardLock.style.visibility = Visibility.Hidden;
            }
        }
    }
    void OnUnlockTowersBack(ClickEvent evt)
    {
        buttonsWrapper.Clear();
        buttonsWrapper.Add(startGame);
        buttonsWrapper.Add(unlockTowers);
        buttonsWrapper.Add(exitGame);
    }
    void OnExitGame(ClickEvent evt)
    {
        Application.Quit();
    }
    void TestMethod(ClickEvent evt)
    {
        menuManager.SetHighscore(100);
        int currentHighScore = PlayerPrefs.GetInt(HIGHSCORE);
        highScore.text = "Highscore: " + currentHighScore;
    }
}
