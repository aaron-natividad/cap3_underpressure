using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    [Header("Game UI Components")]
    [SerializeField] private TextMeshProUGUI score;
    [SerializeField] private TextMeshProUGUI requirement;
    [SerializeField] private TextMeshProUGUI timer;

    private CanvasGroup mainGroup;
    private bool isEnabled;

    private void OnEnable()
    {
        Timer.OnTimerSecondsChanged += UpdateTimer;
        ScoreHandler.OnColorRequirementChanged += UpdateColorRequirement;
        ScoreHandler.OnScoreChanged += UpdateScore;
    }

    private void OnDisable()
    {
        Timer.OnTimerSecondsChanged -= UpdateTimer;
        ScoreHandler.OnColorRequirementChanged -= UpdateColorRequirement;
        ScoreHandler.OnScoreChanged -= UpdateScore;
    }

    private void Awake()
    {
        mainGroup = GetComponent<CanvasGroup>();
    }

    public void SetEnabled(bool isEnabled)
    {
        mainGroup.alpha = isEnabled ? 1 : 0;
        this.isEnabled = isEnabled;
    }

    public bool GetEnabled()
    {
        return isEnabled;
    }

    #region Update Methods
    private void UpdateColorRequirement(RobotColor color)
    {
        string[] colorText = {
            "<color=#ffffff>White</color>",
            "<color=#aa3232>Red</color>",
            "<color=#32aa32>Green</color>",
            "<color=#3232aa>Blue</color>"
        };
        requirement.text = "Required Color:\n" + colorText[(int)color];
    }

    private void UpdateScore(int points, int quota)
    {
        score.text = "Bots Delivered:\n" + points.ToString("00") + "/" + quota.ToString("00");
    }

    private void UpdateTimer(int seconds)
    {
        int minutes = seconds / 60;
        int sec = seconds % 60;
        timer.text = minutes.ToString("00") + ":" + sec.ToString("00");
    }
    #endregion
}
