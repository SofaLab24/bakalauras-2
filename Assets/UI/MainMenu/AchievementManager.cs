using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.Rendering.DebugUI;

public class AchievementManager : MonoBehaviour 
{
    [SerializeField]
    List<AchievementInfo> achievements;

    [SerializeField]
    Texture2D incompleteAchievement;

    int GetAchievementCompletionPercent(AchievementInfo achievement)
    {
        int progress = PlayerPrefs.GetInt(achievement.Name, 0);
        return Mathf.Clamp(Mathf.FloorToInt(100 * ((progress * 1f) / achievement.TargetValue)), 0, 100);
    }
    public void ConfigureAchievementScreen(VisualElement achievementScreen)
    {
        foreach (var achievementInfo in achievements)
        {
            VisualElement achievement = achievementScreen.Q<VisualElement>(achievementInfo.name);
            int completionPercent = GetAchievementCompletionPercent(achievementInfo);
            if (completionPercent < 100) // achievement is not yet complete
            {
                achievement.Q<VisualElement>("Image").style.backgroundImage = incompleteAchievement;
                achievement.Q<ProgressBar>("Progress").style.visibility = Visibility.Visible;
                ProgressBar bar = achievement.Q<ProgressBar>("Progress");
                bar.value = completionPercent;
                bar.title = $"{PlayerPrefs.GetInt(achievementInfo.Name)} / {achievementInfo.TargetValue}";
            }
            else // achievement is reached
            {
                achievement.Q<VisualElement>("Image").style.backgroundImage = achievementInfo.Image;
                achievement.Q<ProgressBar>("Progress").style.visibility = Visibility.Hidden;
            }
        }
    }

    public void TestFunction(int value)
    {
        foreach (var achievementInfo in achievements)
        {
            PlayerPrefs.SetInt(achievementInfo.name, value);
        }
    }
}
