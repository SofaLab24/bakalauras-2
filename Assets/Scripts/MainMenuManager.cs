using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    const string HIGHSCORE = "HIGHSCORE";
    private void Awake()
    {
        if (!PlayerPrefs.HasKey(HIGHSCORE))
        {
            PlayerPrefs.SetInt(HIGHSCORE, 0);
        }
    }
    void Update()
    {
        transform.Rotate(Vector3.up, Time.deltaTime * 3);
    }
    public void SetHighscore(int score)
    {
        PlayerPrefs.SetInt(HIGHSCORE, score);
    }
}
