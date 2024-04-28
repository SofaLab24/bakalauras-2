using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Achievement", menuName = "Achievements/GenerateNewAchievement", order = 1)]
public class AchievementInfo : ScriptableObject
{
    public string Name;
    public Texture2D Image;
    public int TargetValue;
}
