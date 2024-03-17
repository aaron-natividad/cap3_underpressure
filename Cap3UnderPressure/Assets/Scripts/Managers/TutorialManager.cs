using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : Manager
{
    private SceneHandler sceneHandler;

    protected override void OnAwake()
    {
        sceneHandler = GetComponent<SceneHandler>();
    }

    protected override void StartGame()
    {
        base.StartGame();
        PlayerUI.instance.symptomsBar.DisableBar();
    }

    protected override void EndGame()
    {
        Player.instance.state = PlayerState.Disabled;
        sceneHandler.LoadNextScene();
        base.EndGame();
    }
}
