using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Constants
{
    public static Color[] robotColors = { 
        new Color(1f, 1f, 1f, 1f),
        new Color(0.5f, 0.2f, 0.2f, 1f),
        new Color(0.2f, 0.5f, 0.2f, 1f),
        new Color(0.2f, 0.2f, 0.5f, 1f)
    };

    public const string SCENE_INTRO = "GameIntro";
    public const string SCENE_BURNOUT = "BurnoutIntro";
    public const string SCENE_SYMPTOM = "Symptom Screen";
    public const string SCENE_EVAL = "Evaluation";
    public const string SCENE_WIN = "Win Scene";
    public const string SCENE_LOSE = "Lose Scene";
}
