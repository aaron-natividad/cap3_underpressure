using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ScoreHandler : MonoBehaviour
{
    public static Action<RobotColor> OnColorRequirementChanged;
    public static Action<int, int> OnScoreChanged;

    [Header("Parameters")]
    [SerializeField] private int quota;
    [Space(10)]
    [SerializeField] private RobotColor fixedColor;
    [SerializeField] private bool fixedColorRequirement = false;

    private RobotColor requiredColor;
    private int score = 0;

    private void OnEnable()
    {
        SubmissionCounter.OnRobotEvaluated += ReceiveEvaluation;
    }

    private void OnDisable()
    {
        SubmissionCounter.OnRobotEvaluated -= ReceiveEvaluation;
    }

    public void Initialize()
    {
        GetColorRequirement();
        OnScoreChanged?.Invoke(score, quota);
    }

    private void GetColorRequirement()
    {
        requiredColor = fixedColorRequirement ? fixedColor : (RobotColor)(Random.Range(0, 3) + 1);
        OnColorRequirementChanged?.Invoke(requiredColor);
    }

    private void AddPoint()
    {
        score++;
        OnScoreChanged?.Invoke(score, quota);
    }

    private void ReceiveEvaluation(bool isCorrect)
    {
        if (!isCorrect) return;
        AddPoint();
        GetColorRequirement();
    }
}
