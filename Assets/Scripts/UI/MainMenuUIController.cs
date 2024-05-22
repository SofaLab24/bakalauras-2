using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MainMenuUIController : MonoBehaviour
{
    UIDocument mainOverlay;

    Label startGame;
    Label unlockTowers;
    Label achievements;
    Label exitGame;

    VisualElement buttonsWrapper;
    [SerializeField]
    VisualTreeAsset unlockTowerScreenTemplate;
    VisualElement unlockTowerScreen;
    Label unlockTowerScreenBackButton;

    [SerializeField]
    VisualTreeAsset achievementsScreenTemplate;
    VisualElement achievementsScreen;
    Label achievementsBackButton;
    AchievementManager achievementManager;

    Label flameLock;
    Label wizardLock;

    Label highScore;
    const string HIGHSCORE = "HIGHSCORE";

    private void Start()
    {
        mainOverlay = GetComponent<UIDocument>();

        unlockTowerScreen = unlockTowerScreenTemplate.CloneTree();
        unlockTowerScreen.style.width = Length.Percent(100);
        unlockTowerScreen.style.height = Length.Percent(100);

        achievementsScreen = achievementsScreenTemplate.CloneTree();
        achievementsScreen.style.width = Length.Percent(100);
        achievementsScreen.style.height = Length.Percent(100);

        startGame = mainOverlay.rootVisualElement.Q<Label>("Start");
        startGame.RegisterCallback<ClickEvent>(OnStartGame);
        unlockTowers = mainOverlay.rootVisualElement.Q<Label>("UnlockTowers");
        unlockTowers.RegisterCallback<ClickEvent>(OnUnlockTowers);
        achievements = mainOverlay.rootVisualElement.Q<Label>("Achievements");
        achievements.RegisterCallback<ClickEvent>(OnAchievemnts);
        exitGame = mainOverlay.rootVisualElement.Q<Label>("Exit");
        exitGame.RegisterCallback<ClickEvent>(OnExitGame);
        buttonsWrapper = mainOverlay.rootVisualElement.Q<VisualElement>("Buttons");
        

        unlockTowerScreenBackButton = unlockTowerScreen.Q<Label>("Back");
        unlockTowerScreenBackButton.RegisterCallback<ClickEvent>(OnBackToMainMenu);

        achievementsBackButton = achievementsScreen.Q<Label>("Back");
        achievementsBackButton.RegisterCallback<ClickEvent>(OnBackToMainMenu);
        achievementManager = GetComponent<AchievementManager>();

        highScore = mainOverlay.rootVisualElement.Q<Label>("Highscore");
        int currentHighScore = PlayerPrefs.GetInt(HIGHSCORE);
        highScore.text = "Highscore: " + currentHighScore;

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
        PlayerPrefs.SetInt(HIGHSCORE, 22);
        if (PlayerPrefs.GetInt(HIGHSCORE) >= 20)
        {
            flameLock.style.visibility = Visibility.Hidden;
            if (PlayerPrefs.GetInt(HIGHSCORE) >= 60)
            {
                wizardLock.style.visibility = Visibility.Hidden;
            }
        }
    }
    void OnAchievemnts(ClickEvent evt)
    {
        buttonsWrapper.Clear();
        achievementManager.ConfigureAchievementScreen(achievementsScreen);
        buttonsWrapper.Add(achievementsScreen);
    }
    void OnBackToMainMenu(ClickEvent evt)
    {
        buttonsWrapper.Clear();
        buttonsWrapper.Add(startGame);
        buttonsWrapper.Add(unlockTowers);
        buttonsWrapper.Add(achievements);
        buttonsWrapper.Add(exitGame);
    }
    void OnExitGame(ClickEvent evt)
    {
        Application.Quit();
    }
}
