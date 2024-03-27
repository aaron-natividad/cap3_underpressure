using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Manager
{
    [SerializeField] private string queuedScene; // only used in game manager

    private ScoreHandler score;
    private Timer timer;
    private DialogueHandler dialogue;

    protected override void OnAwake()
    {
        score = GetComponent<ScoreHandler>();
        dialogue = GetComponent<DialogueHandler>();
        timer = GetComponent<Timer>();
    }

    protected override void OnStart()
    {
        DataManager.instance.queuedScene = queuedScene;
    }

    protected override void StartGame()
    {
        score?.Initialize();
        timer?.Initialize();
        base.StartGame();
    }

    protected override void EndGame()
    {
        dialogue.StartCurrentDialogueGroup();
        base.EndGame();
    }
}
