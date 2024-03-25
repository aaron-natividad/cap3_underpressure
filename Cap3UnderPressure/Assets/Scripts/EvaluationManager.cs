using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvaluationManager : Manager
{
    [SerializeField] private EvalElevator elevator;

    [Header("Screens")]
    [SerializeField] private ScreenUI tv;

    [Header("Dialogue")]
    [SerializeField] private DialogueGroup winGroup;
    [SerializeField] private DialogueGroup loseGroup;

    private SceneHandler sceneHandler;
    private DialogueHandler dialogueHandler;
    private bool passed;

    protected override void OnAwake()
    {
        sceneHandler = GetComponent<SceneHandler>();
        dialogueHandler = GetComponent<DialogueHandler>();
        base.OnAwake();
    }

    protected override void AddListeners()
    {
        DialogueHandler.OnDialogueGroupEnd += ReceiveGroup;
        base.AddListeners();
    }

    protected override void RemoveListeners()
    {
        DialogueHandler.OnDialogueGroupEnd -= ReceiveGroup;
        base.RemoveListeners();
    }

    protected override void StartGame()
    {
        base.StartGame();
        PlayerUI.instance.symptomsBar.DisableBar();
        passed = DataManager.instance.EvaluateQuota();
        StartCoroutine(CO_StartGame());
    }

    protected override void EndGame()
    {
        Player.instance.state = PlayerState.Disabled;
        StartCoroutine(CO_EndGame());
        base.EndGame();
    }

    private void ReceiveGroup(int index)
    {
        if (index == 1) EndGame();
    }

    private IEnumerator CO_StartGame()
    {
        tv.PlayEvaluation();
        yield return new WaitForSeconds(4f);
        dialogueHandler.dialogueGroups[1] = passed ? winGroup : loseGroup;
        dialogueHandler.StartCurrentDialogueGroup();
    }

    private IEnumerator CO_EndGame()
    {
        elevator.MoveElevator(passed);
        DataManager.instance.AddRandomSymptom();
        yield return new WaitForSeconds(3f);

        LoadNextScene();
    }

    private void LoadNextScene()
    {
        if (!passed)
        {
            sceneHandler.LoadScene("BadEnding");
            return;
        }

        if (DataManager.instance.queuedScene == "BurnoutIntro")
        {
            sceneHandler.LoadScene(DataManager.instance.queuedScene);
            DataManager.instance.queuedScene = "Tutorial 2";
        }
        else if (DataManager.instance.queuedScene == "GoodEnding")
        {
            sceneHandler.LoadScene(DataManager.instance.queuedScene);
            DataManager.instance.queuedScene = "Title";
        }
        else
        {
            sceneHandler.LoadNextScene();
        }
    }
}
